using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json.Serialization;
using TeamAlumniNETBackend.Data;

namespace TeamAlumniNETBackend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // KEYCLOAK
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
           .AddJwtBearer(options =>
           {
               options.RequireHttpsMetadata = false;
               options.SaveToken = true;
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   //Access token for postman can be found at http://localhost:8000/#
                   //requires token from keycloak instance - location stored in secret manager
                   IssuerSigningKeyResolver = (token, securityToken, kid, parameters) =>
                   {
                       var client = new HttpClient();
                       var keyuri = builder.Configuration["TokenSecrets:KeyURI"];
                       //Retrieves the keys from keycloak instance to verify token
                       var response = client.GetAsync(keyuri).Result;
                       Debug.WriteLine("\nResponse: " + response);
                       var responseString = response.Content.ReadAsStringAsync().Result;
                       Debug.WriteLine("\nResponseString: " + responseString);
                       var keys = JsonConvert.DeserializeObject<JsonWebKeySet>(responseString);
                       Debug.WriteLine("\nKeys: " + keys.Keys.ToString());
                       return keys.Keys;
                   },

                   ValidIssuers = new List<string>
                   {
                       builder.Configuration["TokenSecrets:IssuerURI"]
                   },

                   //This checks the token for a the 'aud' claim value
                   ValidAudience = "account",
               };
           });

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //builder.Services.AddCors(options =>
            //{
            //    options.AddDefaultPolicy(
            //        builder =>
            //        {
            //            builder.WithOrigins("https://localhost:44377", "http://localhost:3000")
            //                                .AllowAnyHeader()
            //                                .AllowAnyMethod();
            //        });
            //});

            // Add services to the container.

            builder.Services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Alumni Backend API",
                    Description = "ASP.NET Core Web API for Alumni. ",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Richard, Kjetil, Anne and Jacob",
                        Email = string.Empty,
                        Url = new Uri("https://example.com"),
                    },

                    License = new OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = new Uri("https://example.com/license"),
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });


            builder.Services.AddDbContext<AlumniDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), builder =>
                {
                    builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                });
            });
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();




            var app = builder.Build();



            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseRouting();
            app.UseCors(builder =>
            {
                builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin();

            });
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.MapControllers();

            app.Run();
        }
    }
}