using System.Collections.Immutable;
using Second.StaticData.SampleRecords;
using Sdp.Table;

namespace Second.StaticData.Tables;

public sealed partial class StudentTable(ImmutableArray<Student> records)
    : StaticDataTable<StudentTable, Student>(records);
