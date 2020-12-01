using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using RentalKendaraan_072.Models;

namespace RentalKendaraan_072
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IFileProvider>(
                new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<Models.RentKendaraanContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Default")));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //services.AddDefaultIdentity<IdentityUser>()
            //    .AddEntityFrameworkStores<rental_kendaraanContext>();
            services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultUI()
                .AddEntityFrameworkStores<RentKendaraanContext>().AddDefaultTokenProviders();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("readonlypolicy",
                    builder => builder.RequireRole("Admin", "Manager", "Kasir"));
                options.AddPolicy("writepolicy",
                    builder => builder.RequireRole("Admin", "Kasir"));
                options.AddPolicy("editpolicy",
                    builder => builder.RequireRole("Admin", "Kasir"));
                options.AddPolicy("deletepolicy",
                    builder => builder.RequireRole("Admin", "Kasir"));
            });
            services.AddScoped<Peminjaman>();
            services.AddScoped<Pengembalian>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}