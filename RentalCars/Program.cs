using RentalCars.Data;
using RentalCars.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddDbContext<RentalContext>();
builder.Services.AddScoped<RentalService>(); 

var app = builder.Build();

// Get the service scope factory
var serviceScopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();

// Create a new scope
using (var scope = serviceScopeFactory.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<RentalContext>();

    DbInitializer.Initialize(context);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();