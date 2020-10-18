// <copyright file="FraudRadarTests.cs" company="Payvision">
// Copyright (c) Payvision. All rights reserved.
// </copyright>

using Refactoring.FraudDetection.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Logging;
using Moq;

namespace Refactoring.FraudDetection.Tests
{
    [TestClass]
    public class FraudRadarTests
    {
        private Mock<IAppSettings> _config;
        private Mock<ILogger<FraudRadar>> _fraudLogger;
        private Mock<ILogger<FilesOrderRepository>> _filesRepoLogger;

        private FilesOrderRepository _orderRepository;

        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            _config = new Mock<IAppSettings>();
            _fraudLogger = new Mock<ILogger<FraudRadar>>();
            _filesRepoLogger = new Mock<ILogger<FilesOrderRepository>>();

            var testFilePath = GetTestFilePath(TestContext.TestName);
            _config.Setup(p => p.FileRepositoryPath).Returns(testFilePath);

            _orderRepository = new FilesOrderRepository(_config.Object, _filesRepoLogger.Object);
        }

        [TestMethod]
        [DeploymentItem("./Files/OneLineFile.txt", "Files")]
        public void CheckFraud_OneLineFile_NoFraudExpected()
        {
            var orders = _orderRepository.GetAllOrdersAsync();

            var result = ExecuteTest(orders.Result);

            result.Should().NotBeNull("The result should not be null.");
            result.Should().HaveCount(0, "The result should not contains fraudulent lines");
        }

        [TestMethod]
        [DeploymentItem("./Files/TwoLines_FraudulentSecond.txt", "Files")]
        public void CheckFraud_TwoLines_SecondLineFraudulent()
        {
            var orders = _orderRepository.GetAllOrdersAsync();

            var result = ExecuteTest(orders.Result);

            result.Should().NotBeNull("The result should not be null.");
            result.Should().HaveCount(1, "The result should contains the number of lines of the file");
            result.First().IsFraudulent.Should().BeTrue("The first line is not fraudulent");
            result.First().OrderId.Should().Be(2, "The first line is not fraudulent");
        }

        [TestMethod]
        [DeploymentItem("./Files/ThreeLines_FraudulentSecond.txt", "Files")]
        public void CheckFraud_ThreeLines_SecondLineFraudulent()
        {
            var orders = _orderRepository.GetAllOrdersAsync();

            var result = ExecuteTest(orders.Result);

            result.Should().NotBeNull("The result should not be null.");
            result.Should().HaveCount(1, "The result should contains the number of lines of the file");
            result.First().IsFraudulent.Should().BeTrue("The first line is not fraudulent");
            result.First().OrderId.Should().Be(2, "The first line is not fraudulent");
        }

        [TestMethod]
        [DeploymentItem("./Files/FourLines_MoreThanOneFraudulent.txt", "Files")]
        public void CheckFraud_FourLines_MoreThanOneFraudulent()
        {
            var orders = _orderRepository.GetAllOrdersAsync();

            var result = ExecuteTest(orders.Result);

            result.Should().NotBeNull("The result should not be null.");
            result.Should().HaveCount(2, "The result should contains the number of lines of the file");
        }

        private List<FraudResult> ExecuteTest(IEnumerable<Order> orders)
        {
            var fraudRadar = new FraudRadar(_config.Object, _fraudLogger.Object);

            return fraudRadar.Check(orders).ToList();
        }

        private string GetTestFilePath(string testName)
        {
            switch (testName)
            {
                case "CheckFraud_OneLineFile_NoFraudExpected":
                    return Path.Combine(Environment.CurrentDirectory, "Files", "OneLineFile.txt");
                case "CheckFraud_TwoLines_SecondLineFraudulent":
                    return Path.Combine(Environment.CurrentDirectory, "Files", "TwoLines_FraudulentSecond.txt");
                case "CheckFraud_ThreeLines_SecondLineFraudulent":
                    return Path.Combine(Environment.CurrentDirectory, "Files", "ThreeLines_FraudulentSecond.txt");
                case "CheckFraud_FourLines_MoreThanOneFraudulent":
                    return Path.Combine(Environment.CurrentDirectory, "Files", "FourLines_MoreThanOneFraudulent.txt");
                default:
                    throw new Exception($"{testName} related file not found.");
            }
        }
    }
}