using InstagramAPI.Models;
using System.Collections.Generic;

namespace InstagramAPI.ModelLogic
{
    public class UsersAndUserLoggedIn
    {
        public AppUser AppUser { get; set; }
        public IEnumerable<AppUser> AppUsers { get; set; }

        public UsersAndUserLoggedIn()
        {
            AppUsers = new List<AppUser>();
        }
    }
}
