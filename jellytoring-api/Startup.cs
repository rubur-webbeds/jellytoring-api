using jellytoring_api.Infrastructure;
using jellytoring_api.Infrastructure.Email;
using jellytoring_api.Infrastructure.Users;
using jellytoring_api.Models.Settings;
using jellytoring_api.Service.Email;
using jellytoring_api.Service.Users;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace jellytoring_api
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "jellytoring_api", Version = "v1" });
            });

            services.AddSingleton<IConnectionFactory>(new MySqlConnectionFactory(Configuration.GetConnectionString("DefaultConnection")));
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));

            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IUsersRepository, UsersRepository>();

            services.AddTransient<IEmailService, EmailService>();
            services.AddScoped<IEmailConfirmationService, EmailConfirmationService>();
            services.AddScoped<IEmailConfirmationRepository, EmailConfirmationRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "jellytoring_api v1"));
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
