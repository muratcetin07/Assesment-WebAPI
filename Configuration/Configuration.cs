using Microsoft.Extensions.Configuration;
using System;

namespace AppConfiguration
{
    public static class Configuration
    {
        public static IConfiguration _configuration;

        public static void Configure(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static string GetConfiguration(string key) => _configuration.GetValue<string>(key);

        public static string GetConfiguration(IConfigurationSection configSection, string key) => configSection.GetValue<string>(key);

        public static IConfigurationSection GetSection(string key) => _configuration.GetSection(key);

        public static string MongoDbConnectionString
        {
            get
            {
                var result = GetConfiguration("MongoDbConnectionString");
                if (!string.IsNullOrEmpty(result)) return result;

                throw new Exception("Appsetting.json doesn't contain MongoDbConnectionString");
            }
        }

        public static string MongoDbName
        {
            get
            {
                
                var result = GetConfiguration("MongoDbName");
                if (!string.IsNullOrEmpty(result)) return result;

                throw new Exception("Appsetting.json doesn't contain MongoDbName");

            }
        }

        public static string Secret
        {
            get
            {
                var result = GetConfiguration("Secret");
                if (!string.IsNullOrEmpty(result)) return result;

                throw new Exception("Appsetting.json doesn't contain secret");

            }
        }
    }
}
