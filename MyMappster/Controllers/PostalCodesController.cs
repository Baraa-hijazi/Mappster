using System.Globalization;
using MyMappster.Data.Models;
using MyMappster.Data;
using Microsoft.AspNetCore.Mvc;

namespace MyMappster.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostalCodesController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<AreaResponse>> Get(double lat, double lng)
    {
        var postalCodes = PostalCodesData.PostalCodes;
        foreach (var postalCode in postalCodes)
        {
            if (!IsPointInPolygon(lat, lng, postalCode.JsonGeometry.Coordinates)) continue;

            var response = new PostalCodeResponse
            {
                EmPostCode = postalCode.EmPostCode.ToString(CultureInfo.InvariantCulture),
                EmPostAreaCode = postalCode.EmPostAreaCode.ToString(CultureInfo.InvariantCulture),
                ShapeLength = postalCode.ShapeLength,
                ShapeArea = postalCode.ShapeArea,
                PolygonData = postalCode.JsonGeometry
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

            var intersect = ((yi > pointLat) != (yj > pointLat)) &&
                            (pointLng < (xj - xi) * (pointLat - yi) / (yj - yi) + xi);
            if (intersect)
            {
                isInside = !isInside;
            }
        }

        return isInside;
    }
}

public class PostalCodeResponse
{
    public string EmPostCode { get; set; } = null!;

    public string EmPostAreaCode { get; set; } = null!;

    public double ShapeLength { get; set; }

    public double ShapeArea { get; set; }

    public JsonGeometry PolygonData { get; set; } = null!;
}