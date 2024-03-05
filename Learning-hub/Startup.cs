using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
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
using Learning_hub.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Learning_hub.Models;
using Learning_hub.Entities;

namespace Learning_hub
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
            services.AddDbContext<LearningHubContext>(option =>
            {
                option.UseSqlServer(Configuration.GetConnectionString("MyDB"));
            });
            services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp", builder =>
                {
                    builder.WithOrigins("http://localhost:3000") // Đổi thành địa chỉ trang web React của bạn
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });


            services.Configure<AppSetting>(Configuration.GetSection("AppSettings"));
            var secretKey = Configuration["AppSettings:SecretKey"];
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        //tự cấp token
                        ValidateIssuer = false,
                        ValidateAudience = false,

                        //ký vào token
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),

                        ClockSkew = TimeSpan.Zero
                    };
                });
            services.Configure<PayPalSettings>(Configuration.GetSection("Paypal"));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Learning_hub", Version = "v1" });
            });
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Learning_hub v1"));
                
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("AllowReactApp");

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}
