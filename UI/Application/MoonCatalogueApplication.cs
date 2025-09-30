using InteractiveMoonCatalogue.Compat;
using InteractiveMoonCatalogue.UI.Cursor;
using InteractiveTerminalAPI.UI;
using InteractiveTerminalAPI.UI.Application;
using InteractiveTerminalAPI.UI.Cursor;
using InteractiveTerminalAPI.UI.Screen;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;

namespace InteractiveMoonCatalogue.UI.Application
{
    internal class MoonCatalogueApplication : PageApplication
    {
        TerminalKeyword routeKeyword;
        protected override int GetEntriesPerPage<T>(T[] entries)
        {
            return 12;
        }
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
                cursorMenus[i] = CursorMenu.Create(startingCursorIndex: 0, elements: elements,
                    sorting:
                    [
                        CompareAscendingDifficulty,
                        CompareDescendingDifficulty,
                        CompareAscendingPrice,
                        CompareDescendingPrice,
                        CompareAscendingWeather,
                        CompareDescendingWeather,
                        CompareName
                    ]);
                cursorMenus[i].sortingIndex = -1;
                CursorMenu cursorMenu = cursorMenus[i];
                ITextElement[] textElements =
                    [
                        TextElement.Create(text: "Select the moon you wish to route to:"),
                        TextElement.Create(text: " "),
                        cursorMenu
                    ];
                screens[i] = new BoxedOutputScreen<string, string>()
                {
                    Title = "Moon Catalogue",
                    elements = textElements,
                    Input = GetCurrentSort,
                    Output = (x) => x,
                };

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
                        SelectInactive = true,
                        Action = () => PurchaseRoute(level, routeNode),
                    };
                }
            }

            currentPage = initialPage;
            currentCursorMenu = initialPage.GetCurrentCursorMenu();
            currentScreen = initialPage.GetCurrentScreen();
        }
        int CompareName(CursorElement cursor1, CursorElement cursor2)
        {
            if (cursor1 == null) return 1;
            if (cursor2 == null) return -1;
            LevelCursorElement element = cursor1 as LevelCursorElement;
            LevelCursorElement element2 = cursor2 as LevelCursorElement;
            string name1 = element.Level.PlanetName.Substring(element.Level.PlanetName.IndexOf(' ') + 1);
            string name2 = element2.Level.PlanetName.Substring(element2.Level.PlanetName.IndexOf(' ') + 1);
            return name1.CompareTo(name2);
        }
        int CompareDescendingPrice(CursorElement cursor1, CursorElement cursor2)
        {
            if (cursor1 == null) return 1;
            if (cursor2 == null) return -1;
            LevelCursorElement element = cursor1 as LevelCursorElement;
            LevelCursorElement element2 = cursor2 as LevelCursorElement;
            int price1 = element.RouteNode.itemCost;
            int price2 = element2.RouteNode.itemCost;
            return price1.CompareTo(price2);
        }
        int CompareAscendingPrice(CursorElement cursor1, CursorElement cursor2)
        {
            if (cursor1 == null) return 1;
            if (cursor2 == null) return -1;
            LevelCursorElement element = cursor1 as LevelCursorElement;
            LevelCursorElement element2 = cursor2 as LevelCursorElement;
            int price1 = element.RouteNode.itemCost;
            int price2 = element2.RouteNode.itemCost;
            return price2.CompareTo(price1);
        }
        int CompareDescendingDifficulty(CursorElement cursor1, CursorElement cursor2)
        {
            if (cursor1 == null) return 1;
            if (cursor2 == null) return -1;
            LevelCursorElement element = cursor1 as LevelCursorElement;
            LevelCursorElement element2 = cursor2 as LevelCursorElement;
            if (LethalLevelLoaderCompat.Enabled)
            {
                return LethalLevelLoaderCompat.CompareDescendingDifficulty(element.Level, element2.Level);
            }

            string riskLevel1 = element.Level.riskLevel;
            string riskLevel2 = element2.Level.riskLevel;
            return riskLevel2.CompareTo(riskLevel1);

        }
        int CompareAscendingDifficulty(CursorElement cursor1, CursorElement cursor2)
        {
            if (cursor1 == null) return 1;
            if (cursor2 == null) return -1;
            LevelCursorElement element = cursor1 as LevelCursorElement;
            LevelCursorElement element2 = cursor2 as LevelCursorElement;
            if (LethalLevelLoaderCompat.Enabled)
            {
                return LethalLevelLoaderCompat.CompareAscendingDifficulty(element.Level, element2.Level);
            }
            string riskLevel1 = element.Level.riskLevel;
            string riskLevel2 = element2.Level.riskLevel;
            return riskLevel1.CompareTo(riskLevel2);
        }
        int CompareDescendingWeather(CursorElement cursor1, CursorElement cursor2)
        {
            if (cursor1 == null) return 1;
            if (cursor2 == null) return -1;
            LevelCursorElement element = cursor1 as LevelCursorElement;
            LevelCursorElement element2 = cursor2 as LevelCursorElement;
            LevelWeatherType weather1 = element.Level.overrideWeather ? element.Level.overrideWeatherType : element.Level.currentWeather;
            LevelWeatherType weather2 = element2.Level.overrideWeather ? element2.Level.overrideWeatherType : element2.Level.currentWeather;
            return weather1.CompareTo(weather2);
        }
        int CompareAscendingWeather(CursorElement cursor1, CursorElement cursor2)
        {
            if (cursor1 == null) return 1;
            if (cursor2 == null) return -1;
            LevelCursorElement element = cursor1 as LevelCursorElement;
            LevelCursorElement element2 = cursor2 as LevelCursorElement;
            LevelWeatherType weather1 = element.Level.overrideWeather ? element.Level.overrideWeatherType : element.Level.currentWeather;
            LevelWeatherType weather2 = element2.Level.overrideWeather ? element2.Level.overrideWeatherType : element2.Level.currentWeather;
            return weather2.CompareTo(weather1);
        }

        string GetCurrentSort()
        {
            int currentSort = currentCursorMenu.sortingIndex;
            return currentSort switch
            {
                0 => $"Sort: Difficulty (Ascending) [{InteractiveTerminalAPI.Compat.InputUtils_Compat.ChangeApplicationSortingKey.GetBindingDisplayString()}]",
                1 => $"Sort: Difficulty (Descending) [{InteractiveTerminalAPI.Compat.InputUtils_Compat.ChangeApplicationSortingKey.GetBindingDisplayString()}]",
                2 => $"Sort: Price (Ascending) [{InteractiveTerminalAPI.Compat.InputUtils_Compat.ChangeApplicationSortingKey.GetBindingDisplayString()}]",
                3 => $"Sort: Price (Descending) [{InteractiveTerminalAPI.Compat.InputUtils_Compat.ChangeApplicationSortingKey.GetBindingDisplayString()}]",
                4 => $"Sort: Weather (Ascending) [{InteractiveTerminalAPI.Compat.InputUtils_Compat.ChangeApplicationSortingKey.GetBindingDisplayString()}]",
                5 => $"Sort: Weather (Descending) [{InteractiveTerminalAPI.Compat.InputUtils_Compat.ChangeApplicationSortingKey.GetBindingDisplayString()}]",
                6 => $"Sort: Alphabetical [{InteractiveTerminalAPI.Compat.InputUtils_Compat.ChangeApplicationSortingKey.GetBindingDisplayString()}]",
                _ => $"Sort: None [{InteractiveTerminalAPI.Compat.InputUtils_Compat.ChangeApplicationSortingKey.GetBindingDisplayString()}]"
            };
        }
        TerminalNode GetRouteNode(int levelIndex)
        {
            if (routeKeyword == null) routeKeyword = terminal.terminalNodes.allKeywords.First(k => k.word == "route");
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
        void PurchaseRoute(SelectableLevel level, TerminalNode routeNode)
        {
            string title = level.PlanetName;
            Action backAction = () => ResetScreen();
            if (!StartOfRound.Instance.inShipPhase)
            {
                ErrorMessage(title, backAction, "You cannot travel to another moon while landed on one!");
                return;
            }
            if (StartOfRound.Instance.travellingToNewLevel)
            {
                ErrorMessage(title, backAction, "You cannot travel to another moon while travelling to one!");
                return;
            }
            if (StartOfRound.Instance.isChallengeFile)
            {
                ErrorMessage(title, backAction, "You cannot travel to another moon while doing a challenge moon!");
                return;
            }

            if (StartOfRound.Instance.levels[routeNode.buyRerouteToMoon] == StartOfRound.Instance.currentLevel)
            {
                ErrorMessage(title, backAction, "You have already arrived to this moon!");
                return;
            }
            if (LethalLevelLoaderCompat.Enabled && LethalLevelLoaderCompat.IsLocked(level))
            {
                ErrorMessage(title, backAction, "You are unable to route to this moon!");
                return;
            }
            int groupCredits = terminal.groupCredits;
            if (groupCredits < routeNode.itemCost)
            {
                ErrorMessage(title, backAction, "Not enough credits to purchase this route!");
                return;
            }
            Confirm(title, $"Do you confirm routing to {level.PlanetName}? You will spend {routeNode.itemCost} Company Credits for the routing.", confirmAction: () => ConfirmRoute(routeNode), declineAction: backAction);
        }
        void ConfirmRoute(TerminalNode routeNode)
        {
            StartOfRound.Instance.ChangeLevelServerRpc(routeNode.buyRerouteToMoon, terminal.groupCredits - routeNode.itemCost);
            ResetScreen();
        }
        bool CanPurchaseRoute(LevelCursorElement element)
        {
            if (!StartOfRound.Instance.inShipPhase || StartOfRound.Instance.travellingToNewLevel || StartOfRound.Instance.isChallengeFile || StartOfRound.Instance.levels[element.RouteNode.buyRerouteToMoon] == StartOfRound.Instance.currentLevel)
            {
                return false;
            }

            if (LethalLevelLoaderCompat.Enabled && LethalLevelLoaderCompat.IsLocked(element.Level))
            {
                return false;
            }
            int groupCredits = terminal.groupCredits;
            return element.RouteNode.itemCost <= groupCredits;
        }

    }
}
