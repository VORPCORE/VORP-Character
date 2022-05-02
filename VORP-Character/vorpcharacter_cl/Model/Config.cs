using System.Collections.Generic;
using System.Runtime.Serialization;

namespace VORP.Character.Client.Model
{
    [DataContract]
    public class Male
    {
        [DataMember(Name = "Heads")]
        public List<string> Heads;

        [DataMember(Name = "Body")]
        public List<string> Body;

        [DataMember(Name = "Legs")]
        public List<string> Legs;

        [DataMember(Name = "HeadTexture")]
        public string HeadTexture;
    }

    [DataContract]
    public class Female
    {
        [DataMember(Name = "Heads")]
        public List<string> Heads;

        [DataMember(Name = "Body")]
        public List<string> Body;

        [DataMember(Name = "Legs")]
        public List<string> Legs;

        [DataMember(Name = "HeadTexture")]
        public string HeadTexture;
    }

    [DataContract]
    public class Config
    {
        [DataMember(Name = "defaultlang")]
        public string Defaultlang;

        [DataMember(Name = "StartingCoords")]
        public Position StartingCoords;

        [DataMember(Name = "Male")]
        public List<Male> Male;

        [DataMember(Name = "Female")]
        public List<Female> Female;
    }


}
