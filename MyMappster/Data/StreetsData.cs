using MyMappster.Data.Models;
using Newtonsoft.Json;

namespace MyMappster.Data;

public static class StreetsData
{
    public static List<StreetRootObject> Streets { get; set; } = [];

    public static void LoadStreets(string filePath)
    {
        var json = File.ReadAllText(filePath);
        Streets = JsonConvert.DeserializeObject<List<StreetRootObject>>(json) ?? [];
    }
}