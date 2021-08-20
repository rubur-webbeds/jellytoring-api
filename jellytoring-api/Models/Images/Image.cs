using Microsoft.AspNetCore.Http;
using System;

namespace jellytoring_api.Models.Images
{
    public class Image
    {
        public uint Id { get; set; }
        public string Filename { get; set; }
        public DateTime Date { get; set; }
        public bool Confirmed { get; set; }
        public string Location { get; set; }
        public IFormFile File { get; set; }
        public uint UserId { get; set; }
    }
}
