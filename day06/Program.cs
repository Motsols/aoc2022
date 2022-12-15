var input = File.ReadAllText("input.txt");

Console.WriteLine(Environment.GetEnvironmentVariable("part") is "part1" ? Part1() : Part2());

int Part1() => FindMarker(4);

int Part2() => FindMarker(14);

int FindMarker(int length)
{
    var sequenceFound = false;
    var start = 0;

    do
    {
        var s = input.Substring(start, length);
        var uniqueLetters = s.Select(c => c).Distinct();
        if (s.Select(c => c).Distinct().Count() == length)
            sequenceFound = true;

        start++;
    } while (!sequenceFound);

    return start + length - 1;
}