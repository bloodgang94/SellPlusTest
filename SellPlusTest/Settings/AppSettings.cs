using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace SellPlusTest.Settings
{
    public sealed class AppSettings
    {
        private static readonly Lazy<AppSettings> _lazy =
            new Lazy<AppSettings>(() => new AppSettings());

        private AppSettings()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(typeof(AppSettings).Assembly.Location))
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            SellPlusSettings = builder.Build().GetSection("SellPlusSettings").Get<SellPlusSettings>();
        }

        public static AppSettings Instance => _lazy.Value;

        public SellPlusSettings SellPlusSettings { get; }
    }
}
