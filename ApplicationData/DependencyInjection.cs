using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace WebApplication.ApplicationData
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationData(
            this IServiceCollection services)
        {
            var serviceList = from t in Assembly.GetExecutingAssembly().GetTypes()
                where t.Namespace != null && t.IsClass && t.Namespace.EndsWith("ApplicationData.Services")
                select t;

            foreach (var service in serviceList)
            {
                var interfaceList = service.GetInterfaces();

                if (interfaceList is not { Length: 1 })
                {
                    continue;
                }

                services.AddTransient(interfaceList[0], service);
            }

            return services;
        }
    }
}
