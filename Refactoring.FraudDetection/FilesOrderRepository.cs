using Microsoft.Extensions.Logging;
using Refactoring.FraudDetection;
using Refactoring.FraudDetection.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Refactoring.FraudDetection
{
    public class FilesOrderRepository : IOrderRepository
    {
        private readonly IAppSettings _config;
        private readonly ILogger<FilesOrderRepository> _logger;
        private readonly INormalizer<Order> _orderNormalizer;

        public FilesOrderRepository(IAppSettings config, ILogger<FilesOrderRepository> logger, INormalizer<Order> orderNormalizer)
        {
            _config = config;
            _logger = logger;
            _orderNormalizer = orderNormalizer;
        }

        async public Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            string[] lines = {};
            var orders = new List<Order>();

            try 
            {
                lines = await File.ReadAllLinesAsync(_config.FileRepositoryPath);
            }
            catch (Exception e)
            {
                _logger.LogError($"Problem has occured while parsing order file.\n{e.ToString()}");
                throw e;
            }

            int lineCount = 0;
            foreach (var line in lines)
            {
                lineCount++;
                var items = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (items.Length < 8) 
                {
                    _logger.LogWarning($"Order line {lineCount} has missing information, skipping order.");
                    continue;
                }

                var order = new Order();         
                try 
                {
                    order.OrderId = int.Parse(items[0]);
                    order.DealId = int.Parse(items[1]);
                    order.Email = new System.Net.Mail.MailAddress(items[2].ToLower()).Address;
                    order.Street = items[3].ToLower();
                    order.City = items[4].ToLower();
                    order.State = items[5].ToLower();
                    order.ZipCode = items[6];
                    order.CreditCard = items[7];          
                }
                catch (FormatException)
                {
                    _logger.LogWarning($"Order line {lineCount} has an invalid email address, skipping order.");
                    continue;
                }
                catch (Exception)
                {
                    _logger.LogWarning($"Order line {lineCount} has unexpected order or deal ids, skipping order.");
                    continue;
                }
                
                var normalizedOrder = _orderNormalizer.Normalize(order);

                orders.Add(order);
            }

            return orders;
        }
    }
}