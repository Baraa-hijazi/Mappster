using MyMappster.Data;
using MyMappster.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddControllers();

var app = builder.Build();

PostalCodesData.LoadPostalCodes("wwwroot/Trans-Postal-Code.json");
StreetsData.LoadStreets("wwwroot/Trans-Street-Address.json");
PointsData.LoadPoints("wwwroot/Trans-Address-Points.json");
AreasData.LoadAreas("wwwroot/Trans-Areas.json");

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();