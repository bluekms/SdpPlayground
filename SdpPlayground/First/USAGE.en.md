# First — StaticData Layout

> 한국어: [USAGE.md](USAGE.md)

[![Sdp](https://img.shields.io/badge/Sdp-v0.1.0-3b82f6)](https://github.com/bluekms/StaticDataPipeline)
[![Sdp Docs](https://img.shields.io/badge/Sdp%20Docs-en-22c55e)](https://github.com/bluekms/StaticDataPipeline/blob/main/Docs/en/README.md)

A reproduction of the [Sdp](https://github.com/bluekms/StaticDataPipeline) **record → Table → Manager** flow with School/Teacher/Student sample data.
For general Sdp usage, see the [official docs](https://github.com/bluekms/StaticDataPipeline/blob/main/Docs/en/README.md).
This document only lists **what lives in which file** in this project.

## Components

| File | Purpose |
|---|---|
| [`StaticData/SampleExcels/Excel.xlsx`](StaticData/SampleExcels/Excel.xlsx) | Source Excel — three sheets: School / Teacher / Student |
| [`StaticData/SampleCsvs/`](StaticData/SampleCsvs/) | Per-sheet CSVs generated at build time by `ExcelColumnExtractor.exe` |
| [`StaticData/SampleRecords/ExcelRecords.cs`](StaticData/SampleRecords/ExcelRecords.cs) | Per-sheet records + shared nested records |
| [`StaticData/Tables/*.cs`](StaticData/Tables/) | Three Table CRTP wrappers |
| [`StaticData/StaticData.cs`](StaticData/StaticData.cs) | Manager + `TableSet` + shortcut accessors |
| [`Program.cs`](Program.cs) | `LoadAsync` → console dump |

## 1. Records — `ExcelRecords.cs`

Attributes and patterns intentionally demonstrated in the sample:

| Demo | Where |
|---|---|
| `[StaticDataRecord("Excel", "<sheet>")]` | `School`, `Teacher`, `Student` |
| `[ColumnName("…")]` (Korean → English) | `Address.City`/`Address.Detail`, `Student.이름` |
| `[NullString("")]` | `ContactInfo.Phone` (empty cell → `null`) |
| `[Length(n)]` + `ImmutableArray` / `FrozenSet` | `School.Departments` (3), `School.Grades` (2) |
| `[ForeignKey("Record", "Field")]` | `Teacher.SchoolName`, `Student.SchoolId`, `Student.TeacherId` |
| Id type branding (`record struct Id(int Value)`) | `SchoolId`, `TeacherId`, `StudentId` |
| Reusable nested records | `Address`, `ContactInfo` (used by `School`) |
| Korean identifier | `Student.이름` |

## 2. Tables — `Tables/*.cs`

All three classes are the same CRTP wrapper with no body — indexing and lookups are handled by the base type.

```csharp
public sealed class SchoolTable(ImmutableArray<School> records)
    : StaticDataTable<SchoolTable, School>(records);
```

## 3. Manager — `StaticData/StaticData.cs`

`TableSet` members are nullable (partial loading is allowed). The accessor properties encapsulate the null-forgiving for callers.

```csharp
public sealed class StaticData(ILogger logger)
    : StaticDataManager<StaticData.TableSet>(logger)
{
    public sealed record TableSet(
        SchoolTable? SchoolTable,
        TeacherTable? TeacherTable,
        StudentTable? StudentTable);

    public SchoolTable School => Current.SchoolTable!;
    public TeacherTable Teacher => Current.TeacherTable!;
    public StudentTable Student => Current.StudentTable!;
}
```

## 4. Entry point — `Program.cs`

Reads CSVs from the `StaticData/` folder next to the build output and dumps each sheet's records to the console. Failures such as `[ForeignKey]` violations are collected into an `AggregateException`, printed, and exited.

```csharp
var staticData = new StaticData(NullLogger.Instance);

try
{
    await staticData.LoadAsync(csvDir);
}
catch (AggregateException ex)
{
    foreach (var inner in ex.InnerExceptions)
    {
        Console.WriteLine(inner.Message);
    }
    return;
}

foreach (var r in staticData.School.Records) { /* ... */ }
```

## Build flow (`First.csproj`)

1. **`RunExcelColumnExtractor`** (`BeforeTargets="PrepareForBuild"`) — `Tools/ExcelColumnExtractor.exe` reads `SampleExcels/Excel.xlsx` and produces `SampleCsvs/Excel.*.csv`.
2. **`CopyStaticDataCsvs`** (`AfterTargets="Build"`) — copies the generated CSVs into the build output's `StaticData/` folder.
