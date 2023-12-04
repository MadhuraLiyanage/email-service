using System.Net.Http;

namespace MailNotifications.WS
{
    public class Worker : BackgroundService
    {
        private SendMail _sendMail=new();
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _sendMail.FetchMails();
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(GlobalStaticVaiables.Interval, stoppingToken);
            }
        }
    }
}