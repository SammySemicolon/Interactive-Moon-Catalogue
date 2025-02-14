using InteractiveMoonCatalogue.Util;
using InteractiveTerminalAPI.UI.Cursor;
using System.Text;
using UnityEngine;
using static Unity.Audio.Handle;

namespace InteractiveMoonCatalogue.Misc.UI.Cursor
{
    internal class LevelCursorElement : CursorElement
    {
        public SelectableLevel Level { get; set; }
        public TerminalNode RouteNode { get; set; }

        public override string GetText(int availableLength)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(new string(' ', 2));
            string name = Level.PlanetName + new string(' ', Mathf.Max(0, 20 - Level.PlanetName.Length));
            Terminal terminal = Tools.GetTerminal();
            int groupCredits = terminal.groupCredits;
            if(!Active(this))
            {
                sb.Append("<color=#66666666>");
            }
            sb.Append(name);

            sb.Append(' ');
            AppendPriceText(ref sb, groupCredits);
            sb.Append(' ');
            AppendWeatherText(ref sb);

            if (!Active(this))
                sb.Append("</color>");
            return sb.ToString();
        }

        void AppendPriceText(ref StringBuilder sb, int credits)
        {
            int price = RouteNode.itemCost;
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
        }

        void AppendWeatherText(ref StringBuilder sb)
        {
            LevelWeatherType weather = Level.currentWeather;
            if (weather != LevelWeatherType.None)
                sb.Append($"({weather})");
        }
    }
}
