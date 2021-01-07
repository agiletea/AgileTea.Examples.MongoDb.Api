using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgileTea.Examples.MongoDb.Api.Adapters;
using AgileTea.Examples.MongoDb.Api.CsvMapping;
using AgileTea.Examples.MongoDb.Api.Entities;
using AgileTea.Examples.MongoDb.Api.Repositories;
using AgileTea.Persistence.Common.Repository;
using AgileTea.Persistence.Mongo;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace AgileTea.Examples.MongoDb.Api
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AgileTea.Examples.MongoDb.Api", Version = "v1" });
            });

            services.AddScoped<IRepository<ExampleDocument, Guid>, ExampleDocumentRepository>();
            services.AddScoped<IFindingDocumentRepository, FindingDocumentRepository>();
            
            services.AddSingleton<ExampleDocumentAdapter>();
            services.AddSingleton<FindingDocumentAdapter>();
            services.AddSingleton<IDataMappingService, DataMappingService>();
            
            services.AddMongo(options =>
            {
                options.DbConnection = Configuration["mongo:dbConnection"];
                options.DbName = Configuration["mongo:dbName"];
                options.GuidRepresentation = GuidRepresentation.CSharpLegacy;
                options.CanSupportCosmos = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AgileTea.Examples.MongoDb.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
