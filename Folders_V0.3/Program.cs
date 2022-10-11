using Microsoft.EntityFrameworkCore;
using Folders;   // class Application Context namespace

var builder = WebApplication.CreateBuilder(args);

//getting connection string from appsetings.json
string connection = builder.Configuration.GetConnectionString("DefaultConnection");

//adding Application Context as service in order to use it in controllers
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlite(connection));

builder.Services.AddMvc();

var app = builder.Build();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();