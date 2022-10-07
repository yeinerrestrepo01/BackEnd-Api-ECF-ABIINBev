using ECF.Core.applications.WebApi;
using ECF.Core.Repository.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace ECF.Core.API
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        private IServiceCollection services;
        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            this.services = services;
            services.AddDbContext<DROfficeContext>(options =>
              options.UseSqlServer(this.configuration.GetConnectionString("DROfficeDatabase")));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                { Title = "Api ECF", Version = "v1" });
            });
            services.AddRazorPages();
            services.AddControllers();
            // Add Cors
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            })); 
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            StartUp.RegisterDI<DROfficeContext>(services, configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            };
            app.UseCors("MyPolicy");
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwagger();

            //indica la ruta para generar la configuración de swagger
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api ECF");
            });
        }
    }
}
