using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using InteractiveMoonCatalogue.Misc.UI.Application;
using InteractiveTerminalAPI.UI;
using WeatherProbe.Misc;
namespace InteractiveMoonCatalogue
{
    [BepInPlugin(Metadata.GUID,Metadata.NAME,Metadata.VERSION)]
    [BepInDependency("com.sigurd.csync")]
    [BepInDependency("WhiteSpike.InteractiveTerminalAPI")]
    public class Plugin : BaseUnityPlugin
    {
        internal static readonly Harmony harmony = new(Metadata.GUID);
        internal static readonly ManualLogSource mls = BepInEx.Logging.Logger.CreateLogSource(Metadata.NAME);

        void Awake()
        {
            InteractiveTerminalManager.RegisterApplication<MoonCatalogueApplication>("moons", caseSensitive: false);

            mls.LogInfo($"{Metadata.NAME} {Metadata.VERSION} has been loaded successfully.");
        }
    }   
}
