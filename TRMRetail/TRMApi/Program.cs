using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TRMApi.Data;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using Microsoft.OpenApi.Models;
using TRMDataManager.Library.DataAccess;
using TRMDataManager.Library.Internal.DataAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services
	.AddDbContext<ApplicationDbContext>(options =>
		options.UseSqlServer(connectionString))
	.AddDatabaseDeveloperPageExceptionFilter()
	.AddAuthentication(opt =>
	{
		opt.DefaultAuthenticateScheme = "JwtBearer";
		opt.DefaultChallengeScheme = "JwtBearer";

	}).AddJwtBearer("JwtBearer", jwtOptions =>
	{
		jwtOptions.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("Secrets:SecurityKey"))),
			ValidateIssuer = false,
			ValidateAudience = false,
			ValidateLifetime = true,
			ClockSkew = TimeSpan.FromMinutes(5) // tolerance for time checks
		};
	});

builder.Services
	.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
	.AddRoles<IdentityRole>()
	.AddEntityFrameworkStores<ApplicationDbContext>();

// Personal services

builder.Services
	.AddTransient<IInventoryData, InventoryData>()
	.AddTransient<ISQLDataAccess, SQLDataAccess>()
	.AddTransient<IProductData, ProductData>()
	.AddTransient<IProductData, ProductData>()
	.AddTransient<ISaleData, SaleData>()
	.AddTransient<IUserData, UserData>();

builder.Services.AddControllersWithViews();

builder.Services.AddSwaggerGen(setup =>
{
	setup.SwaggerDoc(
		"v1",
		new OpenApiInfo
		{
			Title = "MECCU RETAIL API",
			Version = "v1"
		}
		);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseMigrationsEndPoint();
}
else
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI( x =>
{
	x.SwaggerEndpoint("/swagger/v1/swagger.json",
		"Timco API v1");
});

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
