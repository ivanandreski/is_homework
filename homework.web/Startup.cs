using homework.Domain;
using homework.Domain.Models;
using homework.Repository;
using homework.Repository.Implementation;
using homework.Repository.Interface;
using homework.Service.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace homework.web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;



            // Add compatibility switch.
            AppContext.SetSwitch("Switch.System.Windows.Media.ShouldRenderEvenWhenNoDisplayDevicesAreAvailable", true);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            //options.UseSqlServer(
            //    Configuration.GetConnectionString("DefaultConnection")));
                options.UseSqlite(@"DataSource=mydatabase.db;"));
            services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddTransient<IScreaningRepository, Repository.Implementation.ScreaningRepository>();
            services.AddTransient<ITicketRepository, Repository.Implementation.TicketRepository>();
            services.AddTransient<IUserRepository, Repository.Implementation.UserRepository>();
            services.AddTransient<IShoppingCartRepository, Repository.Implementation.ShoppingCartRepository>();
            services.AddTransient<IOrderItemRepository, Repository.Implementation.OrderItemRepository>();

            services.AddTransient<IScreaningService, Service.Implementation.ScreaningService>();
            services.AddTransient<IMovieService, Service.Implementation.MovieService>();
            services.AddTransient<ITicketService, Service.Implementation.TicketService>();
            services.AddTransient<IShoppingCartService, Service.Implementation.ShoppingCartService>();

            // Stripe
            services.Configure<StripeSettings>(Configuration.GetSection("Stripe"));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Stripe
            StripeConfiguration.ApiKey = Configuration.GetSection("Stripe")["SecretKey"];

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
