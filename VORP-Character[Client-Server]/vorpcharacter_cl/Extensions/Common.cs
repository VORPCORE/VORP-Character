using CitizenFX.Core;
using VorpCharacter.Model;

namespace VorpCharacter.Extensions
{
    static class Common
    {
        public static dynamic GetCoreUser(this VorpPlayer player) => PluginManager.CORE.getUser(player.Handle);
        public static dynamic GetCoreUserCharacter(this VorpPlayer player) => player.GetCoreUser().getUsedCharacter;
        public static dynamic GetCoreUserCharacters(this VorpPlayer player) => player.GetCoreUser().getUserCharacters;
        public static Vector3 AsVector(this Position position) => new Vector3(position.X, position.Y, position.Z);
        public static string GetTranslation(string langKey)
        {
            if (!PluginManager.Langs.ContainsKey(langKey)) return $"'{langKey}' Translation missing";
            return PluginManager.Langs[langKey];
        }
    }
}
