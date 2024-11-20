using CarMarketplaceWebApi.Services.Interfaces;

namespace CarMarketplaceWebApi.Services.Implementations
{
    public class FileManagerService : IFileManagerService
    {
        private readonly IWebHostEnvironment _environment;

        public FileManagerService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public bool BeAValidImage(IFormFile file)
        {
            return file != null && file.ContentType.Contains("image") && 1024 * 1024 * 5 >= file.Length;
        }

        public async Task<List<string>> UploadFilesAsync(IEnumerable<IFormFile> files)
        {
            if (files == null || !files.Any())
                throw new ArgumentException("The files parameter cannot be null or empty.");

            var urls = new List<string>();

            foreach (var file in files)
            {
                var fileName = await UploadLocalAsync(file);
                urls.Add("https://www.example.com/uploads/" + fileName);
            }

            return urls;
        }


        public async Task<string> UploadLocalAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("The file parameter cannot be null.");

            if (!BeAValidImage(file))
                throw new Exception("The file format is not valid. You have to upload an image file and it should be maximum 5MB.");

            var fileName = Guid.NewGuid().ToString() + "_" +
                           Path.GetFileNameWithoutExtension(file.FileName) +
                           Path.GetExtension(file.FileName);

            var uploadsPath = _environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            if (!Directory.Exists(uploadsPath))
            {
                Directory.CreateDirectory(uploadsPath);
            }

            var filePath = Path.Combine(uploadsPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName;
        }
    }
}
