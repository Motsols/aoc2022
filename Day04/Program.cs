var input = File.ReadAllLines("input.txt").ToList();

Console.WriteLine(Environment.GetEnvironmentVariable("part") is "part1" ? Part1(input) : Part2(input));

int Part1(IEnumerable<string> input) => input.Select(i =>
{
    var groups = i.Split(",").Select(Expand).ToList();
    return groups[0].All(c => groups[1].Contains(c)) ? 1 : groups[1].All(c => groups[0].Contains(c)) ? 1 : 0;
}).Sum();

int Part2(IEnumerable<string> input) => input.Select(i =>
{
    var groups = i.Split(",").Select(Expand).ToList();
    return groups[0].Any(c => groups[1].Contains(c)) ? 1 : 0;
}).Sum();

List<int> Expand(string area)
{
    var range = area.Split("-");
    int start = int.Parse(range[0]), stop = int.Parse(range[1]);
    var areas = new List<int>();
    for (var i = start; i <= stop; i++)
        areas.Add(i);

    return areas;
}