using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using WebTimeClock.ApplicationNotification.Configurations;
using WebTimeClock.ApplicationNotification.Services;

namespace WebTimeClock.ApplicationNotification
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationNotification(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            });

            services.AddSingleton(provider => provider.GetRequiredService<IConfiguration>().GetSection(NotificationSettings.SectionName).Get<NotificationSettings>());
            services.AddSingleton(provider => provider.GetRequiredService<IConfiguration>().GetSection(SmtpSettings.SectionName).Get<SmtpSettings>());

            services.AddTransient<EmailService>();

            return services;
        }
    }
}
