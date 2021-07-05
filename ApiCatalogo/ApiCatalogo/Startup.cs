using ApiCatalogo.Context;
using ApiCatalogo.Extensions;
using ApiCatalogo.Filter;
using ApiCatalogo.Logging;
using ApiCatalogo.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ApiCatalogo
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
            services.AddScoped<ApiLoggingFilter>();

            // incluir o Unit of Work no ConfigureServices 
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            string mySqlConnection = Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(mySqlConnection, ServerVersion.AutoDetect(mySqlConnection)));


            

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling
                = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

         
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiCatalogo", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            // adiciona o middleware de tratamento de erros
            //app.ConfigureExceptionHandler();


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiCatalogo v1"));
            }

            loggerFactory.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration 
                {
                    LogLevel = LogLevel.Information
                }
                )); ; 




            // adiciona o middleware de roteamento
            app.UseRouting();
             
            // app.UseAuthentication()

            // Add o middleware q executa o endpoint do
            // request atual
            app.UseAuthorization();

            //Adiciona o middleware para executar o endpoint
            // do request atual
            app.UseEndpoints(endpoints =>
            {
                // adiciona os endpoints para os actions
                // dos controladores sem especificar rotas
                endpoints.MapControllers();
            });
        }
    }
}
