using VorpCharacter.Model;

namespace VorpCharacter.Extensions
{
    static class Common
    {
        public static dynamic GetCoreUser(this VorpPlayer player) => PluginManager.CORE.getUser(player.Handle);
        public static dynamic GetCoreUserCharacter(this VorpPlayer player) => player.GetCoreUser().getUsedCharacter;
        public static dynamic GetCoreUserCharacters(this VorpPlayer player) => player.GetCoreUser().getUserCharacters;
    }
}
