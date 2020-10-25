using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstagramAPI.ModelLogic
{
    public class MediaLogic
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int Like { get; set; }
        public IFormFile File { get; set; }
    }
}
