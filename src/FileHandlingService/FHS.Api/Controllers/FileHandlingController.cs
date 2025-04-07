using FHS.Application.Abstractions;
using FHS.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace FHS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileHandlingController(IFileServices FileService) : ControllerBase
    {
        private readonly IFileServices _FileService = FileService;

        [HttpGet]
        [Route("GetValuesFromExcelFile")]
        public async Task<List<InspectionSummaryDto>> GetValuesFromExcelFile(string filePath)
        {
            try
            {
                return await _FileService.GetValuesFromExcelFile(filePath);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
