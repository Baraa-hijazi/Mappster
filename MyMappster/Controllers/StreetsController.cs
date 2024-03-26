using MyMappster.Data;
using Microsoft.AspNetCore.Mvc;
using MyMappster.Data.Models;

namespace MyMappster.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StreetsController : ControllerBase
{
    private const double Tolerance = 0.001;

    [HttpGet]
    public ActionResult<StreetResponse> Get(double lat, double lng)
    {
        var streets = StreetsData.Streets;
        
        var matchingStreet = streets.FirstOrDefault(s => IsPointNearStreet(lat, lng, s.JsonGeometry.Coordinates));
        
        if (matchingStreet != null)
        {
            var response = new StreetResponse
            {
                StreetData = matchingStreet.JsonGeometry,
                FullStNameEn = matchingStreet.FullStNameEn,
                FullStNameAr = matchingStreet.FullStNameAr
            };

            return Ok(response);
        }

        return NotFound();
    }

    private bool IsPointNearStreet(double lat, double lng, List<List<double>> streetCoordinates)
    {
        for (var i = 0; i < streetCoordinates.Count - 1; i++)
        {
            var start = streetCoordinates[i];
            var end = streetCoordinates[i + 1];
            if (IsPointNearLineSegment(lat, lng, start[1], start[0], end[1], end[0]))
                return true;
        }
        return false;
    }

    private bool IsPointNearLineSegment(double pointLat, double pointLng, double startLat, double startLng, double endLat, double endLng)
    {
        var d = DistanceToLineSegment(pointLat, pointLng, startLat, startLng, endLat, endLng);
        return d <= Tolerance;
    }

    private double DistanceToLineSegment(double pointLat, double pointLng, double startLat, double startLng,
        double endLat, double endLng)
    {
        var dx = endLng - startLng;
        var dy = endLat - startLat;
        var l2 = dx * dx + dy * dy;
        var t = ((pointLng - startLng) * dx + (pointLat - startLat) * dy) / l2;

        if (t < 0) return Math.Sqrt((pointLng - startLng) * (pointLng - startLng) + (pointLat - startLat) * (pointLat - startLat));
        if (t > 1) return Math.Sqrt((pointLng - endLng) * (pointLng - endLng) + (pointLat - endLat) * (pointLat - endLat));

        var projectionLng = startLng + t * dx;
        var projectionLat = startLat + t * dy;

        return Math.Sqrt((pointLng - projectionLng) * (pointLng - projectionLng) + (pointLat - projectionLat) * (pointLat - projectionLat));
    }
}