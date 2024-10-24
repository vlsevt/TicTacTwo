using GameBrain;

namespace ConsoleUI;

public static class Visualizer
{
    public static void DrawBoard(TicTacTwoBrain gameInstance, int gridStartX, int gridStartY, int gridSize)
{
    
    for (var y = 0; y < gameInstance.DimY; y++)
    {
        for (var x = 0; x < gameInstance.DimX; x++)
        {
            bool isInGrid = (x >= gridStartX && x < gridStartX + gridSize) 
                         && (y >= gridStartY && y < gridStartY + gridSize);

            if (isInGrid)
            {
                Console.BackgroundColor = ConsoleColor.DarkGray;
            }

            Console.Write(" " + DrawGamePiece(gameInstance.GameBoard[x][y]) + " ");
            Console.ResetColor();

            if (x < gameInstance.DimX - 1)
            {
                if (isInGrid && x < gridStartX + gridSize - 1)
                {
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                }
                Console.Write("│");
                Console.ResetColor();
            }
        }

        Console.WriteLine();

        if (y < gameInstance.DimY - 1)
        {
            for (var x = 0; x < gameInstance.DimX; x++)
            {
                bool isInGrid = (x >= gridStartX && x < gridStartX + gridSize)
                             && (y >= gridStartY && y < gridStartY + gridSize - 1);

                if (isInGrid)
                {
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                }

                Console.Write("───");
                Console.ResetColor();

                if (x < gameInstance.DimX - 1)
                {
                    if (isInGrid && x < gridStartX + gridSize - 1)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                    }
                    Console.Write("┼");
                    Console.ResetColor();
                }
            }
            Console.WriteLine();
        }
    }
}
    
    private static string DrawGamePiece(EGamePiece piece)
    {
        switch (piece)
        {
            case EGamePiece.O:
                Console.ForegroundColor = ConsoleColor.Green; 
                return "O";
            case EGamePiece.X:
                Console.ForegroundColor = ConsoleColor.Red; 
                return "X";
            default:
                Console.ResetColor();
                return " "; 
        }
    }
}
