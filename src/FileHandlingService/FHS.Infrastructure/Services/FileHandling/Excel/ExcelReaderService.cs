using ExcelDataReader;
using FHS.Application.Abstractions;
using FHS.Domain.Attributes;
using System.Data;
using System.Reflection;
using System.Text;

namespace FHS.Infrastructure.Services.FileHandling.Excel
{
    public class ExcelReaderService : IExcelReaderService
    {
        public async Task<List<T>> ReadExcelFileAsync<T>(string filePath) where T : class, new()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            using var stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
            using var reader = ExcelReaderFactory.CreateReader(stream);

            // Create a configuration for the DataSet
            var config = new ExcelDataSetConfiguration
            {
                ConfigureDataTable = _ => new ExcelDataTableConfiguration
                {
                    UseHeaderRow = true
                }
            };

            var dataSet = reader.AsDataSet(config);
            var table = dataSet.Tables[0];

            var result = new List<T>();
            // Get properties of the class that have the ExcelColumn attribute
            var props = typeof(T).GetProperties()
                .Where(p => p.IsDefined(typeof(ExcelColumnAttribute), false))
                .ToList();

            foreach (DataRow row in table.Rows)
            {
                var instance = new T();

                foreach (var prop in props)
                {
                    var attr = prop.GetCustomAttribute<ExcelColumnAttribute>();
                    var colName = attr?.Name;

                    if (table.Columns.Contains(colName))
                    {
                        var cell = row[colName];

                        if (cell != DBNull.Value)
                        {
                            var value = Convert.ChangeType(cell, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                            prop.SetValue(instance, value);
                        }
                    }
                }
                // Add the instance to the result list
                result.Add(instance);
            }

            return await Task.FromResult(result);
        }
    }
}
