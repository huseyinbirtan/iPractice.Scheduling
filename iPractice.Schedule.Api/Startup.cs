using FluentValidation;
using iPractice.DataAccess;
using iPractice.DataAccess.Repositories;
using iPractice.Scheduling.Api.Application;
using iPractice.Scheduling.Api.Factories;
using iPractice.Scheduling.Api.Middlewares;
using iPractice.Scheduling.Domain.Events;
using iPractice.Scheduling.Domain.ScheduleAggregate;
using iPractice.Scheduling.Domain.SyncAggregates;
using iPractice.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Reflection;
using IValidatorFactory = iPractice.Scheduling.Api.Factories.IValidatorFactory;

namespace iPractice.Scheduling.Api
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
            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = " iPractice APIs"
                });
            });

            var mediatrAssemblies = new Assembly[] { typeof(Startup).Assembly, typeof(AppointmentScheduledEvent).Assembly };

            services.AddMediatR(a => a.RegisterServicesFromAssemblies(mediatrAssemblies));

            services.AddAutoMapper(typeof(Startup).Assembly);

            services.AddValidatorsFromAssemblyContaining<Startup>();

            services.AddScoped<IReadModel, ReadModel>();
            services.AddScoped<IValidatorFactory, ValidatorFactory>();
            services.AddScoped<IScheduleRepository, ScheduleRepository>();
            services.AddScoped<IClientReadRepository, ClientReadRepository>();
            services.AddScoped<IRepository<Client>, Repository<Client>>();
            services.AddScoped<IRepository<Psychologist>, Repository<Psychologist>>();

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(Configuration.GetConnectionString("Sqlite")));

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "OpenUp v1"); });
            }

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
