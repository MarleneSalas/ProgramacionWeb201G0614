using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using U3A1.Models.Entities;
using U3A1.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<Repository<Clasificacion>>();
builder.Services.AddTransient<Repository<Menu>>();
builder.Services.AddTransient<HamburguesasRepository>();

builder.Services.AddDbContext<NeatContext>(x=> 
x.UseMySql("server=localhost;user=root;password=root;database=Neat", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.31-mysql")));

builder.Services.AddMvc();
var app = builder.Build();
app.UseFileServer();

app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapDefaultControllerRoute();
app.UseStaticFiles();
app.Run();
