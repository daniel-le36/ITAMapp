using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ITAMapp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ITAMapp
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddMvc();
            services.AddEntityFrameworkSqlServer();
            services.AddDbContext<ITAMContext>(options => options.UseSqlServer("DefaultConnection"));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();
            //app.UseMvcWithDefaultRoute();
            app.UseMvc(routes => {
                routes.MapRoute(
                    "default",
                    "{category}/{action}/{id?}",
                    new {controller ="Application",category="Application",action="Index"}
                    //template: "{controller=Application}/{action=Index}/{category=Application}/{id?}"
                    );

            });

        }
    }
}
