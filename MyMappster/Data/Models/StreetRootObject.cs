using Newtonsoft.Json;

namespace MyMappster.Data.Models;

public class StreetRootObject
{
    [JsonProperty("json_geometry")]
    public StreetJsonGeometry JsonGeometry { get; set; } = null!;

    [JsonProperty("Full_St_Name_En")]
    public string FullStNameEn { get; set; } = null!;

    [JsonProperty("Full_St_Name_Ar")]
    public string FullStNameAr { get; set; } = null!;
}