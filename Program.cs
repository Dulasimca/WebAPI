using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace TNCSCAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
         //   var host = new WebHostBuilder()
         //.UseKestrel()
         //.UseContentRoot(Directory.GetCurrentDirectory())
         //.UseIISIntegration()
         //.UseStartup<Startup>()
         //.Build();
            CreateWebHostBuilder(args).Build().Run();

        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();


        //.AddJsonOptions(options =>
        //        {
        //    var Resolver = options.SerializerSettings.ContractResolver;
        //    if (Resolver != null)
        //        (Resolver as DefaultContractResolver).NamingStrategy = null;
        //});
    }
}
