using System;
using AutoParkData;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AutoParkData.Repositories;
using AutoParkData.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace AutoParkWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var dbConnectionString = Configuration.GetConnectionString("AutoParkDB");
            var dbCreationScript = Configuration["DataBaseCreationScriptPath"];

            DbCreator.EnsureDbCreated(dbConnectionString, dbCreationScript);

            services.AddScoped<IVehicleTypeRepository, VehicleTypeRepository>(
                provider => new VehicleTypeRepository(dbConnectionString));

            services.AddScoped<IVehicleRepository, VehicleRepository>(
                provider => new VehicleRepository(dbConnectionString));

            services.AddScoped<ISparePartRepository, SparePartRepository>(provider =>
                new SparePartRepository(dbConnectionString));

            services.AddScoped<IOrderRepository, OrderRepository>(provider =>
                new OrderRepository(dbConnectionString));

            services.AddScoped<IOrderItemRepository, OrderItemRepository>(provider =>
                new OrderItemRepository(dbConnectionString));

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
