using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupLocator
{
    public class User
    {
        public String emailId { get; set; }
        public String userName { get; set; }
        public String password { get; set; }
        public DateTime lastSeen { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }

        public void insertUser()
        {
            //await 
        }
    }
}
