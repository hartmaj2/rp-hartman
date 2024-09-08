// Participant model for communication of participant diets between Client and Server
// I want to communicate allergens as list of Allergens

using Shared;

public class ParticipantDto
{

    public int Id { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public int? Age { get; set; }

    public string? PhoneNumber {get; set; }

    public required string BirthNumber {get; set; }

    public required List<AllergenDto> Allergens { get; set; }

}

public static class ParticipantExtensions
{
    // Converts a Participant to Participant diets dto, the navigation properties ParticipantAllergens and Allergen must be loaded from db explicitly using Include
    public static ParticipantDto ConvertToParticipantDto(this Participant thisParticipant)
    {
        return new ParticipantDto()
        {
            Id = thisParticipant.Id,
            FirstName = thisParticipant.FirstName,
            LastName = thisParticipant.LastName,
            Age = thisParticipant.Age,
            PhoneNumber = thisParticipant.PhoneNumber,
            BirthNumber = thisParticipant.BirthNumber,
            Allergens = thisParticipant.Diets!.Select(pa => pa.Allergen!.ToAllergenDto()).ToList()
        };
    }
}

