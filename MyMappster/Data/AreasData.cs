using MyMappster.Data.Models;
using Newtonsoft.Json;

namespace MyMappster.Data;

public static class AreasData
{
    public static List<RootObject> Areas { get; set; } = [];

    public static void LoadAreas(string filePath)
    {
        var json = File.ReadAllText(filePath);
        Areas = JsonConvert.DeserializeObject<List<RootObject>>(json) ?? [];
    }
}