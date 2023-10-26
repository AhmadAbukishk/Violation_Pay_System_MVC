using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Traffic_Violation.Controllers;
using Traffic_Violation.Models;

namespace Traffic_Violation
{
    public class LicenseNotification : BackgroundService
    {
        private readonly ILogger<LicenseNotification> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public LicenseNotification(ILogger<LicenseNotification> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Service Started2");

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Service Started3");

                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ModelContext>();
                    var licenses = dbContext.ProjectLicenses.ToList();
                    var currentTime = DateTime.Now.TimeOfDay;

                    if (currentTime.Hours ==  18)
                    {
                        var currentDate = DateTime.Now.ToString("dd/MM/yyyy");
                        _logger.LogInformation("Service Started");
                        foreach (var item in licenses)
                        {
                            if (item.Expdate.HasValue && currentDate == item.Expdate.Value.ToString("dd/MM/yyyy"))
                            {
                                var mailKitController = new MailKitControllercs(dbContext);
                                mailKitController.SendLicense(item);
                            }
                        }
                    }
                }

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken); 
            }
        }
    }
}