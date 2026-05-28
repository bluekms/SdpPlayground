# CLAUDE.md

## 프로젝트 개요

[StaticDataPipeline(Sdp)](https://github.com/bluekms/StaticDataPipeline) 라이브러리를 학습·검증하기 위한 플레이그라운드. Excel → CSV → 강타입 record/Table 로딩 흐름을 작은 샘플로 재현한다.

## 코딩 컨벤션

### 기본 포맷팅

- 인코딩: UTF-8
- 줄 끝: LF
- 들여쓰기: 스페이스 4칸 (C# 파일)
- 최대 줄 길이: 120자
- 파일 끝에 빈 줄 추가
- 후행 공백 제거

### C# 스타일 규칙

#### var 사용
- **항상 var를 우선 사용**
- 타입이 명확하거나, 내장 타입이거나, 그 외 모든 경우에 var 사용

```csharp
// Good
var user = new User();
var count = 10;
var result = GetResult();

// Avoid
User user = new User();
int count = 10;
```

#### 중괄호 규칙
- **한 줄 코드여도 반드시 중괄호 사용**
- Allman 스타일 (새 줄에 중괄호)

```csharp
// Good
if (condition)
{
    DoSomething();
}

// Bad
if (condition)
    DoSomething();

if (condition) DoSomething();
```

#### Namespace
- **File-scoped namespace 사용**

```csharp
// Good
namespace NK.LobbyWebAPI.Feature.Arena;

public class MyClass { }

// Avoid
namespace NK.LobbyWebAPI.Feature.Arena
{
    public class MyClass { }
}
```

#### Using 문
- System namespace를 먼저 정렬
- namespace 바깥에 위치

```csharp
using System;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using NK.LobbyWebAPI.Authentication.UserSessions;
```

#### Switch 문
- case 내용 들여쓰기
- case가 블록일 때 추가 들여쓰기 없음

```csharp
switch (value)
{
    case 1:
        DoSomething();
        break;
    case 2:
    {
        var x = 1;
        DoOther(x);
        break;
    }
}
```

#### Modifier 순서
```
public, private, protected, internal, required, file, static, extern, new, virtual, abstract, sealed, override, readonly, unsafe, volatile, async
```

### Nullable

- **Nullable reference types 활성화됨**
- null 가능성을 명시적으로 표기

```csharp
// nullable
string? nullableString;

// non-nullable
string nonNullableString;
```

### Record 타입

- 간단한 데이터 클래스는 record 사용
- Primary constructor 문법 활용

```csharp
public sealed record ArenaRanker(long Rank, int Rating, WholeUserData User);
```

### 문서화

- 공개 API에 대한 XML 주석은 필수가 아님 (CS1591 silent)
- 필요한 경우에만 주석 추가


## 빌드

```bash
dotnet build
dotnet test
```

## 용어

- **플레이북** — `claude.*.md` 파일 하나. "이 작업엔 이 플레이북" 식으로 액션 가이드(절차·체크리스트·출력 포맷 등) 묶음을 가리킨다. 로컬(`<프로젝트>\.claude\`)이든 중앙(`D:\.claude\`)이든 둘 다 포함한다.
- **중앙 플레이북** — `D:\.claude\claude.*.md` 파일들. 4개 프로젝트가 공유하는 공용 플레이북. 로컬 프로젝트의 `.claude\` 안에 같은 이름이 있으면 그쪽이 우선이고, 없을 때만 중앙으로 fallback 된다.

## Claude 커스텀 커맨드 (중앙 저장소 `D:\.claude\`)

공용 `claude.*.md` 힌트 파일은 **`D:\.claude\`** 한 곳에서 관리한다. 이 절대 경로는 어떤 터미널(PowerShell / cmd / Git Bash)에서도 동일하게 해석되므로 어디서 호출해도 같은 파일을 가리킨다.

| 커맨드 | 위치 | 설명 |
|--------|------|------|
| `claude.check.md` | `D:\.claude\claude.check.md` | C# 코드 리뷰 체크리스트 |
| `claude.commit.md` | `D:\.claude\claude.commit.md` | 논리적 커밋 분리 가이드 |
| `claude.pr.md [feature]` | `D:\.claude\claude.pr.md` | PR 메시지 작성 |
| `claude.packet.md [feature]` | `D:\.claude\claude.packet.md` | 패킷/Controller/Handler 작업 |
| `claude.schema.md [feature]` | `D:\.claude\claude.schema.md` | Schema/Record/Table 작업 |
| `claude.migration.md` | `.claude\claude.migration.md` (이 프로젝트) | EF Core 마이그레이션 SQL 생성 |

> **로컬 전용**: 로깅 관련(`claude.log.md`, `claude.tlog.md`)과 Python 스크립트(`.claude\*.py`), 그리고 `log/`/`review/` 등 출력 디렉터리는 각 프로젝트의 `.claude\` 에 그대로 둔다. 출력 경로 `.claude/log/yyMMdd.md` 는 **이 프로젝트의** working directory 기준 상대 경로다.

### 호출 순서 (로컬 우선 → 공용 fallback)

`claude.<이름>.md` 파일명을 요청하면 다음 순서로 찾는다:

1. **먼저** 이 프로젝트의 `.claude\claude.<이름>.md` 를 확인 (로컬 override 우선)
2. 없으면 `D:\.claude\claude.<이름>.md` 로 fallback (공용 힌트)

로컬 우선이므로 같은 이름의 파일을 프로젝트 `.claude\`에 두면 그 프로젝트 안에서만 override 된다.

### 실행 컨텍스트 — 중앙 플레이북은 로컬 기반으로 동작

중앙 플레이북을 이 프로젝트에서 호출하면 **이 프로젝트의 컨텍스트를 기반으로 동작한다.** 중앙 플레이북은 절차·체크리스트·출력 포맷만 제공하고, 실제 적용 대상과 규칙은 로컬에서 가져온다.

특히 `claude.commit.md` · `claude.check.md` · `claude.auto.md` 같은 **액션형 플레이북**이 그 대상이다 — 절차는 중앙 한 곳에서 관리하되, 커밋 메시지 스타일·코드 리뷰 규칙·빌드/테스트 명령은 호출 프로젝트의 컨벤션을 따른다.

- **코딩 컨벤션·규칙**: 이 프로젝트의 `CLAUDE.md` 와 솔루션 옵션 파일들
- **검토/수정 대상 파일**: 이 프로젝트 working tree 안의 파일들
- **빌드·테스트 명령**: 이 프로젝트의 `dotnet build` / `dotnet test`
- **출력/기록 경로**: 이 프로젝트의 `.claude\log\…` / `.claude\review\…`
- **용어·예시**: 중앙 플레이북에 등장하는 다른 프로젝트별 이름은 일반적 예시로 읽고, 실제로는 이 프로젝트의 동등 위치/이름으로 치환해 적용

요약: **중앙 플레이북 = 절차·규약. 로컬 = 실행 컨텍스트.** 둘이 어긋나면 로컬을 우선한다.

### 로그 기록 규칙

모든 커맨드는 작업 완료 후 `.claude/log/yyMMdd.md` 에 아래 형식으로 추가한다.

```markdown
## {커맨드} — {작업 요약 한 줄}

### 질문 요약
{사용자 요청 내용을 2~4줄로 요약}

### 답변
{Claude가 제공한 전체 답변을 생략 없이 기록}
```

- 질문 요약과 답변 **모두** 기록한다. 요약본만 남기지 않는다.
- 날짜는 `yyMMdd` 형식 (예: `260408`)
- 같은 날 여러 작업이 있으면 `##` 섹션을 이어서 추가한다.
