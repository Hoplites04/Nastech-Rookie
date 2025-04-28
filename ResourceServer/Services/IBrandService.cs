namespace ResourceServer.Services
{
    public interface IBrandService
    {
        Task<int> CreateBrandAsync(string name);
    }
}