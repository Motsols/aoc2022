var input = File.ReadAllLines("input.txt").ToList();
var alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

Console.WriteLine(Environment.GetEnvironmentVariable("part") is "part1" ? Part1(input) : Part2(input));
Console.ReadKey();

int Part1(IEnumerable<string> input) => input.Select(i =>
{
    var compartment1 = i.Substring(0, i.Length / 2);
    var compartment2 = i.Substring(i.Length / 2, i.Length / 2);
    var letter = compartment1.Where(s => compartment2.Contains(s)).Select(x => x).First();
    return alphabet.IndexOf(letter) +1;
}).Sum();

int Part2(IEnumerable<string> input)
{
    int skip = 0, sum = 0;
    do
    {
        var group = input.Skip(skip).Take(3).ToList();
        var badge = group[0].Where(s => group[1].Contains(s)).Where(s => group[2].Contains(s)).Select(x => x).First();


        sum += alphabet.IndexOf(badge) + 1;
        skip += 3;
    } while (skip < input.Count() - 1);

    return sum;
}