using Infrastructure.DataAccess.Attributes;

namespace Global.Shared.Settings
{
    [ConfigurationName("DatabaseSetting")]
    public class RelationalDatabaseSetting
    {
        public string ConnectionString { get; set; } = null!;
    }
}
