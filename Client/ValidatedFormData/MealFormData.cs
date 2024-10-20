using System.ComponentModel.DataAnnotations;

// Class used by the EditForm to validate, if required properties were set and if they have a proper format
// I am using System.ComponentModel.DataAnnotation with custom ValidationAttributes to validate the properties

public class MealFormData
{
    public int Id { get ; set; } // not set by user but having the fields allows for better conversion to MealDto

    private string _name = string.Empty;

    [Required(ErrorMessage = "Jméno pokrmu musí být zadáno.")]
    public required string Name 
    { 
        get => _name;
        set
        {
            _name = value.Trim(); // This removes leading or trailing white spaces when user enters them to the form (after user presses enter)
        }
    }
    
    [Required(ErrorMessage = "Typ pokrmu musí být zvolen.")]

    public required MealType MealType { get; set; }

    // In contrast to MealDto, this class contains list of allergen selections 
    // Allergen selections are a combination of the name of the allergen and indicator whether it was selected
    public IList<AllergenSelection>? AllergenSelections;

    // Conversion method used after form submit when sending request to api 
    public MealDto ToMealDto(MealTime mealTime, DateOnly date)
    {
        return new MealDto() 
            {
                Id = Id,
                Name = Name,
                MealTime = mealTime,
                Type = MealType,
                Date = date,
                // Add a corresponding AllergenDto only when the selection IsSelected
                Allergens = AllergenSelections!.Where(selection => selection.IsSelected).Select(selection => new AllergenDto {Name = selection.Name}).ToList()
            };
    }

    public static MealFormData CreateDefault()
    {
        return new MealFormData(){Name = string.Empty, MealType = MealType.Main};
    }
    
}


// Used to bind IsSelected property to the EditForm checkboxes
public class AllergenSelection
{
    public required string Name { get; init; }
    public bool IsSelected { get; set; }
}

public static class MealDtoExtensions
{
    // Conversion method from MealDto to MealFormData, used when passing data to editMealModal in Menu
    public static MealFormData ToMealFormData(this MealDto mealDto, IEnumerable<AllergenDto> AllAllergens)
    {
        var mealFormData = new MealFormData()
            {
                Id = mealDto.Id,
                Name = mealDto.Name,
                MealType = mealDto.Type,
                AllergenSelections = AllAllergens.Select(allergen => new AllergenSelection {Name = allergen.Name, IsSelected = false}).ToList()
            };
            
        // Mark all allergens of mealDto as true in AllergenSelections of the mealFormData
        foreach (var selection in mealFormData.AllergenSelections)
        {
            if (mealDto.Allergens.Select(allergen => allergen.Name).Contains(selection.Name))
            {
                selection.IsSelected = true;
            }
        }
        return mealFormData;
    }
}

