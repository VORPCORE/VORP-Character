using CitizenFX.Core;
using VorpCharacter.Model;

namespace VorpCharacter.Extensions
{
    static class Common
    {
        public static Vector3 AsVector(this Position position) => new Vector3(position.X, position.Y, position.Z);

        public static string GetTranslation(string langKey)
        {
            if (!PluginManager.Langs.ContainsKey(langKey)) return $"'{langKey}' Translation missing";
            return PluginManager.Langs[langKey];
        }
    }
}
