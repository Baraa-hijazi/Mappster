using MyMappster.Data.Models;
using Newtonsoft.Json;

namespace MyMappster.Data;

public static class PostalCodesData
{
    public static List<RootObject> PostalCodes { get; set; } = [];

    public static void LoadPostalCodes(string filePath)
    {
        var json = File.ReadAllText(filePath);
        PostalCodes = JsonConvert.DeserializeObject<List<RootObject>>(json) ?? [];
    }
}