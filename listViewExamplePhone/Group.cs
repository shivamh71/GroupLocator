using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupLocator
{
    public class Group
    {
        public string Id { get; set; }

        [JsonProperty(PropertyName = "groupName")]
        public string groupName { get; set; }
        public Group(string gname)
        {
            this.groupName = gname;
        }


       

        public void AddMember(string name)
        {

        }


    }
}

