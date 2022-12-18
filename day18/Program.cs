var input = File.ReadAllLines("input.txt");
var lavaField = new int[23, 23, 23];
var lavaCubes = new List<LavaCube>();

Console.WriteLine(Environment.GetEnvironmentVariable("part") is "part1" ? Part1() : Part2());
//Console.ReadKey();

int Part1()
{
    BuildLavaField();

    return CountSurfaceArea();
};

int Part2()
{
    BuildLavaField();
    PrintLavaField();
    return 1;
    //return CountSurfaceArea() - CountIsolatedSurfaces();
};

void BuildLavaField()
{
    foreach (var line in input)
    {
        var a = line.Split(',').Select(int.Parse).ToArray();
        var cube = new LavaCube(a[0], a[1], a[2]);
        lavaField[cube.x, cube.y, cube.z] = 1;
        lavaCubes.Add(cube);
    }
}

int CountSurfaceArea() => lavaCubes.Select(cube =>
{
    var returnVal = lavaField[cube.x + 1, cube.y, cube.z] == 1 ? 0 : 1;
    returnVal += lavaField[cube.x, cube.y + 1, cube.z] == 1 ? 0 : 1;
    returnVal += lavaField[cube.x, cube.y, cube.z + 1] == 1 ? 0 : 1;

    if (cube.x > 0) returnVal += lavaField[cube.x - 1, cube.y, cube.z] == 1 ? 0 : 1;
    else returnVal++;
    if (cube.y > 0) returnVal += lavaField[cube.x, cube.y - 1, cube.z] == 1 ? 0 : 1;
    else returnVal++;
    if (cube.z > 0) returnVal += lavaField[cube.x, cube.y, cube.z - 1] == 1 ? 0 : 1;
    else returnVal++;

    return returnVal;
}).Sum();

//int CountIsolatedSurfaces()
//{
//    var airPockets = 0;

//    for (var i = 0; i < 22; i++)
//    {
//        for (var j = 0; j < 22; j++)
//        {
//            for (var k = 0; k < 22; k++)
//            {
//                var coveredEverywhere = false;
//                var iMinusOne = i > 0 && lavaField[i - 1, j, k] == 1 ? 1 : 0;
//                var jMinusOne = j > 0 && lavaField[i, j - 1, k] == 1 ? 1 : 0;
//                var kMinusOne = k > 0 && lavaField[i, j, k - 1] == 1 ? 1 : 0;
//                if (lavaField[i, j, k] == 0 &&
//                    lavaField[i + 1, j, k] == 1 &&
//                    lavaField[i, j + 1, k] == 1 &&
//                    lavaField[i, j, k + 1] == 1 &&
//                    iMinusOne == 1 && jMinusOne == 1 && kMinusOne == 1)
//                    coveredEverywhere = true;

//                if (coveredEverywhere)
//                    airPockets += 6;
//            }
//        }
//    }

//    return airPockets;
//}

void PrintLavaField()
{
    Console.Clear();
    for (int k = 0; k <= 22; k++)
    {
        Console.WriteLine($"Layer {k}");
        var printLayer = new List<string>();
        for (int j = 0; j <= 22; j++)
        {
            var backRows = new List<string>();
            for (int i = 0; i <= 22; i++)
            {
                var value = lavaField[i, j, k];
                //printLayer.Add(value == 0 ? "-" : value.ToString());
                backRows.Add(value == 0 ? "-" : value.ToString());
            }
            printLayer.Add(string.Join("", backRows));
        }
        foreach(var layer in printLayer)
        {
            Console.WriteLine(string.Join("", layer));
        }
        //Console.WriteLine(string.Join("", printLayer));
    }
}

record LavaCube(int x, int y, int z);