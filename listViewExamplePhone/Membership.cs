using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroupLocator
{
    public class Membership
    {
        public string Id { get; set; }

        [JsonProperty(PropertyName = "groupId")]
        public string groupId { get; set; }

        [JsonProperty(PropertyName = "emailId")]
        public string emailId { get; set; }

        public Membership(string gId, string eId){
            this.groupId = gId;
            this.emailId = eId;
        }

        

    }
}
