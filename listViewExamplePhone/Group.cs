using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupLocator
{
    public class Group
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public Group(int GroupId, string gname)
        {
            this.GroupId = GroupId;
            this.GroupName = gname;
        }

        public void AddMember(string name)
        {

        }
    }
}

