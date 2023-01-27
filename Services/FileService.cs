namespace ElectionWeb.Services
{
    public class FileService : IFileService
    {
        public async Task<(bool, string)> UploadFile(IFormFile f, string name)
        {
            var file = f;
            var fileName = file.FileName;

            var extension = Path.GetExtension(fileName).ToLower();
            List<string> acceptedFormats = new List<string>() { ".png", ".jpg", ".jpeg" };
            if (!acceptedFormats.Contains(extension))
            {
                var message = string.Format("{0}, not permitted on the method", extension);
                return (false, message);
            }

            var filename = (name + Path.GetFileName(fileName)).Replace(" ", ""); ;
            string uploadpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", filename);
            var stream = new FileStream(uploadpath, FileMode.Create);
            await f.CopyToAsync(stream);
            return (true, filename);
        }
    }

    public interface IFileService
    {
        Task<(bool, string)> UploadFile(IFormFile file, string name);
    }
}
