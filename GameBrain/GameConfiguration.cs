namespace GameBrain;

public record struct GameConfiguration
{
    public string Name { get; set; }
    public int BoardSize { get; set; }
    public int GridSize { get; set; }
    public int GridStartX { get; set; }
    public int GridStartY { get; set; }
    public int WinCondition { get; set; }
    public int NumberOfPieces { get; set; } 
    
    public GameConfiguration(string name, int boardSize = 5, int gridSize = 3, int gridStartX = 1, int gridStartY = 1, int winCondition = 3, int numberOfPieces = 3)
    {
        Name = name;
        BoardSize = boardSize;
        GridSize = gridSize;
        GridStartX = gridStartX;
        GridStartY = gridStartY;
        WinCondition = winCondition;
        NumberOfPieces = numberOfPieces;
    }

    public override string ToString() =>
        $"Configuration {Name}, " +
        $"Board {BoardSize}x{BoardSize}, " +
        $"Grid {GridSize}x{GridSize}, " +
        $"to win: {WinCondition}, " +
        $"can move piece after {NumberOfPieces} moves";
    
    
}