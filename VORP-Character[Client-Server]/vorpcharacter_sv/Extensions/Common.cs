using CitizenFX.Core;

namespace VorpCharacter.Extensions
{
    static class Common
    {
        public static dynamic GetCoreUser(this Player player)
        {
            return PluginManager.CORE.getUser(int.Parse(player.Handle));
        }

        public static dynamic GetCoreUserCharacter(this Player player)
        {
            return player.GetCoreUser().getUsedCharacter;
        }
    }
}
