using System.Collections.Immutable;
using Second.StaticData.SampleRecords;
using Sdp.Table;

namespace Second.StaticData.Tables;

public sealed partial class TeacherTable(ImmutableArray<Teacher> records)
    : StaticDataTable<TeacherTable, Teacher>(records);
