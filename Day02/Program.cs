const int LOSS = 0, DRAW = 3, WIN = 6;
const string ROCK = "A", PAPER = "B", SCISSORS = "C", mustLose = "X", mustDraw = "Y", mustWin = "Z";

var input = File.ReadAllLines("input.txt").ToList();

Console.WriteLine(Environment.GetEnvironmentVariable("part") is "part1" ? Part1(input) : Part2(input));

int Part1(IEnumerable<string> input)
{
    var points = input.GroupBy(x => x).Select(x =>
    {
        var points = 0;
        var hands = x.Key.Replace("X", "A").Replace("Y", "B").Replace("Z", "C").Split(" ");

        if (hands[0] == hands[1])
            points = DRAW + ShapePoints(hands[1]);
        else if (hands[0] is ROCK)
            points = hands[1] is PAPER ? WIN + ShapePoints(hands[1]) : LOSS + ShapePoints(hands[1]);
        else if (hands[0] is PAPER)
            points = hands[1] is SCISSORS ? WIN + ShapePoints(hands[1]) : LOSS + ShapePoints(hands[1]);
        else if (hands[0] is SCISSORS)
            points = hands[1] is ROCK ? WIN + ShapePoints(hands[1]) : LOSS + ShapePoints(hands[1]);

        return points * x.Count();
    });

    return points.Sum();
}

int Part2(IEnumerable<string> input)
{
    var points = input.GroupBy(x => x).Select(x =>
    {
        var points = 0;
        var hands = x.Key.Split(" ", StringSplitOptions.RemoveEmptyEntries);

        if (hands[1] is mustLose)
            points = hands[0] is ROCK ? ShapePoints(SCISSORS) : hands[0] is PAPER ? ShapePoints(ROCK) : ShapePoints(PAPER);
        else if (hands[1] is mustDraw)
            points = DRAW + ShapePoints(hands[0]);
        else if (hands[1] is mustWin)
            points = WIN + (hands[0] is ROCK ? ShapePoints(PAPER) : hands[0] is PAPER ? ShapePoints(SCISSORS) : ShapePoints(ROCK));

        return points * x.Count();
    });

    return points.Sum();
}

int ShapePoints(string letter) => letter switch
{
    ROCK => 1,
    PAPER => 2,
    SCISSORS => 3,
    _ => 0
};