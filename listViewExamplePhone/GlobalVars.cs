using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace GroupLocator
{
    public static class GlobalVars
    {
        public static User currentUser;
        public static string groupId; //group being tracked.

        public static IMobileServiceTable<User> userTable = App.MobileService.GetTable<User>();
        public static IMobileServiceTable<Group> groupTable = App.MobileService.GetTable<Group>();
        public static IMobileServiceTable<Invite> inviteTable = App.MobileService.GetTable<Invite>();
        public static IMobileServiceTable<Membership> membershipTable = App.MobileService.GetTable<Membership>();

        public static void initialize(){
            currentUser = new User();
        }

    }
}
