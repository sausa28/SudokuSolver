var input = new int[81]
{
    9, 0, 2,  0, 0, 0,  0, 0, 0,
    0, 0, 7,  1, 0, 6,  0, 4, 0,
    0, 0, 0,  0, 0, 8,  0, 0, 0,

    0, 5, 0,  9, 0, 0,  0, 0, 8,
    6, 0, 0,  7, 0, 1,  0, 0, 4,
    8, 0, 0,  0, 0, 2,  0, 1, 0,

    0, 0, 0,  2, 0, 0,  0, 0, 0,
    0, 9, 0,  4, 0, 3,  5, 0, 0,
    0, 0, 0,  0, 0, 0,  3, 0, 6
};
var validValues = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

var grid = new int[81];
input.CopyTo((Span<int>)grid);
var startingIndexes = new List<int>();
for (int i = 0; i < grid.Length; i++) {
    if (grid[i] != 0)
	startingIndexes.Add(i);
}

WriteGridToConsole(grid, startingIndexes);
Solve(grid, 0);
WriteGridToConsole(grid, startingIndexes);

bool Solve(int[] grid, int startingIndex)
{
    int i = startingIndex;
    while (i < grid.Length && grid[i] != 0) // Get first empty square
        i++;

    if (i >= grid.Length) // If all squares are populated must be solved (?)
        return true;

    bool isLastCell = i == grid.Length - 1;
    foreach (var value in validValues)
    {
        grid[i] = value;
	UpdateConsoleGrid(i, value);
        
        if (IsValid(grid, i))
        {
            if (isLastCell)
                return true;

            bool solved = Solve(grid, i + 1);
            if (solved)
                return true;
        }
	/*Thread.Sleep(1);*/
    }

    grid[i] = 0;
    UpdateConsoleGrid(i, 0);
    return false;
}

bool IsValid(int[] grid, int index)
{
    return CheckRow(grid, index) && CheckColumn(grid, index) && CheckSquare(grid, index);
}

bool CheckRow(int[] grid, int cellIndex)
{
    return CheckIndices(grid, GetRowIndices(cellIndex));
}

bool CheckColumn(int[] grid, int cellIndex)
{
    return CheckIndices(grid, GetColumnIndices(cellIndex));
}

bool CheckSquare(int[] grid, int cellIndex)
{
    return CheckIndices(grid, GetSquareIndices(cellIndex));
}

bool CheckIndices(int[] grid, IEnumerable<int> indices)
{
    var values = new HashSet<int>();
    foreach (int i in indices)
    {
        if (grid[i] == 0)
            continue;
        if (values.Contains(grid[i]))
            return false;

        values.Add(grid[i]);
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

void WriteGridToConsole(int[] grid, IReadOnlyList<int> startingIndexes)
{
    Console.Clear();
    const string topLine =       "╔═══╤═══╤═══╦═══╤═══╤═══╦═══╤═══╤═══╗";
    const string lineBreak =     "╟───┼───┼───╫───┼───┼───╫───┼───┼───╢";
    const string lineBreakBold = "╠═══╪═══╪═══╬═══╪═══╪═══╬═══╪═══╪═══╣";
    const string bottomLine =    "╚═══╧═══╧═══╩═══╧═══╧═══╩═══╧═══╧═══╝";
    Console.WriteLine(topLine);
    for (int r = 0; r < 9; r++)
    {
        Console.Write("║");
        for (int c = 0; c < 9; c++)
        {
	    int gridIndex = r * 9 + c;
            int value = grid[gridIndex];
            if (value == 0)
                Console.Write("   ");
            else if (startingIndexes.Contains(gridIndex)) {
		Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write($" {value} ");
		Console.ResetColor();
	    }
	    else
                Console.Write($" {value} ");

	    if (c % 3 == 2)
		Console.Write("║");
	    else
		Console.Write("│");
        }
        Console.WriteLine();
	if (r == 8)
	    Console.WriteLine(bottomLine);
	else if (r % 3 == 2)
	    Console.WriteLine(lineBreakBold);
	else
	    Console.WriteLine(lineBreak);
    }
}

void UpdateConsoleGrid(int index, int value) {
    int origLeft = Console.CursorLeft;
    int origTop = Console.CursorTop;

    int r = index / 9;
    int c = index % 9;

    int cursorLeft = 4 * c + 3;
    int cursorTop = 2 * r + 1;
    Console.SetCursorPosition(cursorLeft, cursorTop);
    if (value == 0)
	Console.Write("\b ");
    else
	Console.Write($"\b{value}");

    Console.SetCursorPosition(origLeft, origTop);
}
