using CitizenFX.Core.Native;

namespace VorpCharacter
{
    public class Cache
    {
        private static int _playerPedId;
        public static int PlayerPedId
        {
            get
            {
                if (_playerPedId != API.PlayerPedId())
                    _playerPedId = API.PlayerPedId();

                return _playerPedId;
            }
        }
    }
}
