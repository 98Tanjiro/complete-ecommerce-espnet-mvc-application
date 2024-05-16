using Ecinema.Data;
using Ecinema.Data.Cart;
using Ecinema.Data.Services;
using Ecinema.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Ecinema.Data.ViewModels;

var builder = WebApplication.CreateBuilder(args);

//DbContext configuration
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>{options.SignIn.RequireConfirmedAccount = true;}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();



//Services configuration 
builder.Services.AddScoped<IActorsService, ActorsService>();
builder.Services.AddScoped<IMoviesService, MoviesService>();
builder.Services.AddScoped<ICinemasService, CinemasService>();
builder.Services.AddScoped<IOrdersService, OrdersService>();
builder.Services.AddScoped<IProducersService, ProducersService>();
builder.Services.AddScoped<ShoppingCart>();
builder.Services.AddScoped<LoginVM, LoginVM>();
builder.Services.AddScoped<RegisterVM, RegisterVM>();


// Add services to the container.
builder.Services.AddControllersWithViews();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
//seed database
AppDbInitializer.Seed(app);

app.Run();
