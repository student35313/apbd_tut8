namespace Tutorial8.Models.DTOs;

public class CountryDTO
{
    public int IdCountry { get; set; }
    public string Name { get; set; }
    public List<string> Trips { get; set; } = new(); 
}