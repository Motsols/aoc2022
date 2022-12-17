var input = File.ReadAllLines("input.txt");
int lowestX = int.MaxValue, highestX = 0, highestY = 0, sandUnits = 0, fallheight = 0, minX = 0, maxX = int.MaxValue, maxY = 0;
bool outOfBounds = false;
string[,] caveStructure;

//BuildCaveStructure();

Console.WriteLine(Environment.GetEnvironmentVariable("part") is "part1" ? Part1() : Part1());
Console.ReadKey();

int Part1()
{
    BuildCaveStructure(false);
    maxX = highestX; maxY = highestY;
    do
    {
        bool stable = false;
        int lastPositionY = 0, lastPositionX = 500 - lowestX;
        do
        {
            if (CanMoveDown(lastPositionY, lastPositionX))
            {
                lastPositionY += 1;
            }
            else if (CanMoveDownLeft(lastPositionY, lastPositionX))
            {
                lastPositionX += -1;
                lastPositionY += 1;
            }
            else if (CanMoveDownRight(lastPositionY, lastPositionX))
            {
                lastPositionX += 1;
                lastPositionY += 1;
            }
            else
            {
                stable = true;
                caveStructure[lastPositionY, lastPositionX] = "O";
            }
        } while (!stable && !outOfBounds);

        fallheight += -1;
        sandUnits += outOfBounds ? 0 : 1;
        PrintCaveSystem();
    } while (!outOfBounds);

    return sandUnits;
};

bool CanMoveDown(int y, int x) => IsWithinBounds(y + 1, x) && caveStructure[y + 1, x] == null;
bool CanMoveDownLeft(int y, int x) => IsWithinBounds(y + 1, x - 1) && caveStructure[y + 1, x - 1] == null;
bool CanMoveDownRight(int y, int x) => IsWithinBounds(y + 1, x + 1) && caveStructure[y + 1, x + 1] == null;
bool IsWithinBounds(int y, int x)
{
    var withinBounds = x > 0 && x < maxX && y <= highestY;
    if (!withinBounds)
        outOfBounds = true;

    return withinBounds;
}

int Part2()
{
    BuildCaveStructure(true);
    maxX = 1000; maxY = highestY + 2;
    do
    {
        bool stable = false;
        int lastPositionY = 0, lastPositionX = 1000;
        do
        {
            if (CanMoveDown(lastPositionY, lastPositionX))
            {
                lastPositionY += 1;
            }
            else if (CanMoveDownLeft(lastPositionY, lastPositionX))
            {
                lastPositionX += -1;
                lastPositionY += 1;
            }
            else if (CanMoveDownRight(lastPositionY, lastPositionX))
            {
                lastPositionX += 1;
                lastPositionY += 1;
            }
            else
            {
                stable = true;
                caveStructure[lastPositionY, lastPositionX] = "O";
            }
        } while (!stable && !outOfBounds);

        fallheight += -1;
        sandUnits += outOfBounds ? 0 : 1;
        PrintCaveSystem();
    } while (!outOfBounds);

    return sandUnits;
};

int ToInt(string s) => int.Parse(s.ToString());

void BuildCaveStructure(bool part2)
{
    var formations = new List<List<int[]>>();
    foreach (var line in input)
    {
        var points = line.Split(" -> ").Select(s =>
        {
            var points = s.Split(",").Select(x => ToInt(x)).ToArray();
            if (points[0] < lowestX) lowestX = points[0];
            if (points[0] > highestX) highestX = points[0];
            if (points[1] > highestY) highestY = points[1];
            return points;
        }).ToList();

        for (int i = 1; i < points.Count(); i++)
            formations.Add(new List<int[]> { { points[i - 1] }, { points[i] } });

        //if(part2)
        //    for (int i = 1; i <= 1000; i++)
        //        formations.Add(new List<int[]> { { points[i - 1] }, { points[i] } });
    }
    if (!part2)
    {
        caveStructure = new string[highestY + 1, highestX - lowestX + 1];
        caveStructure[0, 500 - lowestX] = "+";
    }
    else
    {
        formations.Add(new List<int[]> { new[] { 0, highestY + 2 }, new[] { 1000, highestY + 2 } });
        highestX = 1000;
        caveStructure = new string[highestY + 3, highestX + 1];
        caveStructure[0, 500] = "+";
    }

    foreach (var formation in formations)
    {
        int point1X, point1Y;
        int point2X, point2Y;

        if (part2)
        {
            point1X = formation[0][0]; point1Y = formation[0][1];
            point2X = formation[1][0]; point2Y = formation[1][1];
        }
        else
        {
            point1X = formation[0][0] - lowestX; point1Y = formation[0][1];
            point2X = formation[1][0] - lowestX; point2Y = formation[1][1];
        }

        if (point1X != point2X && point1X > point2X) DrawLineX(point2X, point1X, point1Y);
        if (point1X != point2X && point2X > point1X) DrawLineX(point1X, point2X, point1Y);
        if (point1X == point2X && point1Y > point2Y) DrawLineY(point2Y, point1Y, point1X);
        if (point1X == point2X && point2Y > point1Y) DrawLineY(point1Y, point2Y, point1X);
    }

    PrintCaveSystem();
}

void DrawLineX(int smallX, int bigX, int y)
{
    for (int i = smallX; i <= bigX; i++)
    {
        caveStructure[y, i] = "#";
    }
}

void DrawLineY(int smallY, int bigY, int x)
{
    for (int i = smallY; i <= bigY; i++)
    {
        caveStructure[i, x] = "#";
    }
}

void PrintCaveSystem()
{
    var origRow = Console.CursorTop;
    var origCol = Console.CursorLeft;
    Console.Clear();
    for (int i = 0; i <= highestY; i++)
    {
        var columns = new List<string>();
        for (int j = 0; j <= highestX - lowestX; j++)
        {
            var value = caveStructure[i, j];
            columns.Add(string.IsNullOrWhiteSpace(value) ? " " : value);
        }
        Console.SetCursorPosition(0, i);
        Console.Write(string.Join("", columns));
        //Console.WriteLine(string.Join("", columns));
    }
    Thread.Sleep(20);
}