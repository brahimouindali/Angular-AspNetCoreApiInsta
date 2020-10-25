using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InstagramAPI.Models
{
    public class UserLikeMedia
    {
        [ForeignKey("AppUserId")]
        public AppUser AppUser { get; set; }
        [Required]
        public string AppUserId { get; set; }
        public Media Media { get; set; }
        public int MediaId { get; set; }
    }
}
