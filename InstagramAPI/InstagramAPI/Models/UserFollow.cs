using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InstagramAPI.Models
{
    public class UserFollow
    {
        [ForeignKey("AppUserFollowId")]
        public AppUser AppUserFollow { get; set; }
        [Required]
        public string? AppUserFollowId { get; set; }

        [ForeignKey("AppUserFollowedId")]
        public AppUser AppUserFollowed { get; set; }
        [Required]
        public string? AppUserFollowedId { get; set; }
    }
}
