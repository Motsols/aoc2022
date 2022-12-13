var input = File.ReadAllText("input.txt");
Console.WriteLine(Environment.GetEnvironmentVariable("part") is "part1" ? Part1(input) : Part2(input));
Console.ReadKey();

int Part1(string input) => input.Split("\r\n\r\n").Select(s => s.Split("\r\n").Sum(int.Parse)).Max();

int Part2(string input) => input.Split("\r\n\r\n").Select(s => s.Split("\r\n").Sum(int.Parse)).OrderByDescending(x => x).Take(3).Sum();