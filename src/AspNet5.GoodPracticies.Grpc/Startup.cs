using AspNet5.GoodPracticies.DTO.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using src.AspNet5.GoodPracticies.Grpc.Services;

namespace AspNet5.GoodPracticies.Grpc
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            string mariaDbConnString = "Server=localhost;Port=3306;Database=AspNet5Teste;Uid=root;Pwd=root;";

            services.AddDbContextPool<UsersDBContext>(
                options => options.UseMySql(
                    mariaDbConnString, 
                    ServerVersion.AutoDetect(mariaDbConnString)));
                    
            services.AddGrpc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<UserService>();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with Grpc endpoints must be made through a Grpc client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
        }
    }
}
