using InteractiveMoonCatalogue.Compat;
using InteractiveMoonCatalogue.Misc.UI.Cursor;
using InteractiveMoonCatalogue.Util;
using InteractiveTerminalAPI.UI;
using InteractiveTerminalAPI.UI.Application;
using InteractiveTerminalAPI.UI.Cursor;
using InteractiveTerminalAPI.UI.Screen;
using System.Collections.Generic;
using System.Linq;

namespace InteractiveMoonCatalogue.Misc.UI.Application
{
    internal class MoonCatalogueApplication : PageApplication
    {
        TerminalKeyword routeKeyword;
        public override void Initialization()
        {
            List<SelectableLevel> levelsIndex = StartOfRound.Instance.levels.ToList(); //disgusting
            SelectableLevel[] levels = StartOfRound.Instance.levels;
            levels = levels.Where(x => !x.PlanetName.Contains("Liquidation")).ToArray();
            if (LethalLevelLoaderCompat.Enabled) LethalLevelLoaderCompat.GrabAllAvailableLevels(ref levels);
            (SelectableLevel[][], CursorMenu[], IScreen[]) entries = GetPageEntries(levels);

            SelectableLevel[][] pagesLevels = entries.Item1;
            CursorMenu[] cursorMenus = entries.Item2;
            IScreen[] screens = entries.Item3;

            for (int i = 0; i < pagesLevels.Length; i++)
            {
                SelectableLevel[] levelList = pagesLevels[i];
                CursorElement[] elements = new CursorElement[levelList.Length];
                cursorMenus[i] = CursorMenu.Create(startingCursorIndex: 0, elements: elements);
                CursorMenu cursorMenu = cursorMenus[i];
                ITextElement[] textElements =
                    [
                        TextElement.Create(text: "Select the moon you wish to route to:"),
                        TextElement.Create(text: " "),
                        cursorMenu
                    ];
                screens[i] = BoxedScreen.Create(title: "Moon Catalogue", elements: textElements);

                for (int j = 0; j < levelList.Length; j++)
                {
                    SelectableLevel level = levelList[j];
                    if (level == null) continue;
                    TerminalNode routeNode = GetRouteNode(levelsIndex.IndexOf(level));


                    elements[j] = new LevelCursorElement()
                    {
                        Level = level,
                        RouteNode = routeNode,
                        Active = (x) => CanPurchaseRoute((LevelCursorElement)x),
                        SelectInactive = false,
                        Action = () => PurchaseRoute(routeNode),
                    };
                }
            }

            currentPage = initialPage;
            currentCursorMenu = initialPage.GetCurrentCursorMenu();
            currentScreen = initialPage.GetCurrentScreen();
        }
        TerminalNode GetRouteNode(int levelIndex)
        {
            if (routeKeyword == null) routeKeyword = Tools.GetTerminal().terminalNodes.allKeywords.First(k => k.word == "route");
            for (int i = 0; i < routeKeyword.compatibleNouns.Length; i++)
            {
                TerminalNode routeMoon = routeKeyword.compatibleNouns[i].result;

                CompatibleNoun[] additionalNodes = routeMoon.terminalOptions;
                for (int j = 0; j < additionalNodes.Length; j++)
                {
                    TerminalNode confirmNode = additionalNodes[j].result;
                    if (confirmNode == null) continue;
                    if (confirmNode.buyRerouteToMoon == levelIndex) return confirmNode;
                }
            }
            return null;
        }
        void PurchaseRoute(TerminalNode routeNode)
        {
            if (!StartOfRound.Instance.inShipPhase)
            {
                ErrorMessage("Moon Catalogue", () => PreviousScreen(), "You cannot travel to another moon while landed on one.");
                return;
            }
            if (StartOfRound.Instance.travellingToNewLevel)
            {
                ErrorMessage("Moon Catalogue", () => PreviousScreen(), "You cannot travel to another moon while travelling to one");
                return;
            }
            if (StartOfRound.Instance.isChallengeFile)
            {
                ErrorMessage("Moon Catalogue", () => PreviousScreen(), "You cannot travel to another moon while doing a challenge moon.");
                return;
            }

            if (StartOfRound.Instance.levels[routeNode.buyRerouteToMoon] == StartOfRound.Instance.currentLevel)
            {
                ErrorMessage("Moon Catalogue", () => PreviousScreen(), "You have already arrived to this moon!");
                return;
            }
            Terminal terminal = Tools.GetTerminal();
            int groupCredits = terminal.groupCredits;
            if (groupCredits < routeNode.itemCost)
            {
                ErrorMessage("Moon Catalogue", () => PreviousScreen(), "Not enough credits to purchase this route");
                return;
            }
            StartOfRound.Instance.ChangeLevelServerRpc(routeNode.buyRerouteToMoon, terminal.groupCredits - routeNode.itemCost);
            UnityEngine.Object.Destroy(InteractiveTerminalManager.Instance);
        }
        bool CanPurchaseRoute(LevelCursorElement element)
        {
            if (!StartOfRound.Instance.inShipPhase || StartOfRound.Instance.travellingToNewLevel || StartOfRound.Instance.isChallengeFile || StartOfRound.Instance.levels[element.RouteNode.buyRerouteToMoon] == StartOfRound.Instance.currentLevel)
            {
                return false;
            }
            Terminal terminal = Tools.GetTerminal();
            int groupCredits = terminal.groupCredits;
            return element.RouteNode.itemCost <= groupCredits;
        }

    }
}
