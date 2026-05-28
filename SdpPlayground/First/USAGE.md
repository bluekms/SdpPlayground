# First — StaticData 구성

> English: [USAGE.en.md](USAGE.en.md)

[![Sdp](https://img.shields.io/badge/Sdp-v0.1.0-3b82f6)](https://github.com/bluekms/StaticDataPipeline)
[![Sdp Docs](https://img.shields.io/badge/Sdp%20Docs-ko-22c55e)](https://github.com/bluekms/StaticDataPipeline/blob/main/Docs/ko/README.md)

[Sdp](https://github.com/bluekms/StaticDataPipeline)의 **record → Table → Manager** 흐름을 학교/선생/학생 샘플로 재현한 코드.
Sdp 자체의 일반 사용법은 [공식 문서](https://github.com/bluekms/StaticDataPipeline/blob/main/Docs/ko/README.md)를 참고한다.
이 문서는 본 프로젝트에서 **어떤 파일에 무엇이 들어있는지**만 정리한다.

## 구성 요소

| 파일 | 역할 |
|---|---|
| [`StaticData/SampleExcels/Excel.xlsx`](StaticData/SampleExcels/Excel.xlsx) | 원본 Excel — School / Teacher / Student 3 시트 |
| [`StaticData/SampleCsvs/`](StaticData/SampleCsvs/) | 빌드시 `ExcelColumnExtractor.exe`가 시트별로 자동 생성하는 CSV |
| [`StaticData/SampleRecords/ExcelRecords.cs`](StaticData/SampleRecords/ExcelRecords.cs) | 시트별 record + 공용 nested record |
| [`StaticData/Tables/*.cs`](StaticData/Tables/) | Table CRTP wrapper 3개 |
| [`StaticData/StaticData.cs`](StaticData/StaticData.cs) | Manager + `TableSet` + 단축 accessor |
| [`Program.cs`](Program.cs) | `LoadAsync` → 콘솔 출력 |

## 1. Record — `ExcelRecords.cs`

샘플에서 의도적으로 시연한 어트리뷰트·패턴:

| 데모 | 사용처 |
|---|---|
| `[StaticDataRecord("Excel", "<시트명>")]` | `School`, `Teacher`, `Student` |
| `[ColumnName("…")]` (한글 → 영문) | `Address.City`/`Address.Detail`, `Student.이름` |
| `[NullString("")]` | `ContactInfo.Phone` (빈 셀 → `null`) |
| `[Length(n)]` + `ImmutableArray` / `FrozenSet` | `School.Departments` (3), `School.Grades` (2) |
| `[ForeignKey("Record", "Field")]` | `Teacher.SchoolName`, `Student.SchoolId`, `Student.TeacherId` |
| Id 타입 브랜딩 (`record struct Id(int Value)`) | `SchoolId`, `TeacherId`, `StudentId` |
| 중첩 record 재사용 | `Address`, `ContactInfo` (`School`에서 사용) |
| 한글 식별자 | `Student.이름` |

## 2. Table — `Tables/*.cs`

세 클래스 모두 동일 형태의 CRTP wrapper. 인덱싱·조회는 베이스 타입이 처리하므로 본문 없음.

```csharp
public sealed class SchoolTable(ImmutableArray<School> records)
    : StaticDataTable<SchoolTable, School>(records);
```

## 3. Manager — `StaticData/StaticData.cs`

`TableSet`의 멤버는 nullable로 선언 (부분 로드 허용). accessor 프로퍼티가 호출부의 null-forgiving을 캡슐화한다.

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

## 4. 진입점 — `Program.cs`

빌드 출력 옆 `StaticData/`의 CSV를 읽고, 각 시트 records를 그대로 콘솔에 출력. `[ForeignKey]` 검증 실패 등은 `AggregateException`으로 묶여 메시지만 찍고 종료.

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

## 빌드 흐름 (`First.csproj`)

1. **`RunExcelColumnExtractor`** (`BeforeTargets="PrepareForBuild"`) — `Tools/ExcelColumnExtractor.exe`가 `SampleExcels/Excel.xlsx` → `SampleCsvs/Excel.*.csv` 생성.
2. **`CopyStaticDataCsvs`** (`AfterTargets="Build"`) — 생성된 CSV를 빌드 출력의 `StaticData/`로 복사.
