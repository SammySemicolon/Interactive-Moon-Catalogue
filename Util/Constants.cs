namespace WeatherProbe.Misc.Util
{
    internal static class Constants
    {

        #region Weather Display

        internal const string SELECT_WEATHER_FORMAT = "Select the available weathers for {0}:";
        internal const string CURRENT_WEATHER_FORMAT = "Current weather: {0}";

        internal const string CONFIRM_WEATHER_FORMAT = "Do you wish to change {0}'s weather to {1} for {2} credits?";
        internal const string CONFIRM_RANDOM_WEATHER_FORMAT = "Do you wish to randomize {0}'s weather for {1} credits?";
        internal const string CONFIRM_CLEAR_WEATHER_FORMAT = "Do you wish to clear {0}'s weather for {1} credits?";

        internal const string WEATHER_CHANGED_FORMAT = "{0}'s weather has changed to {1}. Thank you for your purchase.";
        internal const string NOT_ENOUGH_CREDITS_PROBE = "You do not have enough credits to purchase a randomized weather probe.";
        internal const string NOT_ENOUGH_CREDITS_SPECIFIED_PROBE = "You do not have enough credits to purchase a specified weather probe.";
        internal const string SAME_WEATHER_FORMAT = "Unable to change {0}'s weather due to already being the selected weather ({1}).";

        internal const string CANCEL_PROMPT = "Abort";
        #endregion

        #region Weather Probe

        internal const string WEATHER_PROBE_PRICE_KEY = $"Price of {WeatherProbeBehaviour.COMMAND_NAME}";
        internal const int WEATHER_PROBE_PRICE_DEFAULT = 300;
        internal const string WEATHER_PROBE_PRICE_DESCRIPTION = "Price of the weather probe when a weather is not selected for the level";

        internal const string WEATHER_PROBE_ALWAYS_CLEAR_KEY = "Always pick clear weather";
        internal const bool WEATHER_PROBE_ALWAYS_CLEAR_DEFAULT = false;
        internal const string WEATHER_PROBE_ALWAYS_CLEAR_DESCRIPTION = "When enabled, randomized weather probe will always clear out the weather present in the selected level";

        internal const string WEATHER_PROBE_PICKED_WEATHER_PRICE_KEY = $"Price of {WeatherProbeBehaviour.COMMAND_NAME} with selected weather";
        internal const int WEATHER_PROBE_PICKED_WEATHER_PRICE_DEFAULT = 500;
        internal const string WEATHER_PROBE_PICKED_WEATHER_PRICE_DESCRIPTION = "This price is used when using the probe command with a weather";

        internal const string MAIN_WEATHER_PROBE_SCREEN_TITLE = "Weather Probe";
        internal const string MAIN_WEATHER_PROBE_TOP_TEXT = "Select a moon you wish to alter the weather of:";
        #endregion
    }
}
