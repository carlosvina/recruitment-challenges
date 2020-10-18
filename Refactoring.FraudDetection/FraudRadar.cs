// <copyright file="FraudRadar.cs" company="Payvision">
// Copyright (c) Payvision. All rights reserved.
// </copyright>
using Refactoring.FraudDetection.Models;
using Refactoring.FraudDetection.Extensions;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Refactoring.FraudDetection
{
    public class FraudRadar : IFraudRadar
    {
        private readonly IAppSettings _config;
        private readonly ILogger<FraudRadar> _logger;

        public FraudRadar(IAppSettings config, ILogger<FraudRadar> logger)
        {
            _config = config;
            _logger = logger;    
        }
        
        public IEnumerable<FraudResult> Check(IEnumerable<Order> orders)
        {
            var orderList = (List<Order>) orders;
            var fraudResults = new List<FraudResult>();
            
            for (int i = 0; i < orderList.Count; i++)
            {
                var current = orderList[i];
                bool isFraudulent = false;

                current.Normalize();
                for (int j = i + 1; j < orderList.Count; j++)
                {
                    isFraudulent = false;

                    orderList[j].Normalize();
                    if (current.DealId == orderList[j].DealId
                        && current.Email == orderList[j].Email
                        && current.CreditCard != orderList[j].CreditCard)
                    {
                        isFraudulent = true;
                    }

                    if (current.DealId == orderList[j].DealId
                        && current.State == orderList[j].State
                        && current.ZipCode == orderList[j].ZipCode
                        && current.Street == orderList[j].Street
                        && current.City == orderList[j].City
                        && current.CreditCard != orderList[j].CreditCard)
                    {
                        isFraudulent = true;
                    }

                    if (isFraudulent)
                    {
                        fraudResults.Add(new FraudResult { IsFraudulent = true, OrderId = orderList[j].OrderId });
                    }
                }
            }

            return fraudResults;
        }
    }
}