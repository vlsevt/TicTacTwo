using GameBrain;

public class TicTacTwoBrain
{
    public GameState _gameState;


    public TicTacTwoBrain(GameConfiguration gameConfiguration)
    {
        var gameBoard = new EGamePiece[gameConfiguration.BoardSize][];
        for (var x = 0; x < gameBoard.Length; x++)
        {
            gameBoard[x] = new EGamePiece[gameConfiguration.BoardSize];
        }

        _gameState = new GameState(
            gameBoard,
            gameConfiguration
        );


    }
    public string GetGameStateJson()
    {
        return _gameState.ToString();
    }
    
    public string GetGameConfigName()
    {
        return _gameState.GameConfiguration.Name;
    }
    
    
    public EGamePiece[][] GameBoard
    {
        get => GetBoard();
        private set => _gameState.GameBoard = value;
    }


    public int DimX => _gameState.GameBoard!.Length;
    public int DimY => _gameState.GameBoard![0].Length;

    private EGamePiece[][] GetBoard()
    {
        var copyOfBoard = new EGamePiece[_gameState.GameBoard!.GetLength(0)][];
        //, _gameState.GameBoard.GetLength(1)];
        for (var x = 0; x < _gameState.GameBoard.Length; x++)
        {
            copyOfBoard[x] = new EGamePiece[_gameState.GameBoard[x].Length];
            for (var y = 0; y < _gameState.GameBoard[x].Length; y++)
            {
                copyOfBoard[x][y] = _gameState.GameBoard[x][y];
            }
        }

        return copyOfBoard;
    }


    
    public bool CheckForWinInGrid(int startX, int startY, int gridSize, int winCondition)
{
    if (startX + gridSize - 1 >= DimX || startY + gridSize - 1 >= DimY)
    {
        throw new ArgumentOutOfRangeException("The grid is out of bounds");
    }

    // Check horizontal lines
    for (int y = startY; y < startY + gridSize; y++)
    {
        for (int x = startX; x <= startX + gridSize - winCondition; x++)
        {
            bool win = true;
            for (int i = 0; i < winCondition; i++)
            {
                if (_gameState.GameBoard![x + i][y] != _gameState.GameBoard[x][y] || _gameState.GameBoard[x][y] == EGamePiece.Empty)
                {
                    win = false;
                    break;
                }
            }
            if (win) return true;
        }
    }

    // Check vertical lines
    for (int x = startX; x < startX + gridSize; x++)
    {
        for (int y = startY; y <= startY + gridSize - winCondition; y++)
        {
            bool win = true;
            for (int i = 0; i < winCondition; i++)
            {
                if (_gameState.GameBoard![x][y + i] != _gameState.GameBoard[x][y] || _gameState.GameBoard[x][y] == EGamePiece.Empty)
                {
                    win = false;
                    break;
                }
            }
            if (win) return true;
        }
    }

    // Check vertical lines left to right
    for (int x = startX; x <= startX + gridSize - winCondition; x++)
    {
        for (int y = startY; y <= startY + gridSize - winCondition; y++)
        {
            bool win = true;
            for (int i = 0; i < winCondition; i++)
            {
                if (_gameState.GameBoard![x + i][y + i] != _gameState.GameBoard[x][y] || _gameState.GameBoard[x][y] == EGamePiece.Empty)
                {
                    win = false;
                    break;
                }
            }
            if (win) return true;
        }
    }

    // Check vertical lines right to left
    for (int x = startX + winCondition - 1; x < startX + gridSize; x++)
    {
        for (int y = startY; y <= startY + gridSize - winCondition; y++)
        {
            bool win = true;
            for (int i = 0; i < winCondition; i++)
            {
                if (_gameState.GameBoard![x - i][y + i] != _gameState.GameBoard[x][y] || _gameState.GameBoard[x][y] == EGamePiece.Empty)
                {
                    win = false;
                    break;
                }
            }
            if (win) return true;
        }
    }

    return false;
}
    public bool MakeAMove(int x, int y)
    {
        if (_gameState.GameBoard![x][y] != EGamePiece.Empty)
        {
            return false;
        }

        _gameState.GameBoard[x][y] = _gameState.NextMoveBy;
        _gameState.NextMoveBy = _gameState.NextMoveBy == EGamePiece.X ? EGamePiece.O : EGamePiece.X;
        return true;
    }

    public bool MoveAPiece(int fromX, int fromY, int toX, int toY, EGamePiece player)
    {
        var from = _gameState.GameBoard![fromX][fromY];
        var to = _gameState.GameBoard[toX][toY];
        if (to != EGamePiece.Empty || from == EGamePiece.Empty || from != player)
        {
            return false;
        }

        _gameState.GameBoard[fromX][fromY] = EGamePiece.Empty;
        _gameState.GameBoard[toX][toY] = player;
        
        _gameState.NextMoveBy = _gameState.NextMoveBy == EGamePiece.X ? EGamePiece.O : EGamePiece.X;
        return true;
    }

    public bool MoveAGrid(int x, int y)
    {

        if (x < 0 || y < 0 || x + _gameState.GameConfiguration.GridSize > _gameState.GameConfiguration.BoardSize || y + _gameState.GameConfiguration.GridSize > _gameState.GameConfiguration.BoardSize)
        {
            return false;
        }
        _gameState.NextMoveBy = _gameState.NextMoveBy == EGamePiece.X ? EGamePiece.O : EGamePiece.X;
        _gameState.GameConfiguration = _gameState.GameConfiguration with { GridStartX = x };
        _gameState.GameConfiguration = _gameState.GameConfiguration with { GridStartY = y };
        
        return true;
    }
}
