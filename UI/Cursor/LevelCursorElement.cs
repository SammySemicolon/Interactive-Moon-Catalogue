using InteractiveMoonCatalogue.Compat;
using InteractiveMoonCatalogue.Util;
using InteractiveTerminalAPI.UI.Cursor;
using System.Text;
using UnityEngine;

namespace InteractiveMoonCatalogue.Misc.UI.Cursor
{
    internal class LevelCursorElement : CursorElement
    {
        public SelectableLevel Level { get; set; }
        public TerminalNode RouteNode { get; set; }

        public override string GetText(int availableLength)
        {
            StringBuilder sb = new StringBuilder();
            bool isCompany = false;
            string name;
            if (LethalLevelLoaderCompat.Enabled && LethalLevelLoaderCompat.IsLocked(Level))
            {
                name = "[???]";
            }
            else
            {
                name = $"{Level.PlanetName.Substring(Level.PlanetName.IndexOf(' ')+1)}";
                isCompany = name.Contains("Gordion");
                if (isCompany) name += " / The Company";
            }
            Terminal terminal = Tools.GetTerminal();
            int groupCredits = terminal.groupCredits;

            if(!Active(this))
            {
                if (Level == StartOfRound.Instance.currentLevel)
                    sb.Append("<color=#026440>");
                else sb.Append("<color=#66666666>");
            }
            sb.Append(name);
            sb.Append(new string(' ', Mathf.Max(0, 14 - name.Length)));
            string risk = "";
            if (!string.IsNullOrEmpty(Level.riskLevel) && !Level.riskLevel.Contains("Safe"))
            {
                if (Level.riskLevel.Contains("CORRUPTION")) risk = $" (Corrupted)";
                else risk = $" ({Level.riskLevel})";
            }
            if (!string.IsNullOrEmpty(risk))
            {
                sb.Append(risk);
            }
            AppendPriceText(ref sb, groupCredits, ref risk);
            AppendWeatherText(ref sb, ref risk);

            if (!Active(this))
                sb.Append("</color>");
            return sb.ToString();
        }

        void AppendPriceText(ref StringBuilder sb, int credits, ref string previousText)
        {
            int price = RouteNode.itemCost;
            if (price <= 0)
            {
                sb.Append(new string(' ', Mathf.Max(0, 9)));
                return;
            }
            sb.Append(new string(' ', Mathf.Max(0, 9 - previousText.Length)));
            if (price > credits)
            {
                sb.Append("<color=#880000>");
                sb.Append(price);
                sb.Append("$");
                sb.Append("</color>");
            }
            else
            {
                sb.Append(price);
                sb.Append("$");
            }
            previousText = price + "$";
        }

        void AppendWeatherText(ref StringBuilder sb, ref string previousText)
        {
            LevelWeatherType weather = Level.currentWeather;
            if (weather != LevelWeatherType.None)
            {
                sb.Append(new string(' ', Mathf.Max(0, 9 - previousText.Length)));
                sb.Append($"({weather})");
            }
        }
    }
}
