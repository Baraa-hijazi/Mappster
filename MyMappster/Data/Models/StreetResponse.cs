using System.Text.Json.Serialization;

namespace MyMappster.Data.Models;

public class StreetResponse
{
    [JsonPropertyName("Full_St_Name_En")]
    public string FullStNameEn { get; set; } = null!;
    
    [JsonPropertyName("Full_St_Name_Ar")]
    public string FullStNameAr { get; set; } = null!;

    public StreetJsonGeometry StreetData { get; set; } = null!;
}