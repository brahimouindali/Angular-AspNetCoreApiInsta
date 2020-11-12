using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InstagramAPI.Models
{
    public class Media
    {
        public int Id { get; set; }
        public string MediaUrl { get; set; }
        public DateTime PublishedAt { get; set; }
        public string Description { get; set; }

        [ForeignKey("AppUserId")]
        public AppUser AppUser { get; set; }
        [Required]
        public string AppUserId { get; set; }
        public bool IsVideo { get; set; }
    }
}
