using MyMappster.Data.Models;
using MyMappster.Data;
using Microsoft.AspNetCore.Mvc;

namespace MyMappster.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PointsController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<MapPoint>> Get(double lat, double lng)
    {
        var points = PointsData.Points;

        const double tolerance = 0.0002;
        var filteredPoints = points.AsParallel().Where(p =>
            Math.Abs(p.Latitude - lat) < tolerance && Math.Abs(p.Longitude - lng) < tolerance).ToList();

        return Ok(filteredPoints);
    }
}