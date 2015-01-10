using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupLocator
{
    public class User
    {
        public string Id { get; set; }

        [JsonProperty(PropertyName = "emailId")]
        public String emailId { get; set; }

        [JsonProperty(PropertyName = "userName")]
        public String userName { get; set; }

        [JsonProperty(PropertyName = "password")]
        public String password { get; set; }

        [JsonProperty(PropertyName = "lastSeen")]
        public DateTime lastSeen { get; set; }

        [JsonProperty(PropertyName = "latitude")]
        public double latitude { get; set; }

        [JsonProperty(PropertyName = "longitude")]
        public double longitude { get; set; }

        public User()
        {
            userName = null;
            password = null;
            emailId = null;
        }
        public async void insertUser()
        {
            await GlobalVars.userTable.InsertAsync(this);
        }

        private async void UpdateUser()
        {
            await GlobalVars.userTable.UpdateAsync(this);
        }

    }
}
