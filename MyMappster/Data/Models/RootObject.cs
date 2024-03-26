using Newtonsoft.Json;

namespace MyMappster.Data.Models;

public class RootObject
{
    [JsonProperty("json_featuretype")]
    public string JsonFeatureType { get; set; } = null!;
    
    public int ObjectId { get; set; }
    
    [JsonProperty("name_arabi")]
    public string NameArabi { get; set; } = null!;

    [JsonProperty("area_name")]
    public string AreaName { get; set; } = null!;
    
    [JsonProperty("shape_length")]
    public double ShapeLength { get; set; }
    
    [JsonProperty("shape_area")]
    public double ShapeArea { get; set; }
    
    [JsonProperty("json_ogc_wkt_crs")]
    public string JsonOgcWktCrs { get; set; } = null!;
    
    [JsonProperty("json_geometry")]
    public JsonGeometry JsonGeometry { get; set; } = null!;

    public double EmPostCode { get; set; }

    public double EmPostAreaCode { get; set; }
    
    [JsonProperty("Full_St_Name_En")]
    public string FullStNameEn { get; set; } = null!;
    
    [JsonProperty("Full_St_Name_Ar")]
    public string FullStNameAr { get; set; } = null!;
}