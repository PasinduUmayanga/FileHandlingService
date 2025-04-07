using FHS.Domain.Attributes;

namespace FHS.Application.DTOs
{
    public class InspectionSummaryDto
    {
        [ExcelColumn("Inspector")]
        public string Inspector { get; set; }

        [ExcelColumn("Location")]
        public string Location { get; set; }

        [ExcelColumn("Inspection Date")]
        public DateTime InspectionDate { get; set; }

        [ExcelColumn("Status")]
        public string Status { get; set; }

        [ExcelColumn("Question Code")]
        public string QuestionCode { get; set; }
    }
}
