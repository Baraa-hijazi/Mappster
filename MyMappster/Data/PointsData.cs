using MyMappster.Data.Models;
using Newtonsoft.Json;

namespace MyMappster.Data;

public static class PointsData
{
    public static List<MapPoint> Points { get; set; } = [];

    public static void LoadPoints(string filePath)
    {
        var json = File.ReadAllText(filePath);
        Points = JsonConvert.DeserializeObject<List<MapPoint>>(json) ?? [];
    }
}