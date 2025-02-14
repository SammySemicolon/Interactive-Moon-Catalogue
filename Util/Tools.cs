using UnityEngine;

namespace InteractiveMoonCatalogue.Util
{
    internal static class Tools
    {
        static Terminal terminal = null;

        internal static Terminal GetTerminal()
        {
            if (terminal == null) terminal = GameObject.Find("TerminalScript").GetComponent<Terminal>();
            return terminal;
        }
    }
}
