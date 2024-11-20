namespace CarMarketplaceWebApi.Services.Interfaces
{
    public interface IFileManagerService
    {
            bool BeAValidImage(IFormFile file);
            Task<List<string>> UploadFilesAsync(IEnumerable<IFormFile> files);
    }
}
