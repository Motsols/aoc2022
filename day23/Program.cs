using System.Text;

var input = File.ReadAllLines("input.txt");
int addCoords = 1500, maxX = input[0].Length+addCoords*2, maxY = input.Length + addCoords*2, posMaxX = maxX - 1, posMaxY = maxY - 1, roundMoves = 0;
var map = new int[maxX, maxY];
var elves = new List<Elf>();
var moves = new int[maxX, maxY];
var currentDirection = Direction.North;
var currentPosition = new Position(0, 0);

BuildMap();
Console.WriteLine(Environment.GetEnvironmentVariable("part") is "part1" ? Part1() : Part2());

int Part1()
{
    Console.WriteLine("Initial state:");
    PrintMap();
    for (var i = 1; i <= 10; i++)
    {
        PlanMove();
        Move();
        ChangeDirection();
        moves = new int[maxX, maxY];
        PrintMap();
    }

    var count = CountFreeSpots();
    return count;
}

int Part2()
{
    var roundNumber = 0;
    do
    {
        roundNumber++;
        roundMoves = 0;
        PlanMove();
        Move();
        ChangeDirection();
        moves = new int[maxX, maxY];

    } while (roundMoves != 0);

    return roundNumber;
}

void ChangeDirection()
{
    currentDirection = currentDirection switch
    {
        Direction.North => Direction.South,
        Direction.South => Direction.West,
        Direction.West => Direction.East,
        Direction.East => Direction.North,
        _ => currentDirection
    };
}

void PlanMove()
{
    foreach (var elf in elves)
    {
        currentPosition = elf.CurrentPos;
        var newPosition = new Position(-1, -1);

        if (FreeAround())
            newPosition = elf.CurrentPos;
        else
            switch (currentDirection)
            {
                case Direction.North:
                    if (FreeNorth())
                        newPosition = elf.CurrentPos with { Y = elf.CurrentPos.Y - 1 };
                    else if (FreeSouth())
                        newPosition = elf.CurrentPos with { Y = elf.CurrentPos.Y + 1 };
                    else if (FreeWest())
                        newPosition = elf.CurrentPos with { X = elf.CurrentPos.X - 1 };
                    else if (FreeEast())
                        newPosition = elf.CurrentPos with { X = elf.CurrentPos.X + 1 };
                    else
                        newPosition = currentPosition;
                    break;
                case Direction.South:
                    if (FreeSouth())
                        newPosition = elf.CurrentPos with { Y = elf.CurrentPos.Y + 1 };
                    else if (FreeWest())
                        newPosition = elf.CurrentPos with { X = elf.CurrentPos.X - 1 };
                    else if (FreeEast())
                        newPosition = elf.CurrentPos with { X = elf.CurrentPos.X + 1 };
                    else if (FreeNorth())
                        newPosition = elf.CurrentPos with { Y = elf.CurrentPos.Y - 1 };
                    else
                        newPosition = currentPosition;
                    break;
                case Direction.West:
                    if (FreeWest())
                        newPosition = elf.CurrentPos with { X = elf.CurrentPos.X - 1 };
                    else if (FreeEast())
                        newPosition = elf.CurrentPos with { X = elf.CurrentPos.X + 1 };
                    else if (FreeNorth())
                        newPosition = elf.CurrentPos with { Y = elf.CurrentPos.Y - 1 };
                    else if (FreeSouth())
                        newPosition = elf.CurrentPos with { Y = elf.CurrentPos.Y + 1 };
                    else
                        newPosition = currentPosition;
                    break;
                case Direction.East:
                    if (FreeEast())
                        newPosition = elf.CurrentPos with { X = elf.CurrentPos.X + 1 };
                    else if (FreeNorth())
                        newPosition = elf.CurrentPos with { Y = elf.CurrentPos.Y - 1 };
                    else if (FreeSouth())
                        newPosition = elf.CurrentPos with { Y = elf.CurrentPos.Y + 1 };
                    else if (FreeWest())
                        newPosition = elf.CurrentPos with { X = elf.CurrentPos.X - 1 };
                    else
                        newPosition = currentPosition;
                    break;
            }

        if (newPosition.X != -1)
        {
            elf.NewPos = newPosition;
            moves[newPosition.X, newPosition.Y] += 1;
        }

        if (currentPosition.X != elf.NewPos.X || currentPosition.Y != elf.NewPos.Y)
            roundMoves++;
    }
}

void Move()
{
    foreach (var elf in elves)
    {
        if (elf.NewPos.X == -1 || moves[elf.NewPos.X, elf.NewPos.Y] > 1)
        {
            continue;
        }

        map[elf.CurrentPos.X, elf.CurrentPos.Y] = 0;
        map[elf.NewPos.X, elf.NewPos.Y] = 1;
        elf.CurrentPos = elf.NewPos;
    }
}

