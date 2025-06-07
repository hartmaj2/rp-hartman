using Microsoft.AspNetCore.Mvc;
using System.Text.Json;


[ApiController] // marks this class so it can be added to server controllers using builder.Services.AddControllers();
[Route("api/participants")]

public class ParticipantFileController : ControllerBase
{

    private static readonly List<ParticipantDto> _participants;
    private static int _nextId = 0;

    static ParticipantFileController()
    {
        _participants = LoadParticipantsFromFile();
        SetIds(_participants);
    }

    private static List<ParticipantDto> LoadParticipantsFromFile()
    {
        string participantFilePath = "TestRequests/ParticipantsPopulate.http";

        var text = System.IO.File.ReadLines(participantFilePath).Skip(3);
        var json_text = string.Join("\n",text);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        return JsonSerializer.Deserialize<List<ParticipantDto>>(json_text,options)!;

    }

    private static void SetIds(List<ParticipantDto> participants)
    {
        foreach (var p in participants)
        {
            p.Id = _nextId++;
        }
    }

    // Gets participant by id, it is important to eagerly load the participant Allergens before we convert him (the database does not automatically include allergens)
    [HttpGet("{id:int}")]
    public ParticipantDto? GetParticipantById(int id)
    {
        return _participants.Find(p => p.Id == id);
    }

    // Gets the list of participants from the participant table
    // Also transfers them to dtos so allergens are sent as list of allergendtos
    [HttpGet("all")]
    public IEnumerable<ParticipantDto> GetParticipants()
    {
        return _participants;
    }

    [HttpPost("edit/{id:int}")]
    public IActionResult EditParticipant(int id, [FromBody] ParticipantDto updatedParticipant)
    {
        DeleteParticipant(id);
        return AddParticipant(updatedParticipant);
    }

    // Adds a participant to the participant table
    [HttpPost("add")]
    public IActionResult AddParticipant([FromBody] ParticipantDto participantDto)
    {
        _participants.Add(participantDto);
        return CreatedAtAction(nameof(GetParticipants),participantDto);
    }

    // Adds a whole list of participants
    [HttpPost("add-many")]
    public IActionResult AddMultipleParticipants([FromBody] ICollection<ParticipantDto> participants)
    {
        foreach (var p in participants)
        {
            _participants.Add(p);
        }
        return CreatedAtAction(nameof(GetParticipants),participants);
    }

    // Deletes single participant with given id
    [HttpDelete("delete/{id:int}")]
    public IActionResult DeleteParticipant(int id)
    {
        var i = _participants.FindIndex(p => p.Id == id);
        _participants.RemoveAt(i);
        return NoContent();
    }

    // Deletes everything from the participant table
    [HttpDelete("delete-all")]
    public IActionResult DeleteAllParticipantsDb()
    {
        _participants.Clear();
        return NoContent();
    }

}