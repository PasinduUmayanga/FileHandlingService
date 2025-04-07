namespace FHS.Application.Abstractions
{
    public interface IExcelReaderService
    {
        Task<List<T>> ReadExcelFileAsync<T>(string filePath) where T : class, new();
    }
}
