using System.ComponentModel;

namespace ElectionWeb.Models
{
    public class DataUpload
    {
        [DisplayName("Document Type")]
        public string FileType { get; set; }
        public IFormFile File { get; set; }
    }
}
