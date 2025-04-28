using OxxoWeb.Models;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.StaticFiles;

var builder = WebApplication.CreateBuilder(args);

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowUnityWebGL", policy =>
    {
        policy.WithOrigins("http://localhost:5110") // Cambia esto por el dominio donde se ejecuta el juego
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Configurar sesión
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Agregar servicios al contenedor
builder.Services.AddRazorPages();
builder.Services.AddScoped<MoniDataBaseContext>();
builder.Services.AddScoped<DataBaseContextPerfil>();
builder.Services.AddScoped<AdolfoDatabaseContext>();
builder.Services.AddScoped<RecordatoriosDataBaseContext>();

var app = builder.Build();

// Configurar el pipeline de solicitudes HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

// Configurar tipos MIME para WebGL
var provider = new FileExtensionContentTypeProvider();
provider.Mappings[".data"] = "application/octet-stream";
provider.Mappings[".wasm"] = "application/wasm";

app.UseStaticFiles(new StaticFileOptions
{
    ContentTypeProvider = provider
});

// Usar CORS
app.UseCors("AllowUnityWebGL");

app.UseRouting();

app.UseAuthorization();
app.UseSession();

// Mapear rutas
app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
