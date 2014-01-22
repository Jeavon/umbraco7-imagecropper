namespace ImageCropper.Models
{
    public class ImageCrop
    {
        public string Id { get; set; }
        public string X1 { get; set; }
        public string Y1 { get; set; }
        public string X2 { get; set; }
        public string Y2 { get; set; }
        public string WidthOriginal { get; set; }
        public string HeightOriginal { get; set; }
        public string WidthDisplay { get; set; }
        public string HeightDisplay { get; set; }
        public string ResizeWidth { get; set; }
        public string Compression { get; set; }
        public string ProcessorUrl { get; set; }
    }
}