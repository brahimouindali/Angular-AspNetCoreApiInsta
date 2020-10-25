using InstagramAPI.Models;
using System.Collections.Generic;

namespace InstagramAPI.ModelLogic
{
    public class MediaManage
    {
        public Media Media { get; set; }
        public int CountComments { get; set; }
        public int CountLikes { get; set; }
        public bool IsLiked { get; set; }
        public IEnumerable<Comment> Comments { get; set; }

        public MediaManage()
        {
            Comments = new List<Comment>();
        }
    }
}
