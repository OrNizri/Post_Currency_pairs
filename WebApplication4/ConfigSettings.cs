using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPiCurrencies
{
    public static class ConfigSettings
    {
        public static string connection { get; }

        static ConfigSettings()
        {
            var configurationBuilder = new ConfigurationBuilder();
            string path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.Development.json");
            configurationBuilder.AddJsonFile(path, false);
            connection = configurationBuilder.Build().GetSection("ConnectionStrings:CurrenciesDatabase").Value;
        }
    }
}
