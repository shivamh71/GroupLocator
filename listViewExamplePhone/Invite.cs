using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupLocator
{
    public class Invite
    {
        public string Id { get; set; }

        [JsonProperty(PropertyName = "senderEmailId")]
        public string senderEmailId { get; set; }

        [JsonProperty(PropertyName = "groupId")]
        public string groupId { get; set; }

        [JsonProperty(PropertyName = "receiverEmailId")]
        public string receiverEmailId { get; set; }

        public Invite(string senderEmailId, string groupId, string receiverEmailId){
            this.senderEmailId = senderEmailId;
            this.groupId = groupId;
            this.receiverEmailId = receiverEmailId;
        }

    }
}
