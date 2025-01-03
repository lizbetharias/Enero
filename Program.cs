using Clinica.Models;
using Clinica.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using iText.Kernel.Crypto;
using System.Security;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configurar la cadena de conexi�n y agregar el contexto de la base de datos
builder.Services.AddDbContext<BDContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar los servicios necesarios

builder.Services.AddControllersWithViews(options =>
{
    var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser() // Exige que el usuario est� autenticado
                    .Build();
    options.Filters.Add(new Microsoft.AspNetCore.Mvc.Authorization.AuthorizeFilter(policy)); // Aplica la pol�tica a todas las p�ginas por defecto
});
// Agregar soporte de memoria distribuida para sesiones
builder.Services.AddDistributedMemoryCache();

// Configurar las opciones de la sesi�n
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Ajusta el tiempo de expiraci�n de la sesi�n
    options.Cookie.HttpOnly = true;                 // Asegura que solo se pueda acceder a la cookie desde HTTP
    options.Cookie.IsEssential = true;              // Marca la cookie como esencial
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
   .AddCookie(options =>
   {
       options.LoginPath = "/Usuario/Login"; // P�gina de inicio de sesi�n
       options.LogoutPath = "/Usuario/Logout";       // P�gina de cierre de sesi�n
       options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Tiempo de expiraci�n de la cookie
       options.AccessDeniedPath = "/Usuario/UnauthorizedAlert"; // Nueva p�gina de acceso denegado
   });


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Cliente", policy => policy.RequireRole("Cliente"));
    options.AddPolicy("Administrador", policy => policy.RequireRole("Administrador"));
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

// Habilitar las sesiones y autenticaci�n
app.UseSession();
app.UseAuthentication(); // Debe ir antes de UseAuthorization
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

//el castleeee de pdf---------------
