using LethalLevelLoader;

namespace InteractiveMoonCatalogue.Compat
{
    internal static class LethalLevelLoaderCompat
    {
        public static bool Enabled =>
            BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(LethalLevelLoader.Plugin.ModGUID);

        internal static int CompareAscendingDifficulty(SelectableLevel level1, SelectableLevel level2)
        {
            PatchedContent.TryGetExtendedContent(level1, out ExtendedLevel extendedLevel1);
            PatchedContent.TryGetExtendedContent(level2, out ExtendedLevel extendedLevel2);
            return extendedLevel2.CalculatedDifficultyRating.CompareTo(extendedLevel1.CalculatedDifficultyRating);
        }

        internal static int CompareDescendingDifficulty(SelectableLevel level1, SelectableLevel level2)
        {
            PatchedContent.TryGetExtendedContent(level1, out ExtendedLevel extendedLevel1);
            PatchedContent.TryGetExtendedContent(level2, out ExtendedLevel extendedLevel2);
            return extendedLevel1.CalculatedDifficultyRating.CompareTo(extendedLevel2.CalculatedDifficultyRating);
        }

        internal static bool IsLocked(SelectableLevel level)
        {
            ExtendedLevel extendedLevel;
            PatchedContent.TryGetExtendedContent(level, out extendedLevel);
            if (extendedLevel == null || extendedLevel.IsRouteLocked) return true;
            return false;
        }
    }
}
