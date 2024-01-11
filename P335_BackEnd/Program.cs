using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using P335_BackEnd.Data;
using P335_BackEnd.Entities;
using P335_BackEnd.Helper;
using P335_BackEnd.Services;

namespace P335_BackEnd
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllersWithViews();

			builder.Services.AddDbContext<AppDbContext>(opt =>
			{
				opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
			});
			builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
			{
				opt.Password.RequiredLength = 4;
				opt.Password.RequireNonAlphanumeric = false;
				opt.Password.RequireUppercase = false;
				opt.Password.RequireLowercase = false;
			}).AddEntityFrameworkStores<AppDbContext>();

			builder.Services.AddScoped<ProductService>();

			var app = builder.Build();

			await DataSeed.InitializeAsync(app.Services);

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();
            
            app.MapControllerRoute(
                  name: "areas",
                  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );

            app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.Run();
		}
	}
}
