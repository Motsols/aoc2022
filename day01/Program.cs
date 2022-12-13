var input = File.ReadAllText("input.txt");
Console.WriteLine(Environment.GetEnvironmentVariable("part") is "part1" ? Solve.part1(input) : Solve.part2(input));

public static class Solve
{
    public static int part1(string input)
    {
        return input.Split("\r\n\r\n").Select(s => s.Split("\r\n").Sum(x => int.Parse(x))).Max();
    }

    public static int part2(string input)
    {
        return input.Split("\r\n\r\n").Select(s => s.Split("\r\n").Sum(x => int.Parse(x))).OrderByDescending(x => x).Take(3).Sum(x => x);
    }
}