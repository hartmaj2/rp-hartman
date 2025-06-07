using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

[ApiController]
[Route("api/allergens")]

// Used to handle requests related to allergens/diets that the meals/participants can contain/have
public class AllergensController : ControllerBase
{

    private static readonly List<AllergenDto> _allergens;

    private static int _nextId = 0;

    static AllergensController()
    {
        _allergens = LoadAllergensFromFile();
        SetIds(_allergens);
    }

    private static List<AllergenDto> LoadAllergensFromFile()
    {
        string pathToAllergenFile = "TestRequests/AllergensPopulate.http";
        var text = System.IO.File.ReadLines(pathToAllergenFile).Skip(3);
        var json_text = string.Join("\n", text);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

    
        return JsonSerializer.Deserialize<List<AllergenDto>>(json_text, options)!;
 

    }

    private static void SetIds(List<AllergenDto> allergens)
    {
        foreach (var a in allergens)
        {
            a.Id = _nextId++;
        }
    }

    // Gets the list of all allergens from the Allergens table
    [HttpGet("all")]
    public IEnumerable<AllergenDto> GetAllAllergens()
    {
        return _allergens;
    }

    // Adds an allergen to the Allergens table
    [HttpPost("add")]
    public IActionResult AddNewAllergen([FromBody] AllergenDto allergen)
    {
        _allergens.Add(allergen);
        return CreatedAtAction(nameof(GetAllAllergens),allergen);
    }

    // Adds a whole list of allergens to the Allergens table (useful when populating empty table)
    [HttpPost("add-many")]
    public IActionResult AddMultipleAllergens([FromBody] ICollection<AllergenDto> allergens)
    {
        foreach (var p in allergens)
        {
            _allergens.Add(p);
        }
        return CreatedAtAction(nameof(GetAllAllergens),allergens);
    }

    // Deletes single allergen with given id from the Allergens table
    [HttpDelete("delete/{id:int}")]
    public IActionResult DeleteAllergen(int id)
    {
        var i = _allergens.FindIndex(p => p.Id == id);
        _allergens.RemoveAt(i);
        return NoContent();
    }

    // Deletes everything from the Allergens table 
    // (this is better than truncating because it takes foreign keys into account and uses cascading to remove the corresponding values in associative tables)
    [HttpDelete("delete-all")]
    public IActionResult DeleteAllAllergens()
    {
        _allergens.Clear();
        return NoContent();
    }

}