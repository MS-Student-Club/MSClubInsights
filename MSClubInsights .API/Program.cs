using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MSClubInsights.Domain.Entities.Identity;
using MSClubInsights.Infrastructure.DependancyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddInfrastructureServices(builder.Configuration);


//Adding CustomServices

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My Custom API", // ✅ This changes the main API title in Swagger UI
        Version = "v1",
        Description = "A detailed API documentation."
    });
});
var app = builder.Build();


    app.MapOpenApi();


app.UseHttpsRedirection();
app.UseStaticFiles(); // Enables serving static files from wwwroot

app.UseSwaggerUI(o => 
{
  o.SwaggerEndpoint("/openapi/v1.json", "MS Club Insights API v1");
  o.DocumentTitle = "MS Club Insights API";
  o.HeadContent = "<style>.swagger-ui .topbar { background-color: #5298DF; }</style>"; // Changes the header color
  o.InjectStylesheet("/swagger-custom.css"); // Inject custom CSS

});


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
}
app.UseAuthorization();
app.UseRateLimiter();
app.MapControllers();

app.Run();


