using Microsoft.EntityFrameworkCore;
using votek.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DataContext>(options =>{
   
    var config = builder.Configuration;
    var connectionString = config.GetConnectionString("database");
    options.UseMySQL(connectionString);
    
});


builder.Services.AddScoped<IUserRepository, EfUserRepository>();


builder.Services.AddAuthentication("MyCookieAuth")
    .AddCookie("MyCookieAuth", options =>
    {
        options.Cookie.Name = "MyAuthCookie";
        options.LoginPath = "/Sign/Login"; // Oturum açılmadığında yönlendirilecek sayfa
        options.AccessDeniedPath = "/Sign/AccessDenied"; // Yetkisiz erişimlerde yönlendirilecek sayfa
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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();