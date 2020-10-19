using Refactoring.FraudDetection.Models;
using System.Collections.Generic;

namespace Refactoring.FraudDetection.Services
{
    public class FraudRadarService : IFraudRadarService
    {
        public IEnumerable<FraudResult> GetFraudulentOrders(IEnumerable<Order> orders)
        {
            var orderList = (List<Order>) orders;
            var fraudResults = new List<FraudResult>();
            
            for (int i = 0; i < orderList.Count; i++)
            {
                var current = orderList[i];
                bool isFraudulent = false;

                for (int j = i + 1; j < orderList.Count; j++)
                {
                    isFraudulent = false;

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