using Microsoft.Extensions.Configuration;
using System;

namespace Refactoring.FraudDetection
{
    public class AppSettings : IAppSettings
    {
        public string FileRepositoryPath { get; set; }

        public AppSettings(IConfiguration config)
        {
            FileRepositoryPath = 
                !string.IsNullOrEmpty(config["FileRepositoryPath"]) 
                ? config["FileRepositoryPath"] 
                : throw new Exception("FileRepositoryPath setting is missing in appsettings.json");
        }
    }
}