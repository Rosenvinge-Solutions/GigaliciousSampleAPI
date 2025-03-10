using GigaliciousSample.WebAPI.EndpointFilters.Extensions;
using GigaliciousSample.WebAPI.Middleware;
using GigaliciousSample.WebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connection = builder.Configuration.GetConnectionString("LocalDB");
builder.Services.AddScoped<IApiKeyService>(_ => new ApiKeyService(connection!));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseMiddleware<ApiKeyMiddleware>();
app.MapGet("/", () => "Hello World!")
    .RequireScope("Sample.Read");
app.MapGet("/restricted", (HttpContext context) => "I wrote, Hello World!")
    .RequireScope("Sample.Write");

app.Run();
