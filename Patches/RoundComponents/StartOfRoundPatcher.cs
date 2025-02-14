using WeatherProbe;
using HarmonyLib;
using Unity.Netcode;
using UnityEngine;
using WeatherProbe.Misc;

namespace WeatherProbe.Patches.RoundComponents
{
    [HarmonyPatch(typeof(StartOfRound))]
    internal static class StartOfRoundPatcher
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(StartOfRound.Awake))]
        static void InitLguStore(StartOfRound __instance)
        {
            Plugin.mls.LogDebug("Initiating components...");
            if (__instance.NetworkManager.IsHost || __instance.NetworkManager.IsServer)
            {
                GameObject behaviour = Object.Instantiate(Plugin.networkPrefab);
                behaviour.hideFlags = HideFlags.HideAndDontSave;
                behaviour.GetComponent<NetworkObject>().Spawn();
                Plugin.mls.LogDebug("Spawned behaviour...");
            }
        }
        [HarmonyPatch(nameof(StartOfRound.SetPlanetsWeather))]
        [HarmonyPostfix]
        static void SetPlanetsWeatherPostfix(StartOfRound __instance)
        {
            if (__instance.IsHost || __instance.IsServer) return;
            if (WeatherProbeBehaviour.Instance == null) return;
            WeatherProbeBehaviour.Instance.SyncProbeWeathersServerRpc();
        }
    }
}
