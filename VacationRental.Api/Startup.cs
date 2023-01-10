using System.Reflection;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using VacationRental.Api.Interfaces;
using VacationRental.Api.Services;
using VacationRental.Domain.Aggregates.BookingAggregate;
using VacationRental.Domain.Aggregates.RentalAggregate;
using VacationRental.Infrastructure.Repositories;

namespace VacationRental.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSwaggerGen(opts => opts.SwaggerDoc("v1", new Info { Title = "Vacation rental information", Version = "v1" }));

            ConfigureMapper(services);
            
            services.AddSingleton<IRentalService, RentalService>();
            services.AddSingleton<IBookingService, BookingService>();
            services.AddSingleton<ICalendarService, CalendarService>();
            
            services.AddSingleton<IRentalRepository, RentalInMemoryRepository>();
            services.AddSingleton<IBookingRepository, BookingInMemoryRepository>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(opts => opts.SwaggerEndpoint("/swagger/v1/swagger.json", "VacationRental v1"));
        }

        static void ConfigureMapper(IServiceCollection services)
        {
            var currentAssembly = Assembly.GetExecutingAssembly();
            
            services.AddAutoMapper(currentAssembly);
        }
    }
}
