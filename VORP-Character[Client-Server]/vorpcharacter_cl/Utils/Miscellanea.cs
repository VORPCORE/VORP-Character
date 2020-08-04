using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vorpcharacter_cl.Utils
{
    public class Miscellanea
    {
        /*
         * LoadModel | "int hash" is a hash key from model
         * Wait for Model hash Load in cache
         */
        public static async Task<bool> LoadModel(uint hash)
        {
            if(Function.Call<bool>(Hash.IS_MODEL_VALID, hash))
            {
                Function.Call(Hash.REQUEST_MODEL, hash);
                while(!Function.Call<bool>(Hash.HAS_MODEL_LOADED, hash))
                {
                    await BaseScript.Delay(100);
                }
                return true;
            }
            else
            {
                Debug.WriteLine($"Model {hash} is not valid!");
                return false;
            }
        }

        public static void TeleportToCoords(float x, float y, float z, float heading = 0.0f)
        {
            int playerPedId = API.PlayerPedId();
            API.SetEntityCoords(playerPedId, x, y, z, true, true, true, false);
            API.SetEntityHeading(playerPedId, heading);
        }


    }
}
