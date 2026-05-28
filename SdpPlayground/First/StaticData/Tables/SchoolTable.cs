using System.Collections.Immutable;
using First.StaticData.SampleRecords;
using Sdp.Table;

namespace First.StaticData.Tables;

public sealed class SchoolTable(ImmutableArray<School> records)
    : StaticDataTable<SchoolTable, School>(records);
