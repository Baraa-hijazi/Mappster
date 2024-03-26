namespace MyMappster.Data.Models;

public class JsonGeometry
{
    public string Type { get; set; } = null!;
    public List<List<List<double>>> Coordinates { get; set; } = [];
}
