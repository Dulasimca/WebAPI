using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
//using System.WebServer;

namespace TNCSCAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            //  GlobalVariable globalVariable = new GlobalVariable(configuration);
        }

        public IConfiguration Configuration { get; }
        public static string MyConnection { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddCors();
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options =>
                {
                    var Resolver = options.SerializerSettings.ContractResolver;
                    if (Resolver != null)
                        (Resolver as DefaultContractResolver).NamingStrategy = null;
                });
            //services.Configure<IISServerOptions>(options =>
            //{
            //    options.AutomaticAuthentication = false;
            //});
            //services.AddJsonFormatters();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            //    app.UseCors(options =>
            //options.AllowAnyOrigin()
            //.AllowAnyMethod()
            //.AllowAnyHeader()
            //options.WithOrigins("http://localhost:4200", "http://180.179.49.72:8083", "https://tncsc-scm.in:80", "https://tncsc-scm.in", "http://tncsc-scm.in", "http://localhost:443", "https://tncsc-scm.in:443", "https://www.tncsc-scm.in")
            //); 
            app.UseCors(options =>
           options.WithOrigins("http://localhost:4200", "http://180.179.49.72:8083", "https://tncsc-scm.in:80", "https://tncsc-scm.in", "http://tncsc-scm.in", "http://localhost:443", "https://tncsc-scm.in:443", "https://www.tncsc-scm.in")
           .AllowAnyMethod()
           .AllowAnyHeader()
           .AllowCredentials()
           );
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
