using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using InteractiveMoonCatalogue.UI.Application;
using InteractiveTerminalAPI.UI;
using WeatherProbe.Misc;
namespace InteractiveMoonCatalogue
{
    [BepInPlugin(Metadata.GUID,Metadata.NAME,Metadata.VERSION)]
    [BepInDependency("WhiteSpike.InteractiveTerminalAPI")]
    public class Plugin : BaseUnityPlugin
    {
        internal static readonly Harmony harmony = new(Metadata.GUID);
        internal static ManualLogSource mls;

        void Awake()
        {
            InteractiveTerminalManager.RegisterApplication<MoonCatalogueApplication>("imoons", caseSensitive: false);
            mls = Logger;
            mls.LogInfo($"{Metadata.NAME} {Metadata.VERSION} has been loaded successfully.");
        }
    }   
}
