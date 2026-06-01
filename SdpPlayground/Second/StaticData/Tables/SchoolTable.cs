using System.Collections.Immutable;
using Second.StaticData.SampleRecords;
using Sdp.Table;

namespace Second.StaticData.Tables;

public sealed partial class SchoolTable(ImmutableArray<School> records)
    : StaticDataTable<SchoolTable, School>(records);
