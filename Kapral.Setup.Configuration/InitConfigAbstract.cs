using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kapral.Setup.Configuration
{
    [AppSettings]
    public abstract class InitConfigAbstract
    {
        protected string PathConfig { get; set; }

        protected InitConfigAbstract(string pathConfig)
        {
            PathConfig = pathConfig;
            ReadCondfig();
        }

        protected abstract bool Validate();

        protected void ReadCondfig()
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(PathConfig);
            var config = configFile.AppSettings.Settings;

            var properties = GetType().GetProperties(
                    BindingFlags.GetProperty |
                    BindingFlags.Instance |
                    BindingFlags.Public |
                    BindingFlags.DeclaredOnly
                );

            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttributes(typeof(AppFieldAttribute), true).FirstOrDefault() as AppFieldAttribute;
                if (attribute == null) continue;

                var key = attribute.Name;

                if (config.AllKeys.Contains(key))
                {
                    var value = Convert.ChangeType(config[key].Value, property.PropertyType);
                    property.SetValue(this, value, null);
                }
            }
        }

        public void ValidateAndSave()
        {
            var result = Validate();
            var appSettingCongig = GetType().GetCustomAttributes(typeof(AppSettingsAttribute), true).FirstOrDefault() as AppSettingsAttribute;

            if (!result || appSettingCongig == null)
            {
                throw new Exception("Not all settings are correct.");
            }

            var configFile = ConfigurationManager.OpenExeConfiguration(PathConfig);
            var config = configFile.AppSettings.Settings;

            var properties = GetType().GetProperties(
                    BindingFlags.GetProperty |
                    BindingFlags.Instance |
                    BindingFlags.Public |
                    BindingFlags.DeclaredOnly
                );

            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttributes(typeof(AppFieldAttribute), true).FirstOrDefault() as AppFieldAttribute;
                if (attribute == null) continue;

                var key = attribute.Name;
                var value = property.GetValue(this, null).ToString();

                if (!config.AllKeys.Contains(key))
                {
                    config.Add(key, "");
                }

                config[key].Value = value;
            }

            configFile.Save(ConfigurationSaveMode.Modified);
        }
    }
}
