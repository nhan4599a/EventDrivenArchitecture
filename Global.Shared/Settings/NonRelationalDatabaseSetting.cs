namespace Global.Shared.Settings
{
    public class NonRelationalDatabaseSetting : RelationalDatabaseSetting
    {
        public string DatabaseName { get; set; } = null!;
    }
}
