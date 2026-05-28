# SdpPlayground

> 한국어: [README.md](README.md)

A playground for exploring the [StaticDataPipeline (Sdp)](https://github.com/bluekms/StaticDataPipeline) library.
It reproduces the Excel → CSV → strongly-typed record/Table loading flow with a small sample.

## Requirements

- .NET 10 SDK
- Download the following three files from the [StaticDataPipeline releases](https://github.com/bluekms/StaticDataPipeline/releases) and place them at the listed paths.

| Release asset | Destination |
|---|---|
| `Sdp.dll` | `First/libs/Sdp.dll` |
| `ExcelColumnExtractor-v<ver>-win-x64.exe` | `Tools/ExcelColumnExtractor.exe` |
| `StaticDataHeaderGenerator-v<ver>-win-x64.exe` | `Tools/StaticDataHeaderGenerator.exe` |

> `Tools/*.exe` is gitignored, so these binaries must be placed manually after every clone.

## Getting Started

```bash
dotnet run --project First
```

At build time, `Tools/ExcelColumnExtractor.exe` reads `First/StaticData/SampleExcels/Excel.xlsx` and emits
`First/StaticData/SampleCsvs/Excel.*.csv`, which are then copied into the build output's `StaticData/` folder.

## Usage

See [First/USAGE.en.md](First/USAGE.en.md) for a step-by-step guide on defining records, creating tables, and loading static data.
