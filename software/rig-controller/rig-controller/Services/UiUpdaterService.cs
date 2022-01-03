﻿using Microsoft.AspNetCore.SignalR;
using rig_controller.Hubs;

namespace rig_controller.Services
{
    public class UiUpdaterService
    {
        private readonly IHubContext<UiHub> uiHubContext;
        private readonly ILogger<UiUpdaterService> logger;
        private readonly RigStateService rigStateService;

        public UiUpdaterService(IHubContext<UiHub> uiHubContext, ILogger<UiUpdaterService> logger, RigStateService rigStateService)
        {
            this.uiHubContext = uiHubContext;
            this.logger = logger;
            this.rigStateService = rigStateService;

            Task.Run(async () => await SetFrequency());
        }

        public async Task SetFrequency()
        {
            string digits = (rigStateService.RigState.Frequency / 1000000.0).ToString("0000.000");

            logger.LogInformation($"Server setting frequency in UI to {digits}");

            await uiHubContext.Clients.All.SendAsync("SetFrequency", digits[0], digits[1], digits[2], digits[3], digits[5], digits[6], digits[7]);
        }
    }
}
