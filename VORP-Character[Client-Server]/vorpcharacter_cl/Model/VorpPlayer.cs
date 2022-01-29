using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace VorpCharacter.Model
{
    public static class VorpPlayer
    {
        public static int Handle => GetPlayerIndex();
        public static int PedHandle = PlayerPedId();

        public static Vector3 Position
        {
            get => GetEntityCoords(PedHandle, false, false);
            set => SetEntityCoords(PedHandle, value.X, value.Y, value.Z, true, true, true, false);
        }

        public static float Heading
        {
            get => GetEntityHeading(PedHandle);
            set => SetEntityHeading(PedHandle, (float)value);
        }

        // Core methods
        public static dynamic CoreUser => PluginManager.CORE.getUser(Handle);
        public static dynamic CoreUserCharacter => CoreUser().getUsedCharacter;
        public static dynamic CoreUserCharacters => CoreUser().getUserCharacters;
    }
}
