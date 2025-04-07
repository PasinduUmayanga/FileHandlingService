using FHS.Application.Abstractions;
using FHS.Application.DTOs;

namespace FHS.Application.Services
{
    public class FileServices(IExcelReaderService excelReaderService) : IFileServices
    {
      private readonly  IExcelReaderService _ExcelReaderService = excelReaderService;

        /// <summary>
        /// This method is used to get the values from the excel file and return them as a list of InspectionSummaryDto objects.
        /// </summary>
        /// <returns></returns>
        public async Task<List<InspectionSummaryDto>> GetValuesFromExcelFile(string filePath)
        {
            try
            {
               return await _ExcelReaderService.ReadExcelFileAsync<InspectionSummaryDto>(filePath);       
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
