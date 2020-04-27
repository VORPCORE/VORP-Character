using CitizenFX.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vorpcharacter_sv
{
    public class vorpcharacter_sv : BaseScript
    {
        public vorpcharacter_sv()
        {
            EventHandlers["vorp:SaveSkinDB"] += new Action<Player, object, object, string>(SaveSkinDB);
            EventHandlers["vorpcharacter:getPlayerSkin"] += new Action<Player>(getPlayerSkin);
        }

        private void getPlayerSkin([FromSource]Player source)
        {
            string sid = ("steam:" + source.Identifiers["steam"]);

            Exports["ghmattimysql"].execute("SELECT * FROM characters WHERE identifier = ?", new[] { sid }, new Action<dynamic>((result) =>
            {
                if (result.Count == 0)
                {
                    Debug.WriteLine("Character: Usuario no registrado");
                }
                else
                {

                    string s_skin = result[0].skinPlayer;
                    string s_body = result[0].compPlayer;

                    //dynamic skinPlayer = JsonConvert.DeserializeObject(s_skin);
                    Dictionary<string, string> sskin = JsonConvert.DeserializeObject<Dictionary<string, string>>(s_skin);
                    Dictionary<string, string> scloth = JsonConvert.DeserializeObject<Dictionary<string, string>>(s_body);

                    //dynamic componentsPlayer = JsonConvert.DeserializeObject(s_body);



                    //Debug.WriteLine(componentsPlayer.Hat.ToString());
                    //Debug.WriteLine(skinPlayer.sex.ToString());

                    source.TriggerEvent("vorp:loadPlayerSkin", sskin, scloth);
                }

            }));
        }

        private void SaveSkinDB([FromSource]Player source, dynamic skin, dynamic components, string name)
        {
            string sid = ("steam:" + source.Identifiers["steam"]);

            string[] divider = name.Split(' ');

            string firstname = divider[0];
            string lastname = divider[1];
            string skinPlayer = JsonConvert.SerializeObject(skin);
            string componentsPlayer = JsonConvert.SerializeObject(components);

            Exports["ghmattimysql"].execute("INSERT INTO characters (`identifier`, `firstname`, `lastname`, `skinPlayer`, `compPlayer`) VALUES (?, ?, ?, ?, ?)", new object[] { sid, firstname, lastname, skinPlayer, componentsPlayer });



        }
    }
}
