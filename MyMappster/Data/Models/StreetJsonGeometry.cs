namespace MyMappster.Data.Models;

public class StreetJsonGeometry
{
    public string Type { get; set; } = null!;
    public List<List<double>> Coordinates { get; set; } = [];
}