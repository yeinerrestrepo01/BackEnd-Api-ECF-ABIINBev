using Microsoft.AspNetCore;


namespace ECF.Core.API
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1118:Utility classes should not have public constructors", Justification = "Revisado AFC")]
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>()
            .ConfigureKestrel((context, options) =>
            {
                options.AllowSynchronousIO = true;
                options.Limits.MaxRequestHeadersTotalSize = 100 * 1024;
                options.Limits.MaxRequestBodySize = 4120100000;
            });

    }
}
