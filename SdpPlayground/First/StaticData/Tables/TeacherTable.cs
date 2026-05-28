using System.Collections.Immutable;
using First.StaticData.SampleRecords;
using Sdp.Table;

namespace First.StaticData.Tables;

public sealed class TeacherTable(ImmutableArray<Teacher> records)
    : StaticDataTable<TeacherTable, Teacher>(records);
