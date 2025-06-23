
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.EntityFrameworkCore;
using NoviCode.SQLBuilder;

namespace NoviCode.Services
{
    public class ECBCurrencyBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ECBCurrencyBackgroundService> _logger;

        public ECBCurrencyBackgroundService(IServiceProvider serviceProvider, ILogger<ECBCurrencyBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<NoviCodeDbContext.NoviCodeDbContext>();
                    var ecbService = scope.ServiceProvider.GetRequiredService<ICurrencyRateService>();

                    var rates = await ecbService.GetAllCurrencyRates();
                    if (rates.Any())
                    {
                        var sql = MergeSqlBuilder.BuildMergeSql(rates);
                        await dbContext.Database.ExecuteSqlRawAsync(sql);
                        _logger.LogInformation($"Synced {rates.Count} rates.");
                    }
                }
                catch(Exception ex)
                {
                    throw ex;
                }
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
