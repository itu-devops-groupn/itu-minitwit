using Serilog;

namespace Minitwit.Web.Logging
{
    public static class LoggingInstaller
    {
        public static IServiceCollection AddSeriLogging(this IServiceCollection services, IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

                return services;
        }
    }
     
}