// <copyright file="FraudRadar.cs" company="Payvision">
// Copyright (c) Payvision. All rights reserved.
// </copyright>
using Refactoring.FraudDetection.Models;
using Refactoring.FraudDetection.Services;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Refactoring.FraudDetection
{
    public class FraudRadar
    {
        private readonly IAppSettings _config;
        private readonly ILogger<FraudRadar> _logger;

        private readonly IFraudRadarService _fraudRadarService;

        public FraudRadar(IAppSettings config, ILogger<FraudRadar> logger, IFraudRadarService fraudRadarService)
        {
            _config = config;
            _logger = logger;
            _fraudRadarService = fraudRadarService;
        }
        
        public IEnumerable<FraudResult> Check(IEnumerable<Order> orders)
        {
            return _fraudRadarService.GetFraudulentOrders(orders);
        }
    }
}