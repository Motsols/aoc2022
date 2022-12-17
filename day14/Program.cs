var input = File.ReadAllLines("input.txt");
int lowestX = int.MaxValue, highestX = 0, highestY = 0, sandUnits = 0, minX = 0, maxX = int.MaxValue, maxY = 0, switchLeft = 0;
bool outOfBounds = false;
string[,] caveStructure;

Console.WriteLine(Environment.GetEnvironmentVariable("part") is "part1" ? Part1() : Part2());

int Part1()
{
    BuildCaveStructure(false);
    maxX = highestX; maxY = highestY;
    do
    {
        int lastPositionY = 0, lastPositionX = 500 - lowestX;

        MoveSand(lastPositionY, lastPositionX);
    } while (!outOfBounds);

    return sandUnits;
};

int Part2()
{
    switchLeft = 300;
    BuildCaveStructure(true);
    maxX = 1001-(switchLeft*2); maxY = highestY + 2;
    do
    {
        bool stable = false;
        int lastPositionY = 0, lastPositionX = 500 - switchLeft;

        if (!CanSpawnSand(lastPositionY, lastPositionX))
            outOfBounds = true;

        MoveSand(lastPositionY, lastPositionX);
    } while (!outOfBounds);

    return sandUnits;
};

void MoveSand(int lastPositionY, int lastPositionX)
{
    bool stable = false;

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

    sandUnits += outOfBounds ? 0 : 1;
    //if (sandUnits < 1500 || sandUnits > 25158)
    //    PrintCaveSystem(true);
}

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
    }

    if (!part2)
    {
        caveStructure = new string[highestY + 1, highestX - lowestX + 1];
        caveStructure[0, 500 - lowestX] = "+";
    }
    else
    {
        formations.Add(new List<int[]> { new[] { 0+switchLeft, highestY + 2 }, new[] { 800, highestY + 2 } });
        highestX = 1000-switchLeft;
        highestY += 3;
        caveStructure = new string[highestY + 3, highestX + 1];
        caveStructure[0, 500-switchLeft] = "+";
    }

    foreach (var formation in formations)
    {
        int point1X, point1Y;
        int point2X, point2Y;

        if (part2)
        {
            point1X = formation[0][0] - switchLeft; point1Y = formation[0][1];
            point2X = formation[1][0] - switchLeft; point2Y = formation[1][1];
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

    //PrintCaveSystem(part2);
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

int ToInt(string s) => int.Parse(s.ToString());
bool CanSpawnSand(int y, int x) => caveStructure[y, x] == "+";
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

//void PrintCaveSystem(bool part2)
//{
//    var origRow = Console.CursorTop;
//    var origCol = Console.CursorLeft;
//    var columns = part2 ? highestX : highestX - lowestX;

//    Console.Clear();
//    for (int i = 0; i <= highestY; i++)
//    {
//        var printColumns = new List<string>();
//        for (int j = 0; j <= columns; j++)
//        {
//            var value = caveStructure[i, j];
//            printColumns.Add(string.IsNullOrWhiteSpace(value) ? " " : value);
//        }
//        Console.SetCursorPosition(0, i);
//        Console.Write(string.Join("", printColumns));
//        //Console.WriteLine(string.Join("", columns));
//    }
//    Thread.Sleep(70);
//}