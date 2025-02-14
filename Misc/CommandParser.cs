using UnityEngine;

namespace WeatherProbe.Misc
{
    internal static class CommandParser
    {
        const string WEATHER_PROBE_HELP_COMMAND = ">PROBE\nSends out a weather probe to the specified moon and changes its current weather.\nIt will cost {0} Company credits for a randomized weather selection or {1} Company Credits when specifying the weather on a given moon.\nIt cannot change to a weather which is not possible to happen on that moon.\n";
        private static TerminalNode DisplayTerminalMessage(string message, bool clearPreviousText = true)
        {
            TerminalNode node = ScriptableObject.CreateInstance<TerminalNode>();
            node.displayText = message;
            node.clearPreviousText = clearPreviousText;
            return node;
        }
        public static void ParseCommands(string fullText, ref Terminal terminal, ref TerminalNode outputNode)
        {
            string[] textArray = fullText.Split();
            string firstWord = textArray[0].ToLower();
            string secondWord = textArray.Length > 1 ? textArray[1].ToLower() : "";
            switch (firstWord)
            {
                case "probe": outputNode = ExecuteProbeCommand(secondWord, ref terminal, ref outputNode); return;
                default: return;
            }
        }
        private static TerminalNode ExecuteProbeCommand(string secondWord, ref Terminal terminal, ref TerminalNode outputNode)
        {
            if (!string.IsNullOrEmpty(secondWord) && secondWord == "help")
                return DisplayTerminalMessage(string.Format(WEATHER_PROBE_HELP_COMMAND, Plugin.Config.RANDOM_PRICE.Value, Plugin.Config.SPECIFIED_PRICE.Value));

            return outputNode;
        }
    }
}
