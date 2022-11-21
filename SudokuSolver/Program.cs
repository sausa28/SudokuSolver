var input = new int?[81]
{
    null,   null,   null,   null,   8,      null,   7,      null,   null,
    3,      7,      null,   null,   null,   5,      null,   4,      null,
    null,   null,      9,   null,   null,   2,      null,   null,   null,
    4,      5,      null,      1,   null,   null,   null,   3,      null,
    null,   null,   null,   null,   null,   null,   null,   null,   null,
    null,      9,   null,   null,   null,      3,   null,      7,      6,
    null,   null,   null,      4,   null,   null,      9,   null,   null,
    null,      3,   null,      8,   null,   null,   null,      6,      2,
    null,   null,      1,   null,      5,   null,   null,   null,   null,
};
var validValues = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

var grid = new int?[81];
input.CopyTo((Span<int?>)grid);

Solve(grid, 0);

WriteGridToConsole(grid);

bool Solve(int?[] grid, int startingIndex)
{
    int i = startingIndex;
    while (grid[i] is not null) // Get first empty square
        i++;

    if (i >= grid.Length) // If all squares are populated must be solved (?)
        return true;

    bool isLastCell = i == grid.Length - 1;
    foreach (var value in validValues)
    {
        grid[i] = value;
        
        if (IsValid(grid, i))
        {
            if (isLastCell)
                return true;

            bool solved = Solve(grid, i + 1);
            if (solved)
                return true;
        }
    }

    //WriteGridToConsole(grid);
    grid[i] = null;
    return false;
}

bool IsValid(int?[] grid, int index)
{
    return CheckRow(grid, index) && CheckColumn(grid, index) && CheckSquare(grid, index);
}

bool CheckRow(int?[] grid, int cellIndex)
{
    return CheckIndices(grid, GetRowIndices(cellIndex));
}

bool CheckColumn(int?[] grid, int cellIndex)
{
    return CheckIndices(grid, GetColumnIndices(cellIndex));
}

bool CheckSquare(int?[] grid, int cellIndex)
{
    return CheckIndices(grid, GetSquareIndices(cellIndex));
}

bool CheckIndices(int?[] grid, IEnumerable<int> indices)
{
    var values = new HashSet<int>();
    foreach (int i in indices)
    {
        if (grid[i] is null)
            continue;
        if (values.Contains(grid[i]!.Value))
            return false;

        values.Add(grid[i]!.Value);
    }

    return true;
}

IEnumerable<int> GetRowIndices(int cellIndex)
{
    int rowIndex = cellIndex / 9;
    for (int i = rowIndex * 9; i < (rowIndex + 1) * 9; i++)
    {
        yield return i;
    }
}

IEnumerable<int> GetColumnIndices(int cellIndex)
{
    int columnIndex = cellIndex % 9;
    for (int i = columnIndex; i < columnIndex + 81 ; i += 9)
    {
        yield return i;
    }
}

IEnumerable<int> GetSquareIndices(int cellIndex)
{
    int squareRowIndex = (cellIndex / 9) / 3;
    int squareColumnIndex = (cellIndex % 9) / 3;
    int squareTopLeft = squareRowIndex * 27 + squareColumnIndex * 3;

    for (int r = 0; r < 3; r++)
    {
        for (int c = 0; c < 3; c++)
        {
            yield return squareTopLeft + r * 9 + c;
        }
    }
}

void WriteGridToConsole(int?[] grid)
{
    Console.Clear();
    const string lineBreak = "-------------------";
    Console.WriteLine(lineBreak);
    for (int r = 0; r < 9; r++)
    {
        Console.Write("|");
        for (int c = 0; c < 9; c++)
        {
            int? value = grid[r * 9 + c];
            if (value is null)
                Console.Write(" ");
            else
                Console.Write(value.ToString());

            Console.Write("|");
        }
        Console.WriteLine();
        Console.WriteLine(lineBreak);
    }
}