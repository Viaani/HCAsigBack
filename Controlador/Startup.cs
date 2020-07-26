using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Controlador
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";

            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //CORS Activación cualquier origen
            services.AddCors(
                options => options.AddPolicy("AllowCors",
                    builder => {
                        builder
                        .WithOrigins("http://localhost:3000") //AllowSpecificOrigins;  
                        //.WithOrigins("http://localhost:3000", "http://localhost:3000/programas") //AllowMultipleOrigins;  
                        //.AllowAnyOrigin() //AllowAllOrigins;  
                        //.WithMethods("GET") //AllowSpecificMethods;  
                        //.WithMethods("GET", "PUT") //AllowSpecificMethods;  
                        //.WithMethods("GET", "PUT", "POST") //AllowSpecificMethods;  
                        .WithMethods("GET", "PUT", "POST", "DELETE") //AllowSpecificMethods;  
                                                                 //.AllowAnyMethod() //AllowAllMethods;  
                                                                 //.WithHeaders("Accept", "Content-type", "Origin", "X-Custom-Header"); //AllowSpecificHeaders;  
                        .AllowAnyHeader(); //AllowAllHeaders;  
                    })
            );

            //CORS Activación un origen
            //services.AddCors(c =>
            //{
            //    c.AddPolicy("AllowOrigin", options => options.WithOrigins("http://localhost:3000"));
            //});


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            
            //Enable CORS policy "AllowCors"  
            app.UseCors("AllowCors");


            //CORS Activación un origen
            //app.UseCors(options => options.WithOrigins("http://localhost:3000"));


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
