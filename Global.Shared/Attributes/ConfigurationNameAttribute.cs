using System;

namespace Infrastructure.DataAccess.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ConfigurationNameAttribute : Attribute
    {
        public string ConfigurationName { get; } = null!;

        public ConfigurationNameAttribute(string configurationName)
        {
            ConfigurationName = configurationName;
        }
    }
}
