using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupLocator
{
    public class InviteItem
    {
        public string groupName { get; set; }
        public string senderEmailId { get; set; }
        public string groupId { get; set; }
        public InviteItem(string groupId, string gname, string sEmailId)
        {
            this.groupId = groupId;
            this.groupName = gname;
            this.senderEmailId = sEmailId;
        }



    }
}
