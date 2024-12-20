namespace Employee.Util;

public static class ScvEmployeeReader
{
    public static List<Contracts.Employee> Read(string filename)
    {
        var employees = new List<Contracts.Employee>();
        var reader = new StreamReader(filename);
        while (reader.ReadLine() is { } line)
        {
            var values = line.Split(';');
            if (values.Length == 2 && int.TryParse(values[0], out var id))
            {
                employees.Add(new Contracts.Employee(id - 1, values[1]));
            }
        }

        return employees;
    }
}