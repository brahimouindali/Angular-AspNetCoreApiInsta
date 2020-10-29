using InstagramAPI.Models;
using System.Collections.Generic;

namespace InstagramAPI.ModelLogic
{
    public class UserMedia
    {
        public AppUser AppUser { get; set; }
        public IEnumerable<Media> Medias { get; set; }
        public int CountMedias { get; set; }
        public int CountAbonne { get; set; }
        public int CountAbonnement { get; set; }


        public UserMedia()
        {
            Medias = new List<Media>();
        }
    }
}
