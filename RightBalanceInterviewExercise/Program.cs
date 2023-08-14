public class Minesweeper
{
    private const int ROWS = 8;
    private const int COLS = 8;
    private const int MINES = 10;
    private const char FOG_SYMBOL = '#';
    private const char MINE_SYMBOL = '*';

    private static readonly int[,] board = new int[ROWS, COLS];
    private static readonly char[,] fogOfWar = new char[ROWS, COLS];
    private static readonly List<(int, int)> mines = new List<(int, int)>();

    static Minesweeper()
    {
        InitializeBoard();
        PlaceMines();
        PopulateBoardWithMineCounts();
        InitializeFogOfWar();
    }

    public static void Main()
    {
        while (true)
        {
            DisplayBoard();

            GetUserInput(out var x, out var y);

            Reveal(x, y);

            if (CheckWin())
            {
                DisplayBoard();
                Console.WriteLine("You win");
                break;
            }
            else if (board[x, y] == -1)
            {
                DisplayBoard();
                Console.WriteLine("You lost");
                break;
            }
        }
    }

    private static void InitializeBoard()
    {
        for (int row = 0; row < ROWS; row++)
        {
            for (int col = 0; col < COLS; col++)
            {
                board[row, col] = 0;
            }
        }
    }

    private static void PlaceMines()
    {
        Random random = new Random();
        for (int i = 0; i < MINES; i++)
        {
            int row, col;
            do
            {
                row = random.Next(ROWS);
                col = random.Next(COLS);
            } while (board[row, col] == -1);

            board[row, col] = -1;
            mines.Add((row, col));
        }
    }

    private static void PopulateBoardWithMineCounts()
    {
        foreach ((int row, int col) in mines)
        {
            for (int r = Math.Max(0, row - 1); r <= Math.Min(row + 1, ROWS - 1); r++)
            {
                for (int c = Math.Max(0, col - 1); c <= Math.Min(col + 1, COLS - 1); c++)
                {
                    if (board[r, c] != -1)
                    {
                        board[r, c]++;
                    }
                }
            }
        }
    }

    private static void InitializeFogOfWar()
    {
        for (int row = 0; row < ROWS; row++)
        {
            for (int col = 0; col < COLS; col++)
            {
                fogOfWar[row, col] = FOG_SYMBOL;
            }
        }
    }

    private static void DisplayBoard()
    {
        Console.Clear();

        Console.Write("  ");
        for (int col = 0; col < COLS; col++)
        {
            Console.Write(col + " ");
        }
        Console.WriteLine();

        for (int row = 0; row < ROWS; row++)
        {
            Console.Write(row + " ");
            for (int col = 0; col < COLS; col++)
            {
                Console.Write(fogOfWar[row, col] + " ");
            }
            Console.WriteLine();
        }
    }

    private static void Reveal(int x, int y)
    {
        if (x < 0 || x >= ROWS || y < 0 || y >= COLS || fogOfWar[x, y] != '#') return;

        if (board[x, y] == -1)
        {
            fogOfWar[x, y] = MINE_SYMBOL;
            return;
        }

        fogOfWar[x, y] = char.Parse(board[x, y].ToString());

        if (board[x, y] == 0)
        {
            for (int r = Math.Max(0, x - 1); r <= Math.Min(x + 1, ROWS - 1); r++)
            {
                for (int c = Math.Max(0, y - 1); c <= Math.Min(y + 1, COLS - 1); c++)
                {
                    Reveal(r, c);
                }
            }
        }
    }

    private static bool CheckWin()
    {
        for (int r = 0; r < ROWS; r++)
        {
            for (int c = 0; c < COLS; c++)
            {
                if (fogOfWar[r, c] == '#' && board[r, c] != -1)
                {
                    return false;
                }
            }
        }
        return true;
    }

    private static void GetUserInput(out int x, out int y)
    {
        do
        {
            x = GetCoordinateInput("Enter the X coordinate (row) to reveal (0 to " + (ROWS - 1) + "): ");
            y = GetCoordinateInput("Enter the Y coordinate (column) to reveal (0 to " + (COLS - 1) + "): ");

            if (x < 0 || x >= ROWS || y < 0 || y >= COLS)
            {
                Console.WriteLine("Invalid coordinates. Please enter again.");
            }
        } while (x < 0 || x >= ROWS || y < 0 || y >= COLS);
    }

    private static int GetCoordinateInput(string prompt)
    {
        do
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out int coordinate))
            {
                return coordinate;
            }
            Console.WriteLine("Invalid input. Please enter a number.");
        } while (true);
    }
}