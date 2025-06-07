using Microsoft.AspNetCore.Mvc;
using System.Text.Json;


[ApiController]
[Route("api/meals")]

// Used to handle all request for creating, deleting or editing meals
// Also provides way to bulk add meals from single json request or getting the meals for a given date only
public class MealsController : ControllerBase
{

    private static readonly List<MealDto> _meals;

    private static int _nextId = 0;

    // The context gets injected automatically using dependency injection
    static MealsController()
    {
        _meals = LoadMealsFromFile();
        SetIds(_meals);
    }

    private static List<MealDto> LoadMealsFromFile()
    {
        var path = "TestRequests/MealsPopulate.http";
        var text = System.IO.File.ReadLines(path).Skip(4);
        var json_text = string.Join("\n", text);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        return JsonSerializer.Deserialize<List<MealDto>>(json_text,options)!; 

    }

    private static void SetIds(List<MealDto> meals)
    {
        foreach (var m in meals)
        {
            m.Id = _nextId++;
        }
    }

    // Gets the list of all meals from the meals table
    // I have to map all Meal objects to MealDtos because I want to be getting a list of AllergenDtos for every meal (I don't want to be getting the db representation directly)
    // (also uses eager loading with Include and ThenInclude)
    [HttpGet("all")]
    public IEnumerable<MealDto> GetAllMeals()
    {
        return _meals;
    }

    // Adds a meal to the Meals table
    [HttpPost("add")]
    public IActionResult AddMeal([FromBody] MealDto received)
    {
        _meals.Add(received);
        return CreatedAtAction(nameof(GetAllMeals),received);
    }

    // Adds a list of meals to the Meals table
    [HttpPost("add-many")]
    public IActionResult AddMeals([FromBody] ICollection<MealDto> receiveds)
    {
        foreach (var meal in receiveds)
        {
            _meals.Add(meal);
        }
        return CreatedAtAction(nameof(GetAllMeals),receiveds);
    }



    // Uses eager loading for navigation entity MealAllergens (navigation entity represents a relationship to another entity or collection of entities)
    // The code would not work without it when using ToMealDto function (the mealAllergens collection would appear empty)
    [HttpGet("{date}")]
    public IEnumerable<MealDto> GetMealsByDate(DateOnly date)
    {
        return _meals.Where(meal => meal.Date == date);
    }

    // Gets the list of names of possible enum values for meal type
    // Used by the MealService of Client
    [HttpGet("meal-types")]
    public IEnumerable<string> GetMealTypes()
    {
        return Enum.GetNames<MealType>();
    }

    [HttpPost("edit/{id:int}")]
    public IActionResult EditMeal(int id, [FromBody] MealDto updatedMeal)
    {
        DeleteMeal(id);
        return AddMeal(updatedMeal);
    }

    // Deletes single meal based on id
    [HttpDelete("{id:int}")]
    public IActionResult DeleteMeal(int id)
    {
        var i = _meals.FindIndex(meal => meal.Id == id);
        _meals.RemoveAt(i);
        return NoContent();
    }

    // Deletes everything from the meals table
    [HttpDelete("delete-all")]
    public IActionResult DeleteAllMeals()
    {
        _meals.Clear();
        return NoContent();
    }

}