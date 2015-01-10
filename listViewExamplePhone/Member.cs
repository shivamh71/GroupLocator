using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupLocator
{
    public class Member
    {
        public string userName { get; set; }
        public string location { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string lastSeen { get; set; }

        public Member(string userName, string location, double latitude, double longitude, string lastSeen)
        {
            this.userName = userName;
            this.location = location;
            this.latitude = latitude;
            this.longitude = longitude;
            this.lastSeen = lastSeen;
        }
    }
}
