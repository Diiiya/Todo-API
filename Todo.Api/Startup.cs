using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Todo.Api.Data;
using Todo.Api.Data.EfCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Todo.Api.Interfaces;
using FluentValidation.AspNetCore;
using Todo.Api.Validators;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Todo.Api
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
            // In Memory Data Source
            // services.AddSingleton<IInMemoryUserRepo, InMemoryUserRepo>();

            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            );

            services.AddScoped<IUserRepo, EfCoreUserRepository>();
            services.AddScoped<IToDoRepo, EfCoreToDoRepository>();
            services.AddScoped<ITagRepo, EfCoreTagRepository>();

            services.AddControllers(options =>
            {
                options.SuppressAsyncSuffixInActionNames = false;
            });

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                              builder =>
                              {
                                  builder.WithOrigins("http://localhost:3000")
                                                        .AllowAnyMethod()
                                                        .AllowAnyHeader();
                              });
            });

            services.AddControllers();
            services
                .AddControllers()
                .AddFluentValidation(fv => 
                {
                    fv.RegisterValidatorsFromAssemblyContaining<CreateUserValidator>();
                    fv.RegisterValidatorsFromAssemblyContaining<UpdateUserValidator>();
                    fv.RegisterValidatorsFromAssemblyContaining<LoginUserValidator>();
                });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Todo.Api", Version = "v1" });
            });

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["Jwt:Key"])),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            // returns "healthy" if it can connect to the db and "degraded", if it cannot
            services.AddHealthChecks()    
                .AddUrlGroup(new Uri("https://localhost:5001/users"), name: "base URL", failureStatus: HealthStatus.Degraded).AddSqlServer(Configuration.GetConnectionString("DefaultConnection"),    
                healthQuery: "SELECT 1",    
                failureStatus: HealthStatus.Degraded,    
                name: "SQL Server");  
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });

            // Should only be called if some static data should be added to the db 
            TestData.Initialize(app);
        }
    }
}
