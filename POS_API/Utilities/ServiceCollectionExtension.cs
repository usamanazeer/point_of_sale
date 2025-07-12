using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using POS_API.Utilities.Authentication;
using POS_API.Utilities.ReceiptPrinterUtilities;

namespace POS_API.Utilities
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddUtilityServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(ServiceCollectionExtension));

            
            services.AddSingleton<IAuthenticationUtilities, AuthenticationUtilities>();

            //signalr
            //services.AddSingleton<INotificationHub, NotificationHub>();
            services.AddSingleton<IReceiptPrinterUtility, ReceiptPrinterUtility>();
            return services;
        }
    }
}
