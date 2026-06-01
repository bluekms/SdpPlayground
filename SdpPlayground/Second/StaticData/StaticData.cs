using Second.StaticData.Tables;
using Microsoft.Extensions.Logging;
using Sdp.Manager;

namespace Second.StaticData;

public sealed partial class StaticData(ILogger logger)
    : StaticDataManager<StaticData.TableSet>(logger)
{
    public sealed partial record TableSet(
        SchoolTable? SchoolTable,
        TeacherTable? TeacherTable,
        StudentTable? StudentTable);

    public SchoolTable School => Current.SchoolTable!;
    public TeacherTable Teacher => Current.TeacherTable!;
    public StudentTable Student => Current.StudentTable!;
}
