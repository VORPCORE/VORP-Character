using CitizenFX.Core;
using CitizenFX.Core.Native;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vorpcharacter_cl
{
    public class vorpcharacter_cl : BaseScript
    {
        public static dynamic CORE = null;
        public static int MaxCharacters = 0;
        public vorpcharacter_cl()
        {
            GetCore();
        }

        public async Task GetCore()
        {
            while (CORE == null)
            {
                TriggerEvent("getCore", new Action<dynamic>((dic) =>
                {
                    CORE = dic;
                }));
                await Delay(100);
            }
            Debug.WriteLine(CORE.ToString());
            CORE.RpcCall("vorp_characters:getMaxCharacters", new Action<int>((max) => {
                MaxCharacters = max;
            }), "none");
        }

    }
}