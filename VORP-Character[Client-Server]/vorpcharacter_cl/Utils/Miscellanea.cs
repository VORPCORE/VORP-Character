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
       /* public static async Task<bool> LoadModel(uint hash)
        {
            if(API.IsModelValid( hash))
            {
                API.RequestModel( hash, true);
                while(!API.HasModelLoaded( hash))
                {
                    await BaseScript.Delay(200);
                }
                return true;
            }
            else
            {
                Debug.WriteLine($"Model {hash} is not valid!");
                return false;
            }
        }*/

        public static async Task<uint> GetHash(string model)
        {
            uint hash = (uint)API.GetHashKey(model);
            if (API.IsModelValid(hash))
            {
                API.RequestModel(hash, true);
                while (!API.HasModelLoaded(hash) || !API.HasCollisionForModelLoaded(hash) )
                {
                    await BaseScript.Delay(200);
                }
                
            }
            else
            {
                API.RequestModel(hash, true);
                Debug.WriteLine($"Model {model} is not valid!");                
            }
            return hash;
        }

        public static void TeleportToCoords(float x, float y, float z, float heading = 0.0f)
        {
            int playerPedId = API.PlayerPedId();
            API.SetEntityCoords(playerPedId, x, y, z, true, true, true, false);
            API.SetEntityHeading(playerPedId, heading);
        }


    }
}
