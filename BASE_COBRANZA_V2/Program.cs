using BASE_COBRANZA_V2.Models.Interfaces;
using BASE_COBRANZA_V2.Models.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.CookiePolicy;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IProcurador, RepoProcurador>();
builder.Services.AddSingleton<IPasos_cobranza, RepoPasos_cobranza>();
builder.Services.AddSingleton<IUsuario, RepoUsuario>();
builder.Services.AddSingleton<IRol, RepoRol>();
builder.Services.AddSingleton<ITipo_impulso, RepoTipo_impulso>();
builder.Services.AddSingleton<IStatus_arbitraje, RepoStatus_arbitraje>();
builder.Services.AddSingleton<IStatus_judicial, RepoStatus_judicial>();
builder.Services.AddSingleton<IStatus_poder_judicial, RepoStatus_poder_judicial>();
builder.Services.AddSingleton<IApoderado, RepoApoderado>();
builder.Services.AddSingleton<IDemanda_principal, RepoDemanda_principal>();

//Configurar autenticacion....
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option => {
        //ruta pagina de logueo..
        option.LoginPath = "/Autenticacion/Logueo";
        //ruta mensaje para acceso no autorizado
        option.AccessDeniedPath = "/Autenticacion/Mensaje";

    });//fin de la configuracion para autenticacion...
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireLoggedIn", policy => policy.RequireAuthenticatedUser());
});
//limitamos el envío de las cookies
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.Strict;
    options.HttpOnly = HttpOnlyPolicy.Always;
  
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Autenticacion}/{action=Logueo}/{id?}");

app.Run();
