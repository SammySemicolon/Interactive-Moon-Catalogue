using HarmonyLib;
using WeatherProbe.Misc;

namespace WeatherProbe.Patches
{
    [HarmonyPatch(typeof(StartMatchLever))]
    internal static class StartMatchLeverPatcher
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(StartMatchLever.StartGame))]
        static void SyncHelmets()
        {
            WeatherProbeBehaviour.probedWeathers.Clear();
        }
    }
}