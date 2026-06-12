using System.Text;
using Second.StaticData.Tables;
using Microsoft.Extensions.Logging.Abstractions;
using Second.StaticData;

namespace Second;

internal class Program
{
    static async Task Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        var staticData = new SchoolStaticData(NullLogger.Instance);

        try
        {
            var csvDir = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "StaticData");

            await staticData.LoadAsync(csvDir);
        }
        catch (AggregateException ex)
        {
            foreach (var inner in ex.InnerExceptions)
            {
                Console.WriteLine(inner.Message);
            }

            return;
        }

        Console.WriteLine("\n=== Excel.School ===");
        foreach (var r in staticData.School.Records)
        {
            Console.WriteLine(
                $"School {{ Id = {r.Id.Value}, Name = {r.Name}, " +
                $"Address = {{ City = {r.Address.City}, Detail = {r.Address.Detail} }}, " +
                $"Contact = {{ Email = {r.Contact.Email}, Phone = {r.Contact.Phone} }}, " +
                $"Departments = [{string.Join(", ", r.Departments)}], " +
                $"Grades = [{string.Join(", ", r.Grades)}] }}");
        }

        Console.WriteLine("\n=== Excel.Teacher ===");
        foreach (var r in staticData.Teacher.Records)
        {
            Console.WriteLine(r);
        }

        Console.WriteLine("\n=== Excel.Student ===");
        foreach (var r in staticData.Student.Records)
        {
            Console.WriteLine(r);
        }
    }
}
