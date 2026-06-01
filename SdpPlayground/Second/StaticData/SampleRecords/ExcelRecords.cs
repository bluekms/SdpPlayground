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
public sealed partial record School(
    School.SchoolId Id,
    string Name,
    Address Address,
    ContactInfo Contact,
    [Length(3)] ImmutableArray<string> Departments,
    [Length(2)] FrozenSet<int> Grades)
{
    public record struct SchoolId(int Value);
}

[StaticDataRecord("Excel", "Teacher")]
public sealed partial record Teacher(
    Teacher.TeacherId Id,
    string Name,
    [ForeignKey("SchoolTable", "Name")] string SchoolName)
{
    public record struct TeacherId(int Value);
}

[StaticDataRecord("Excel", "Student")]
public sealed partial record Student(
    Student.StudentId Id,
    [ColumnName("Name")] string 이름,
    [ForeignKey("SchoolTable", "Id")] School.SchoolId SchoolId,
    [ForeignKey("TeacherTable", "Id")] Teacher.TeacherId TeacherId)
{
    public record struct StudentId(int Value);
}
