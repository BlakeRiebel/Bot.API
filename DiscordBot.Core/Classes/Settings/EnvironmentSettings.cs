namespace DiscordBot.Core.Classes.Settings
{
    public class EnvironmentSettings
    {
        private static readonly string _environment =
            System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ??
            System.Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

        public static string Environment { get { return _environment; } }
        public static bool isProduction { get { return (Environment.ToUpper() == "PRODUCTION"); } }
    }
}