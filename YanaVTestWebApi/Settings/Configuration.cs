using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Test.WebApi.Settings
{
    public static class Configuration
    {
        private static readonly string KEY_SECTION = "Settings";
        private static readonly string PATH_SETTINGS = "appsettings.json";
        public static readonly IConfiguration Values;

        static Configuration() => Values = new ConfigurationBuilder()
            .AddJsonFile(PATH_SETTINGS)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("Environment")}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        public static AppSettings GetSettings() => Values.GetModel<AppSettings>(KEY_SECTION);

        private static TModel GetModel<TModel>(this IConfiguration configuration,
            string key) where TModel : class, new()
        {
            var model = new TModel();
            configuration.GetSection(key).Bind(model);
            return model;
        }

    }
    public class AppSettings 
    {
        public AppSettings() 
        {
            MaxExecution = 60000;
            Step = 1000;
        }
        public int MaxExecution { set; get; }
        public int Step { set; get; }
    }
}
