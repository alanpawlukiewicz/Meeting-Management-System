using MeetingManager.Domain.RepositoryContracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Infrastructure.Services
{
    public class RefreshTokenCleanupService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly TimeSpan _runInterval = TimeSpan.FromDays(1);

        public RefreshTokenCleanupService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var tokenRepository = scope.ServiceProvider.GetRequiredService<IRefreshTokenRepository>();

                    await tokenRepository.DeleteExpiredTokensAsync(stoppingToken);
                }

                await Task.Delay(_runInterval, stoppingToken);
            }
        }
    }
}