int CountFreeSpots()
{
    var lowestX = elves.Min(e => e.CurrentPos.X);
    var highestX = elves.Max(e => e.CurrentPos.X);
    var lowestY = elves.Min(e => e.CurrentPos.Y);
    var highestY = elves.Max(e => e.CurrentPos.Y);
    var freeSpots = 0;

    Console.WriteLine("Final grid:");

    for (var i = lowestY; i <= highestY; i++)
    {
        var lineY = new StringBuilder();
        for (var j = lowestX; j <= highestX; j++)
        {
            lineY.Append(map[j, i].ToString());
            if (map[j, i] == 0)
                freeSpots++;
        }
        Console.WriteLine(lineY.ToString());
    }
    

    return freeSpots;
}

bool FreeAround()
{
    var free = true;
    //NorthWest
    if (currentPosition.Y != 0 && currentPosition.X != 0 && map[currentPosition.X - 1, currentPosition.Y - 1] != 0)
        free = false;
    //North
    else if (currentPosition.Y != 0 && map[currentPosition.X, currentPosition.Y - 1] != 0)
        free = false;
    //NorthEast
    else if (currentPosition.Y != 0 && currentPosition.X < posMaxX && map[currentPosition.X + 1, currentPosition.Y - 1] != 0)
        free = false;
    //East
    else if (currentPosition.X != posMaxX && map[currentPosition.X + 1, currentPosition.Y] != 0)
        free = false;
    //SouthEast
    else if (currentPosition.Y != posMaxY && currentPosition.X != posMaxX && map[currentPosition.X + 1, currentPosition.Y + 1] != 0)
        free = false;
    //South
    else if (currentPosition.Y != posMaxY && map[currentPosition.X, currentPosition.Y + 1] != 0)
        free = false;
    //SouthWest
    else if (currentPosition.Y != posMaxY && currentPosition.X != 0 && map[currentPosition.X - 1, currentPosition.Y + 1] != 0)
        free = false;
    //West
    else if (currentPosition.X != 0 && map[currentPosition.X - 1, currentPosition.Y] != 0)
        free = false;
    return free;
}

bool FreeNorth() => NorthWest() && North() && NorthEast();

bool FreeSouth()
{
    var a = SouthWest();
    var b = South();
    var c = SouthEast();
    return a && b && c;
}
bool FreeWest() => NorthWest() && West() && SouthWest();
bool FreeEast() => NorthEast() && East() && SouthEast();

bool NorthWest()
{
    if (currentPosition.X == 0 || currentPosition.Y == 0)
        return false;
    return map[currentPosition.X - 1, currentPosition.Y - 1] == 0;
}

bool North()
{
    if (currentPosition.Y == 0)
        return false;

    return map[currentPosition.X, currentPosition.Y - 1] == 0;
}

bool NorthEast()
{
    if (currentPosition.X >= posMaxX || currentPosition.Y == 0)
        return false;

    return map[currentPosition.X + 1, currentPosition.Y - 1] == 0;
}

bool East()
{
    if (currentPosition.X >= posMaxX)
        return false;

    return map[currentPosition.X + 1, currentPosition.Y] == 0;
}

bool SouthEast()
{
    if (currentPosition.X == posMaxX || currentPosition.Y >= posMaxY)
        return false;

    return map[currentPosition.X + 1, currentPosition.Y + 1] == 0;
}

bool South()
{
    if (currentPosition.Y >= posMaxY)
        return false;

    return map[currentPosition.X, currentPosition.Y + 1] == 0;
}

bool SouthWest()
{
    if (currentPosition.X == 0 || currentPosition.Y >= posMaxY)
        return false;

    return map[currentPosition.X - 1, currentPosition.Y + 1] == 0;
}

bool West()
{
    if (currentPosition.X == 0)
        return false;

    return map[currentPosition.X - 1, currentPosition.Y] == 0;
}

void BuildMap()
{
    var elfNumber = 0;
    for (var i = addCoords; i < input.Length + addCoords; i++)
    {
        var columns = input[i-addCoords].ToArray();
        for (var j = addCoords; j < columns.Length + addCoords; j++)
        {
            if (columns[j-addCoords].ToString() == ".")
                continue;

            elves.Add(new Elf(elfNumber, new Position(j, i), new Position(j, i)));
            elfNumber++;

            map[j, i] = 1;
        }
    }
}

void PrintMap()
{
    var newMaxY = maxY - 1;
    var newMaxX = maxX - 1;
    for (var i = 0; i < maxY; i++)
    {
        var lineY = new StringBuilder();
        for (var j = 0; j < maxX; j++)
        {
            lineY.Append(map[j, i].ToString());
        }
        Console.WriteLine(lineY.ToString());
    }
    Console.WriteLine("---");
}

class Elf
{
    public Elf(int id, Position currentPos, Position newPos)
    {
        Id = id;
        CurrentPos = currentPos;
        NewPos = newPos;
    }
    public int Id { get; set; }
    public Position CurrentPos { get; set; }
    public Position NewPos { get; set; }

}
record Position(int X, int Y);

enum Direction { North, East, South, West }