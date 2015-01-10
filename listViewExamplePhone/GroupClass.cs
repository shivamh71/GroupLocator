using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupLocator
{
    public class GroupClass
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public List<string> members { get; set; }

        public GroupClass(int GroupId, string gname)
        {
            this.GroupId = GroupId;
            this.GroupName = gname;
            members = new List<string>();
        }

        public void AddMember(string name)
        {
            this.members.Add(name);
        }
    }
}

