using System;

namespace InstagramAPI.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CommentAt { get; set; }
        public Media Media { get; set; }
        public int MediaId { get; set; }
        public AppUser AppUser { get; set; }
        public string AppUserId { get; set; }
        public int Likes { get; set; }
    }
}
