
using Marketplace;
using Marketplace.Api;
using Marketplace.Domain;
using Marketplace.Framework;

var builder = WebApplication.CreateBuilder(args);
var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .Build();

// Add your services here if needed
builder.Services.AddSingleton<IConfiguration>(config);
var store = DocumentStoreHolder.CreateStore();

builder.Services.AddTransient(c => store.OpenAsyncSession());
builder.Services.AddSingleton<ICurrencyLookup, FixedCurrencyLookup>();
builder.Services.AddScoped<IClassifiedAdRepository, ClassifiedAdRepository>();
builder.Services.AddSingleton<ClassifiedAdApplicationService>();

// Configure the app
builder.WebHost.UseConfiguration(config);
builder.WebHost.UseContentRoot(Directory.GetCurrentDirectory());
builder.WebHost.ConfigureKestrel(options =>
{
    // Configure Kestrel if needed
});
builder.Services.AddEndpointsApiExplorer();
// Configure Swagger
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();

// Configure Swagger UI
if (app.Environment.IsDevelopment())
{
    
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Marketplace V1");
        options.RoutePrefix = string.Empty;
    });
}

app.MapControllers();
// Add other middleware and configure the app's request pipeline here

app.Run();