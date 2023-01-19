using Core.Config;
using Core.Domain.Aggregates;
using Core.EventStore;
using EventStore.Client;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Orders.Aggregate;
using Orders.CommandHandlers;
using Orders.Config;
using Orders.EventStore;

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
            var martenConfig = Configuration.GetSection("EventStore").Get<MartenConfig>();
            
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EquipmentRental.WebApi", Version = "v1" });
            });
            services.AddMediatR(typeof(SubmitOrderCommandHandler).Assembly);
            services.AddMediatR(typeof(IAggregate).Assembly);

            services.AddSingleton(c =>
            {
                var cs = martenConfig.ConnectionString;
                return new EventStoreClient(
                        EventStoreClientSettings.Create(cs));
            });
            services.AddMarten(martenConfig, opt =>
            {
                opt.ConfigureProjections();
            });
            services.AddScoped(typeof(IMartenEventStoreRepository<Order>), typeof(OrderRepository));

            services.AddCors(p => p.AddPolicy("equipment-rental-dev", b =>
            b.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()
            ));

            services
                .AddAuthentication(opt =>
                {
                    opt.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie(opt =>
                {
                    opt.LoginPath = "/account/google-login";
                })
                .AddGoogle(opt =>
            {
                opt.ClientId = Configuration["Google:ClientId"];
                opt.ClientSecret = Configuration["Google:ClientSecret"];
            });

            services.AddDbContext<EquipmentRentalDbContext>(o =>
                o.UseSqlServer(Configuration.GetConnectionString("DefaultConnectionString")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "EquipmentRental.WebApi v1");
                });
            }

            app.UseCors("equipment-rental");

            app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}