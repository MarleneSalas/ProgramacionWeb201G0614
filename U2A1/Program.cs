var builder = WebApplication.CreateBuilder(args);
//Patron mvcA
builder.Services.AddMvc();

var app = builder.Build();
//Archivos estaticos
app.UseStaticFiles();

//Enrutamiento por defecto
app.MapDefaultControllerRoute();
app.Run();
