using BepInEx.Configuration;
using CSync.Extensions;
using CSync.Lib;
using System.Runtime.Serialization;
using WeatherProbe.Misc.Util;

namespace WeatherProbe.Misc
{
    [DataContract]
    public class PluginConfig : SyncedConfig2<PluginConfig>
    {
        [field: SyncedEntryField] public SyncedEntry<int> RANDOM_PRICE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> SPECIFIED_PRICE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> RANDOM_ALWAYS_CLEAR { get; set; }
        public PluginConfig(ConfigFile cfg) : base(Metadata.GUID)
        {
            string topSection = "General";
            RANDOM_PRICE = cfg.BindSyncedEntry(topSection, Constants.WEATHER_PROBE_PRICE_KEY, Constants.WEATHER_PROBE_PRICE_DEFAULT, Constants.WEATHER_PROBE_PRICE_DESCRIPTION);
            SPECIFIED_PRICE = cfg.BindSyncedEntry(topSection, Constants.WEATHER_PROBE_PICKED_WEATHER_PRICE_KEY, Constants.WEATHER_PROBE_PICKED_WEATHER_PRICE_DEFAULT, Constants.WEATHER_PROBE_PICKED_WEATHER_PRICE_DESCRIPTION);
            RANDOM_ALWAYS_CLEAR = cfg.BindSyncedEntry(topSection, Constants.WEATHER_PROBE_ALWAYS_CLEAR_KEY, Constants.WEATHER_PROBE_ALWAYS_CLEAR_DEFAULT, Constants.WEATHER_PROBE_ALWAYS_CLEAR_DESCRIPTION);

            ConfigManager.Register(this);
        }
    }
}
