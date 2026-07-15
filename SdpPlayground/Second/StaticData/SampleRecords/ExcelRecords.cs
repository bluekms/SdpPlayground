using System.Collections.Frozen;
using System.Collections.Immutable;
using Sdp.Attributes;

namespace Second.StaticData.SampleRecords;

public sealed partial record Address(
    [ColumnName("도시")] string City,
    [ColumnName("상세주소")] string Detail);

public sealed partial record ContactInfo(
    string Email,
    [NullString("")] string? Phone);

[StaticDataRecord("Excel", "School")]
public sealed partial record SchoolRecord(
    SchoolRecord.SchoolId Id,
    string Name,
    Address Address,
    ContactInfo Contact,
    [Length(3)] ImmutableArray<string> Departments,
    [Length(2)] FrozenSet<int> Grades)
{
    public record struct SchoolId(int Value);
}

[StaticDataRecord("Excel", "Teacher")]
public sealed partial record TeacherRecord(
    TeacherRecord.TeacherId Id,
    string Name,
    [ForeignKey("SchoolTable", "Name")] string SchoolName)
{
    public record struct TeacherId(int Value);
}

[StaticDataRecord("Excel", "Student")]
public sealed partial record StudentRecord(
    StudentRecord.StudentId Id,
    [ColumnName("Name")] string 이름,
    [ForeignKey("SchoolTable", "Id")] SchoolRecord.SchoolId SchoolId,
    [ForeignKey("TeacherTable", "Id")] TeacherRecord.TeacherId TeacherId)
{
    public record struct StudentId(int Value);
}
