using Refactoring.FraudDetection.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Refactoring.FraudDetection
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
    }
}