using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using Task.Api.Core;
using Task.Api.Core.Domain;
using Task.Api.Core.Repositories;
using Task.Api.Options;
using Task.Api.Persistance;

namespace Task.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //Database
            services.AddDbContextPool<TaskDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("TaskDbConnection")));
            //Identity
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.Password.RequiredLength = 4;
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                })
                .AddEntityFrameworkStores<TaskDbContext>();
            //Automapper
            services.AddAutoMapper(typeof(Startup));
            //Dependency
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IWorkItemRepository, WorkItemRepository>();
            //Swagger
            services.AddSwaggerGen(swagger => {
                swagger.SwaggerDoc("v1", new Info { Title = "Task Api", Version = "v1" });
                var security = new Dictionary<string, IEnumerable<string>> {
                    { "Bearer", Enumerable.Empty<string>() }
                };
                swagger.AddSecurityDefinition("Bearer", new ApiKeyScheme {
                    Description = "JWT Authorization header using bearer scheme",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
                swagger.AddSecurityRequirement(security);
            });
            //Mvc
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            //Jwt
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<ApplicationUser>>();
                        var userId = context.Principal.Identity.Name;
                        var user = userManager.FindByIdAsync(userId).Result;
                        if (user == null)
                        {
                            context.Fail("Unauthorized");
                        }
                        if (user != null)
                        {
                            var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.NameIdentifier, user.Id)
                            };
                            var appIdentity = new ClaimsIdentity(claims);
                            context.Principal.AddIdentity(appIdentity);
                        }

                        return System.Threading.Tasks.Task.CompletedTask;
                    }
                };
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                IdentityModelEventSource.ShowPII = true;
            }
            app.UseCors(x => x.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());
            app.UseAuthentication();
            //Swagger
            var swaggerOptions = new Options.SwaggerOptions();
            Configuration.GetSection(nameof(Options.SwaggerOptions)).Bind(swaggerOptions);
            app.UseSwagger(options => options.RouteTemplate = swaggerOptions.JsonRoute);
            app.UseSwaggerUI(options => options.SwaggerEndpoint(swaggerOptions.UiEndpoint, swaggerOptions.Description));
            //Mvc
            app.UseMvc();
        }
    }
}
