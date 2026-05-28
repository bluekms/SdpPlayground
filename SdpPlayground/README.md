# SdpPlayground

> English: [README.en.md](README.en.md)

[StaticDataPipeline(Sdp)](https://github.com/bluekms/StaticDataPipeline) 라이브러리를 학습·검증하기 위한 플레이그라운드.
Excel → CSV → 강타입 record/Table 로딩 흐름을 작은 샘플로 재현한다.

## 요구 사항

- .NET 10 SDK
- [StaticDataPipeline releases](https://github.com/bluekms/StaticDataPipeline/releases)에서 아래 세 파일을 받아 지정 경로에 배치한다.

| 릴리즈 자산 | 배치 경로 |
|---|---|
| `Sdp.dll` | `First/libs/Sdp.dll` |
| `ExcelColumnExtractor-v<ver>-win-x64.exe` | `Tools/ExcelColumnExtractor.exe` |
| `StaticDataHeaderGenerator-v<ver>-win-x64.exe` | `Tools/StaticDataHeaderGenerator.exe` |

> `Tools/*.exe`는 `.gitignore` 대상이므로 클론 직후 별도 배치가 필요하다.

## 시작하기

```bash
dotnet run --project First
```

빌드시 `Tools/ExcelColumnExtractor.exe`가 `First/StaticData/SampleExcels/Excel.xlsx`를 읽어
`First/StaticData/SampleCsvs/Excel.*.csv`를 생성하고, 빌드 출력 디렉터리의 `StaticData/`로 복사된다.

## 사용법

레코드 정의부터 테이블 로딩까지의 단계별 가이드는 [First/USAGE.md](First/USAGE.md)를 참고한다.
