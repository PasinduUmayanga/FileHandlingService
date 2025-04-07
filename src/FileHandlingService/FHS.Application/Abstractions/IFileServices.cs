using FHS.Application.DTOs;

namespace FHS.Application.Abstractions
{
    public interface IFileServices
    {
        Task<List<InspectionSummaryDto>> GetValuesFromExcelFile(string filePath);
    }
}
