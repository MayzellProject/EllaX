﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Cinder.Core.Exceptions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Cinder.Indexer.Infrastructure.Hosting
{
    public class AddressRefresherService : BackgroundService
    {
        private readonly IAddressRefresherRunner _addressRefresherRunner;
        private readonly ILogger<AddressRefresherService> _logger;

        public AddressRefresherService(ILogger<AddressRefresherService> logger, IAddressRefresherRunner addressRefresherRunner)
        {
            _logger = logger;
            _addressRefresherRunner = addressRefresherRunner;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await _addressRefresherRunner.RunAsync(stoppingToken).ConfigureAwait(false);
                }
                catch (LoggedException) { }
                catch (Exception e)
                {
                    _logger.LogError(e, "Address refresher threw a non-logged exception");
                }

                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken).ConfigureAwait(false);
            }
        }
    }
}
