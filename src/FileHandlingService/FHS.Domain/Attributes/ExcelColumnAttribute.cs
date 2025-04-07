
namespace FHS.Domain.Attributes
{
    public class ExcelColumnAttribute : Attribute
    {
        public ExcelColumnAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}