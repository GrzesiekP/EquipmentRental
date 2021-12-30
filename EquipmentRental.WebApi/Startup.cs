using System.Collections.Specialized;
using System.Reflection;
using Core;
using EventStore.Client;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Orders;
using Orders.CommandHandlers;

namespace EquipmentRental.WebApi
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
            var eventStoreConfig = Configuration.GetSection("EventStore").Get<EventStoreConfig>();
            
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EquipmentRental.WebApi", Version = "v1" });
            });
            services.AddMediatR(typeof(SubmitOrderCommandHandler).Assembly);
            services.AddMediatR(typeof(IAggregate).Assembly);

            services.AddSingleton(c =>
            {
                var cs = eventStoreConfig.ConnectionString;
                return new EventStoreClient(
                        EventStoreClientSettings.Create(cs));
            });
            services.AddScoped(typeof(IEventStoreRepository<Order>), typeof(OrderRepository));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EquipmentRental.WebApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}