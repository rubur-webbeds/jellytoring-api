namespace jellytoring_api.Models.Images
{
    public class ImageResolution
    {
        public uint Id { get; set; }
        public Status Status { get; set; }
        public string Reason { get; set; }
        public uint UserId { get; set; }
    }
}
