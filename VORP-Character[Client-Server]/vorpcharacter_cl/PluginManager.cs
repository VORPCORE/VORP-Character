using CitizenFX.Core;
using System;
using System.Threading.Tasks;

namespace VorpCharacter
{
    public class PluginManager : BaseScript
    {
        public static PluginManager Instance { get; private set; }
        public static dynamic CORE;
        public static int MAX_ALLOWED_CHARACTERS = 0;
        public EventHandlerDictionary EventRegistry => EventHandlers;
        public ExportDictionary ExportRegistry => Exports;

        public PluginManager()
        {

            Instance = this;
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

            CORE.RpcCall("vorp_characters:getMaxCharacters", new Action<int>((max) =>
            {
                MAX_ALLOWED_CHARACTERS = max;
            }), "none");
        }

    }
}