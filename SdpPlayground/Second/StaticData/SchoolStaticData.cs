using Second.StaticData.Tables;
using Microsoft.Extensions.Logging;
using Sdp.Manager;

namespace Second.StaticData;

public sealed partial class SchoolStaticData(ILogger logger)
    : StaticDataManager<SchoolStaticData.TableSet>(logger)
{
    public sealed partial record TableSet(
        SchoolTable? SchoolTable,
        TeacherTable? TeacherTable,
        StudentTable? StudentTable);

    public SchoolTable School => Current.SchoolTable!;
    public TeacherTable Teacher => Current.TeacherTable!;
    public StudentTable Student => Current.StudentTable!;
}
