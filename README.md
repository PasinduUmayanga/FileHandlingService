# FileHandlingService

[![AppVeyor build](https://ci.appveyor.com/api/projects/status/github/PasinduUmayanga/FileHandlingService?branch=main&svg=true)](https://ci.appveyor.com/project/PasinduUmayanga/FileHandlingService)
[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-Web%20API-512BD4?logo=dotnet)](https://learn.microsoft.com/aspnet/core)
[![Clean Architecture](https://img.shields.io/badge/Architecture-Clean%20Architecture-2E8B57)](#project-structure)
[![License](https://img.shields.io/badge/License-Not%20specified-lightgrey)](#license)

FileHandlingService is a .NET 8 ASP.NET Core Web API for reading structured data from Excel files. The project uses a Clean Architecture style so the API, application logic, domain models, and infrastructure-specific Excel reader are separated into focused projects.

The current API reads inspection summary data from an Excel file path and maps rows into strongly typed DTO objects using custom column attributes.

## Features

- ASP.NET Core Web API endpoint for Excel file reading.
- Excel parsing with `ExcelDataReader` and `ExcelDataReader.DataSet`.
- Attribute-based Excel column mapping.
- Generic Excel reader service for mapping rows into DTO classes.
- Dependency injection split by application and infrastructure layers.
- AppVeyor CI configuration with NuGet package caching.
- Swagger UI enabled for API exploration.

## Project Structure

```text
src/FileHandlingService
|-- FHS.Api              # ASP.NET Core Web API project
|-- FHS.Application      # Application services and abstractions
|-- FHS.Domain           # Domain attributes and DTO definitions
|-- FHS.Infrastructure   # Excel reader implementation
`-- FileHandlingService.sln
```

## Tech Stack

- .NET 8
- ASP.NET Core Web API
- Swagger / Swashbuckle
- ExcelDataReader
- Microsoft dependency injection
- AppVeyor CI

## Getting Started

### Prerequisites

- .NET 8 SDK
- Visual Studio 2022, Visual Studio Code, or another C# editor

### Restore and Build

```bash
dotnet restore src/FileHandlingService/FileHandlingService.sln
dotnet build src/FileHandlingService/FileHandlingService.sln --configuration Release
```

### Run the API

```bash
dotnet run --project src/FileHandlingService/FHS.Api/FHS.Api.csproj
```

After the API starts, open the Swagger endpoint shown in the terminal, usually:

```text
https://localhost:<port>/swagger
```

## API Endpoint

### Read values from an Excel file

```http
GET /api/FileHandling/GetValuesFromExcelFile?filePath=C:\files\inspection-summary.xlsx
```

The endpoint returns a list of inspection summary records mapped from the Excel file.

## Important Code

### API setup

The API registers controllers, Swagger, and each Clean Architecture layer through extension methods.

```csharp
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddDomainServices();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
```

Source: `src/FileHandlingService/FHS.Api/Program.cs`

### Controller endpoint

The controller delegates file processing to the application service instead of reading Excel files directly.

```csharp
[HttpGet]
[Route("GetValuesFromExcelFile")]
public async Task<List<InspectionSummaryDto>> GetValuesFromExcelFile(string filePath)
{
    return await _FileService.GetValuesFromExcelFile(filePath);
}
```

Source: `src/FileHandlingService/FHS.Api/Controllers/FileHandlingController.cs`

### Application service

The application layer coordinates the use case and depends on an abstraction, not the concrete Excel reader.

```csharp
public async Task<List<InspectionSummaryDto>> GetValuesFromExcelFile(string filePath)
{
    return await _ExcelReaderService.ReadExcelFileAsync<InspectionSummaryDto>(filePath);
}
```

Source: `src/FileHandlingService/FHS.Application/Services/FileServices.cs`

### Excel column mapping

DTO properties use `ExcelColumnAttribute` to connect C# property names with Excel header names.

```csharp
public class InspectionSummaryDto
{
    [ExcelColumn("Inspector")]
    public string Inspector { get; set; }

    [ExcelColumn("Inspection Date")]
    public DateTime InspectionDate { get; set; }
}
```

Source: `src/FileHandlingService/FHS.Domain/DTOs/InspectionSummaryDto.cs`

### Generic Excel reader

The infrastructure layer reads the first worksheet, uses the first row as headers, and fills DTO properties through reflection.

```csharp
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
                var value = Convert.ChangeType(
                    cell,
                    Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

                prop.SetValue(instance, value);
            }
        }
    }

    result.Add(instance);
}
```

Source: `src/FileHandlingService/FHS.Infrastructure/Services/FileHandling/Excel/ExcelReaderService.cs`

### Dependency injection

Application and infrastructure services are registered separately.

```csharp
services.AddScoped<IFileServices, FileServices>();
services.AddScoped<IExcelReaderService, ExcelReaderService>();
```

Sources:

- `src/FileHandlingService/FHS.Application/ConfigureServices.cs`
- `src/FileHandlingService/FHS.Infrastructure/ConfigureServices.cs`

## Continuous Integration

The repository includes an `appveyor.yml` file that:

- Uses the Visual Studio 2022 build image.
- Restores the .NET solution.
- Builds the solution in `Release` mode.
- Caches NuGet packages and NuGet v3 HTTP cache entries.

```yaml
cache:
  - '%USERPROFILE%\.nuget\packages -> **\*.csproj'
  - '%LOCALAPPDATA%\NuGet\v3-cache -> **\*.csproj'
```

## Learning Notes

This project is useful for learning:

- How to organize a small .NET API using Clean Architecture.
- How to keep infrastructure code behind application abstractions.
- How to map Excel headers to DTO properties using custom attributes.
- How to register services with dependency injection.
- How to add a basic AppVeyor CI pipeline with caching.

## License

No license file is currently included in this repository.
