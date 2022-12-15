var input = File.ReadAllText("input.txt");

Console.WriteLine(Environment.GetEnvironmentVariable("part") is "part1" ? Part1() : Part2());

string Part1()
{
    var (containerStacks, instructions) = ExtractContainersAndMovements();

    foreach (var actions in instructions.Select(instruction => instruction.Split(" ")))
    {
        int take = int.Parse(actions[1]), from = int.Parse(actions[3]) - 1, to = int.Parse(actions[5]) - 1;
        for (var i = 0; i < take; i++)
        {
            containerStacks[to].AddRange(containerStacks[from].TakeLast(1));
            containerStacks[from] = containerStacks[from].Take(containerStacks[from].Count - 1).ToList();
        }
    }

    return $"{string.Join("", containerStacks.Select(stack => stack.Last()))}";
}

string Part2()
{
    var (containerStacks, instructions) = ExtractContainersAndMovements();

    foreach (var actions in instructions.Select(instruction => instruction.Split(" ")))
    {
        int take = int.Parse(actions[1]), from = int.Parse(actions[3]) - 1, to = int.Parse(actions[5]) - 1;
        containerStacks[to].AddRange(containerStacks[from].TakeLast(take));
        containerStacks[from] = containerStacks[from].Take(containerStacks[from].Count - take).ToList();
    }

    return $"{string.Join("", containerStacks.Select(stack => stack.Last()))}";
}

(List<List<string>> containerStacks, List<string> instructions) ExtractContainersAndMovements()
{
    var parts = input.Split("\r\n\r\n");
    return (GetContainers(parts[0]), parts[1].Split("\r\n").ToList());
}

List<List<string>> GetContainers(string rawContainers)
{
    List<List<string>> containerStacks = new();
    var containerRows = rawContainers.Replace("[", " ").Replace("]", " ").Split("\r\n").Reverse().ToList();
    var letterPositions = containerRows.First().Where(c => !string.IsNullOrWhiteSpace(c.ToString())).Select(c => containerRows.First().IndexOf(c)).ToList();
    
    containerStacks.AddRange(letterPositions.Select(p => new List<string>()));

    containerRows.TakeLast(containerRows.Count - 1).ToList().ForEach(row =>
    {
        foreach (var pos in letterPositions.Where(pos => !string.IsNullOrWhiteSpace(row[pos].ToString())))
        {
            containerStacks[letterPositions.IndexOf(pos)].Add(row[pos].ToString());
        }
    });

    return containerStacks;
}