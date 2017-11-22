using Core;
using Core.Entities;
using ManagementStocks.Core.Entities;
using ManagementStocks.Core.Interfaces;
using ManagementStocks.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;
using Persistance;

namespace StockMVC
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
            if (Configuration["UsesDapper"].ToLower() == "true")
            {
                services.AddTransient<IQueryRepository<Product>, ProductsQueryRepositoryDapper>();
                services.AddTransient<IStocksQueryRepository, StocksQueryRepositoryDapper>();
            }
            else
            {
                services.AddTransient<IQueryRepository<Product>, ProductsQueryRepository>();
                services.AddTransient<IStocksQueryRepository, StocksQueryRepository>();
            }
            services.AddTransient<IDatabaseContext, DatabaseContext>();
            services.AddTransient<ICommandRepository<Product>, ProductsCommandRepository>();
            services.AddTransient<ICommandRepository<Stock>, StocksCommandRepository>();
            
            var connectionString = Configuration["ConnectionStrings:DefaultConnectionString"];
            services.AddDbContext<DatabaseContext>(opt => opt.UseSqlServer(connectionString));
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddNLog();
            env.ConfigureNLog("nlog.config");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
