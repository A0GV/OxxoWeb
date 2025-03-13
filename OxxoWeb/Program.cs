// ADDING SERVICE NAMESPACE
using OxxoWeb.Models; // Add
using MySql.Data.MySqlClient; // Add


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Register MoniDatabaseContext as a service
builder.Services.AddScoped<MoniDataBaseContext>(); // Add recommended para agregar services 
builder.Services.AddScoped<DataBaseContextPerfil>(); // Add DataBaseContext


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
