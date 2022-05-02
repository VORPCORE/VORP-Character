namespace VORP.Character.Server.Extensions
{
    static class Common
    {
        public static dynamic GetCoreUser(this Player player)
        {
            if (PluginManager.USE_CORE_EXPORTS)
            {
                return PluginManager.Instance.ExportRegistry["vorp_core"].GetUser(player.Handle);
            }

            return PluginManager.CORE.getUser(int.Parse(player.Handle));
        }
        public static dynamic GetCoreUserCharacter(this Player player) => player.GetCoreUser().getUsedCharacter;
        public static dynamic GetCoreUserCharacters(this Player player) => player.GetCoreUser().getUserCharacters;
        public static string SteamHandle(this Player player) => $"steam:{player.Identifiers["steam"]}";

        public static Player GetPlayer(int source)
        {
            return PluginManager.PlayerList[source];
        }
    }
}
