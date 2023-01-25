namespace ElectionWeb.Models
{
    public class DataUpload
    {
        public string FileType { get; set; }
        public IFormFile File { get; set; }
    }
}
