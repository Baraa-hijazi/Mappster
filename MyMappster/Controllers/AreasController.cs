using MyMappster.Data;
using MyMappster.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace MyMappster.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AreasController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<AreaResponse>> Get(double lat, double lng)
    {
        // check if the point is within any of the areas
        var areas = AreasData.Areas;
        foreach (var area in areas)
        {
            if (!IsPointInPolygon(lat, lng, area.JsonGeometry.Coordinates)) continue;

            var response = new AreaResponse
            {
                AreaName = area.AreaName,
                NameArabi = area.NameArabi,
                PolygonData = area.JsonGeometry
            };

            return Ok(response);
        }

        return NotFound();
    }

    private static bool IsPointInPolygon(double pointLat, double pointLng, List<List<List<double>>> polygon)
    {
        var isInside = false;
        for (int i = 0, j = polygon[0].Count - 1; i < polygon[0].Count; j = i++)
        {
            double xi = polygon[0][i][0], yi = polygon[0][i][1];
            double xj = polygon[0][j][0], yj = polygon[0][j][1];

            var intersect = yi > pointLat != yj > pointLat &&
                            pointLng < (xj - xi) * (pointLat - yi) / (yj - yi) + xi;

            if (intersect) isInside = !isInside;
        }

        return isInside;
    }
}

public class AreaResponse
{
    public string AreaName { get; set; } = null!;
    public string NameArabi { get; set; } = null!;
    public JsonGeometry PolygonData { get; set; } = null!;
}