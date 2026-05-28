using System.Collections.Immutable;
using First.StaticData.SampleRecords;
using Sdp.Table;

namespace First.StaticData.Tables;

public sealed class StudentTable(ImmutableArray<Student> records)
    : StaticDataTable<StudentTable, Student>(records);
