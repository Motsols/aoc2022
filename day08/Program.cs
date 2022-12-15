var input = File.ReadAllLines("input.txt");
int[][] arrays;
int rows, columns;

Console.WriteLine(Environment.GetEnvironmentVariable("part") is "part1" ? Part1() : Part2());

int Part1()
{
    arrays = input.Select(l => l.Select(ToInt).ToArray()).ToArray();
    rows = arrays.Length;
    columns = arrays[0].Length;
    var visibleTrees = 0;
    for (var y = 0; y < rows; y++)
    {
        for (var x = 0; x < columns; x++)
        {
            if (Visible(y, x, arrays[y][x], -1, 0))
                visibleTrees++;
            else if (Visible(y, x, arrays[y][x], 1, 0))
                visibleTrees++;
            else if (Visible(y, x, arrays[y][x], 0, -1))
                visibleTrees++;
            else if (Visible(y, x, arrays[y][x], 0, 1))
                visibleTrees++;
        }
    }

    return visibleTrees;
}

int Part2()
{
    arrays = input.Select(l => l.Select(ToInt).ToArray()).ToArray();
    rows = arrays.Length;
    columns = arrays[0].Length;
    var scenicScores = new List<int>();
    for (var y = 0; y < rows; y++)
    {
        for (var x = 0; x < columns; x++)
        {
            var scenicScore = ScenicValue(y, x, arrays[y][x], -1, 0)
                              * ScenicValue(y, x, arrays[y][x], 1, 0)
                              * ScenicValue(y, x, arrays[y][x], 0, -1)
                              * ScenicValue(y, x, arrays[y][x], 0, 1);

            scenicScores.Add(scenicScore);
        }
    }

    return scenicScores.Max();
}

int ToInt(char c) => int.Parse(c.ToString());

bool Visible(int y, int x, int height, int incrementX, int incrementY)
{
    int newX = x + incrementX, newY = y + incrementY;
    var result = (newX < 0 || newX == columns || newY < 0 || newY == rows) || arrays[newY][newX] < height && Visible(newY, newX, height, incrementX, incrementY);
    return result;
}

int ScenicValue(int y, int x, int height, int incrementX, int incrementY)
{
    int newX = x + incrementX, newY = y + incrementY, scenicValue = 0;
    if (newX < 0 || newX >= columns || newY < 0 || newY >= rows) return 0;

    if (newX > 0 && newX < columns && newY > 0 && newY < rows && arrays[newY][newX] < height)
        scenicValue += ScenicValue(newY, newX, height, incrementX, incrementY);
    
    return scenicValue + 1;
}