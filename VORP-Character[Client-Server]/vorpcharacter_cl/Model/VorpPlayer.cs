using static CitizenFX.Core.Native.API;

namespace VorpCharacter.Model
{
    public class VorpPlayer
    {
        public int Handle => GetPlayerIndex();
    }
}
