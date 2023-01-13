using BookStore.DataAccess.Contracts;
using BookStore.DataAccess.DataContext;
using BookStore.DataAccess.Repositories;
using BookStore.Services.Contracts;
using BookStore.Services.Managers;
using BookStore.Services.ShopService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SmartBreadcrumbs.Extensions;
using System;
using System.Reflection;

namespace BookStore.Web
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
            services.AddDbContextPool<BookStoreContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("DbConnection")));

            services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
            services.AddScoped<IFileService, FilesService>();
            services.AddScoped<IShopManager, ShopManager>();
            services.AddAutoMapper(typeof(Startup));
            services.AddMvc();
            //services.AddControllersWithViews();
            services.AddDistributedMemoryCache();   
            services.AddSession(options => { 
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            
            services.AddBreadcrumbs(Assembly.GetExecutingAssembly(), options =>
            {
                options.TagName = "nav";
                options.TagClasses = "";
                options.OlClasses = "breadcrumb";
                options.LiClasses = "breadcrumb-item";
                options.ActiveLiClasses = "breadcrumb-item active";
                options.SeparatorElement = "<li class=\"separator\">/</li>";
            });
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();           
            app.UseRouting();
            //app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Book}/{action=Index}/{id?}");
            });
        }
    }
}
