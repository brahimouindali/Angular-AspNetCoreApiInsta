namespace InstagramAPI.Models
{
    public class UserLikeMedia
    {
        public AppUser AppUser { get; set; }
        public string AppUserId { get; set; }
        public Media Media { get; set; }
        public int MediaId { get; set; }
    }
}
