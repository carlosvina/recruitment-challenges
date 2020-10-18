using Refactoring.FraudDetection.Models;
using System.Collections.Generic;

namespace Refactoring.FraudDetection
{
    public interface IFraudRadar
    {
        IEnumerable<FraudResult> Check(IEnumerable<Order> orders);
    }

}