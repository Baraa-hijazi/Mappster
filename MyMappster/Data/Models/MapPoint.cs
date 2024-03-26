using System.Text.Json.Serialization;

namespace MyMappster.Data.Models;

public class MapPoint
{
    [JsonPropertyName("Community_Name_En")]
    public string? CommunityNameEn { get; set; }
    
    [JsonPropertyName("Community_Name_Ar")]
    public string? CommunityNameAr { get; set; }
    
    [JsonPropertyName("Area_Name_En")]
    public string? AreaNameEn { get; set; }
    
    [JsonPropertyName("Area_Name_Ar")]
    public string? AreaNameAr { get; set; }
    
    [JsonPropertyName("Region")]
    public string? Region { get; set; }
    
    [JsonPropertyName("Emirate")]
    public string? Emirate { get; set; }
    
    [JsonPropertyName("PostCode")]
    public int? PostCode { get; set; }
    
    [JsonPropertyName("Country_Name")]
    public string? CountryName { get; set; }
    
    [JsonPropertyName("Full_Road_Name_En")]
    public string? FullRoadNameEn { get; set; }
    
    [JsonPropertyName("Full_Road_Name_Ar")]
    public string? FullRoadNameAr { get; set; }
    
    [JsonPropertyName("Road_Type")]
    public string? RoadType { get; set; }
    
    [JsonPropertyName("Side")]
    public string? Side { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }
}