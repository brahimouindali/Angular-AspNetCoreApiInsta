using InstagramAPI.Models;

namespace InstagramAPI.ModelLogic
{
    public class MeFollowUser
    {
        public AppUser AppUser { get; set; }
        public bool IFollowedUser { get; set; }
        public bool IsMe { get; set; }
    }
}
