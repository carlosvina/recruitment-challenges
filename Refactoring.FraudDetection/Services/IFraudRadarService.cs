using Refactoring.FraudDetection.Models;
using System.Collections.Generic;

namespace Refactoring.FraudDetection.Services
{
    public interface IFraudRadarService
    {
        IEnumerable<FraudResult> GetFraudulentOrders(IEnumerable<Order> orders); 
    }
}