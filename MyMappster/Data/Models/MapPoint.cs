using Newtonsoft.Json;

namespace MyMappster.Data.Models;

public class MapPoint
{
    [JsonProperty(PropertyName = "Community_Name_En")]
    public string? CommunityNameEn { get; set; }

    [JsonProperty(PropertyName = "Community_Name_Ar")]
    public string? CommunityNameAr { get; set; }

    [JsonProperty(PropertyName = "Area_Name_En")]
    public string? AreaNameEn { get; set; }

    [JsonProperty(PropertyName = "Area_Name_Ar")]
    public string? AreaNameAr { get; set; }

    [JsonProperty(PropertyName = "Region")]
    public string? Region { get; set; }

    [JsonProperty(PropertyName = "Emirate")]
    public string? Emirate { get; set; }

    [JsonProperty(PropertyName = "PostCode")]
    public int? PostCode { get; set; }

    [JsonProperty(PropertyName = "Country_Name")]
    public string? CountryName { get; set; }

    [JsonProperty(PropertyName = "Full_Road_Name_En")]
    public string? FullRoadNameEn { get; set; }

    [JsonProperty(PropertyName = "Full_Road_Name_Ar")]
    public string? FullRoadNameAr { get; set; }

    [JsonProperty(PropertyName = "Road_Type")]
    public string? RoadType { get; set; }

    [JsonProperty(PropertyName = "Side")]
    public string? Side { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }
}