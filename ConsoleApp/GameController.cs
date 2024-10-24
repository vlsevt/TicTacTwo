using ConsoleUI;
using DAL;
using GameBrain;
using MenuSystem;

namespace ConsoleApp;

public static class GameController
{
    private static readonly IConfigRepository ConfigRepository = new ConfigRepositoryJson();
    private static readonly IGameRepository GameRepository = new GameRepositoryJson();


    private static int _gameStage = 1; 
    private static bool _movingGrid;

    public static void LoadExistingGame(TicTacTwoBrain gameInstance, string savedGameFile)
{
    Console.WriteLine("Game loaded successfully!");

    // Begin the main game loop with the loaded game instance
    MainLoop(gameInstance, savedGameFile);
}
    public static string MainLoop(TicTacTwoBrain? gameInstance = null, string savedGameFile = "")
    {
        GameConfiguration chosenConfig;

        // If no game instance is passed, start a new game
        if (gameInstance == null)
        {
            var chosenConfigShortcut = ChooseConfiguration();

            if (!int.TryParse(chosenConfigShortcut, out var configNo))
            {
                return chosenConfigShortcut;
            }

            // Initialize chosenConfig from the configuration repository
            chosenConfig = ConfigRepository.GetConfigurationByName(
                ConfigRepository.GetConfigurationNames()[configNo]
            );

            gameInstance = new TicTacTwoBrain(chosenConfig);
        }
        else
        {
            // If the gameInstance is passed (i.e., loaded game), get the configuration from the game state
            chosenConfig = gameInstance._gameState.GameConfiguration;
        }

        bool hasWinner = false; // Flag to exit the loop when there's a winner
        
        do
        {
            //var (xCount, oCount) = CountPieces(gameInstance);
            EGamePiece playerToMove;

            Visualizer.DrawBoard(gameInstance, chosenConfig.GridStartX, chosenConfig.GridStartY, chosenConfig.GridSize);
            playerToMove = gameInstance._gameState.NextMoveBy;
            var playerPieces = CountPlayerPieces(gameInstance, playerToMove);

            if (playerPieces < 2)
            {
                Console.WriteLine($"Player to move {playerToMove.ToString()}");
                Console.WriteLine("Give me coordinates <x,y> or save ");
                _gameStage = 1;
            }
            else if (playerPieces < chosenConfig.WinCondition)
            {
                Console.WriteLine($"Player to move {playerToMove.ToString()}");
                Console.WriteLine("Give me coordinates <x,y>, save or mg (move the grid) ");
                _gameStage = 2;
            }
            else
            {
                Console.WriteLine($"Player to move {playerToMove.ToString()}");
                Console.WriteLine("Give me coordinates to move the piece <x,y to x,y>, save or mg (move the grid) ");
                _gameStage = 3;
            }

            if (_gameStage == 1)
            {
                var input = Console.ReadLine();

                if (input == null || input.ToLower() == "save")
                {
                    Console.WriteLine("Game saved or exited.");
                    GameRepository.SaveGame(
                        gameInstance.GetGameStateJson(),
                        gameInstance.GetGameConfigName()
                    );

                    break;
                }

                var inputSplit = input.Split(",");
                if (inputSplit.Length != 2 ||
                    !int.TryParse(inputSplit[0], out var inputX) ||
                    !int.TryParse(inputSplit[1], out var inputY))
                {
                    Console.WriteLine("Invalid input. Please enter coordinates in format <x,y>.");
                    continue;
                }

                if (inputX < 0 || inputX >= gameInstance.DimX || inputY < 0 || inputY >= gameInstance.DimY)
                {
                    Console.WriteLine("Invalid move. Coordinates are out of bounds.");
                    continue;
                }

                if (!gameInstance.MakeAMove(inputX, inputY))
                {
                    Console.WriteLine("Invalid move. Try again.");
                    continue;
                }
            }

            if (_gameStage == 2)
            {
                var input = Console.ReadLine();

                if (input != null && input.ToLower() == "mg")
                {
                    _movingGrid = true;
                    Console.WriteLine("Give me coordinates where you want to move the grid <x,y>");
                    input = Console.ReadLine();
                }

                if (input == null || input.ToLower() == "save")
                {
                    Console.WriteLine("Game saved or exited.");
                    GameRepository.SaveGame(
                        gameInstance.GetGameStateJson(),
                        gameInstance.GetGameConfigName()
                    );
                    break;
                }

                var inputSplit = input.Split(",");
                if (inputSplit.Length != 2 ||
                    !int.TryParse(inputSplit[0], out var inputX) ||
                    !int.TryParse(inputSplit[1], out var inputY))
                {
                    Console.WriteLine("Invalid input. Please enter coordinates in format <x,y>.");
                    continue;
                }

                if (inputX < 0 || inputX >= gameInstance.DimX || inputY < 0 || inputY >= gameInstance.DimY)
                {
                    Console.WriteLine("Invalid move. Coordinates are out of bounds.");
                    continue;
                }

                if (_movingGrid)
                {
                    if (!gameInstance.MoveAGrid(inputX, inputY))
                    {
                        Console.WriteLine("Invalid move. Try again.");
                        continue;
                    }
                    chosenConfig.GridStartX = inputX;
                    chosenConfig.GridStartY = inputY;
                    _movingGrid = false;
                } else if (!gameInstance.MakeAMove(inputX, inputY))
                {
                    Console.WriteLine("Invalid move. Try again.");
                    continue;
                }
            }

            if (_gameStage == 3)
            {
                var input = Console.ReadLine();
                
                if (input != null && input.ToLower() == "mg")
                {
                    _movingGrid = true;
                    Console.WriteLine("Give me coordinates where you want to move the grid <x,y>");
                    input = Console.ReadLine();
                }

                if (input == null || input.ToLower() == "save")
                {
                    Console.WriteLine("Game saved or exited.");
                    GameRepository.SaveGame(
                        gameInstance.GetGameStateJson(),
                        gameInstance.GetGameConfigName()
                    );

                    break;
                }

                if (_movingGrid)
                {
                    var inputSplit = input.Split(",");
                    if (inputSplit.Length != 2 ||
                        !int.TryParse(inputSplit[0], out var inputX) ||
                        !int.TryParse(inputSplit[1], out var inputY))
                    {
                        Console.WriteLine("Invalid input. Please enter coordinates in format <x,y>.");
                        continue;
                    }
                    if (!gameInstance.MoveAGrid(inputX, inputY))
                    {
                        Console.WriteLine("Invalid move. Try again.");
                        continue;
                    }
                    chosenConfig.GridStartX = inputX;
                    chosenConfig.GridStartY = inputY;
                    _movingGrid = false;
                }
                else
                {
                    var inputSplit = input.Split(" to ");
                    var inputFrom = inputSplit[0].Split(",");
                    var inputTo = inputSplit[1].Split(",");
                    if (inputFrom.Length != 2 ||
                        !int.TryParse(inputFrom[0], out var inputFromX) ||
                        !int.TryParse(inputFrom[1], out var inputFromY) ||
                        !int.TryParse(inputTo[0], out var inputToX) ||
                        !int.TryParse(inputTo[1], out var inputToY))
                    {
                        Console.WriteLine("Invalid input. Please enter coordinates in format <x,y to x,y>.");
                        continue;
                    }
                    
                    if (inputToX < 0 || inputToX >= gameInstance.DimX || inputToY < 0 || inputToY >= gameInstance.DimY || 
                        inputFromX < 0 || inputFromX >= gameInstance.DimX || inputFromY < 0 || inputFromY >= gameInstance.DimY)
                    {
                        Console.WriteLine("Invalid move. Coordinates are out of bounds.");
                        continue;
                    }
                    
                    if (!gameInstance.MoveAPiece(inputFromX, inputFromY, inputToX, inputToY, playerToMove))
                    {
                        Console.WriteLine("Invalid move. Try again.");
                        continue;
                    }
                }
            }


            if (gameInstance.CheckForWinInGrid(chosenConfig.GridStartX, chosenConfig.GridStartY, chosenConfig.GridSize,chosenConfig.WinCondition))
            {
                Console.WriteLine($"We have a winner in the grid starting at ({chosenConfig.GridStartX}, {chosenConfig.GridStartY})!");
                hasWinner = true;
            }

            if (hasWinner)
            {
                Visualizer.DrawBoard(gameInstance, chosenConfig.GridStartX, chosenConfig.GridStartY, chosenConfig.GridSize);
                Console.WriteLine("Game over!");

                // Check if there's a saved game file and delete it
                if (!string.IsNullOrEmpty(savedGameFile) && File.Exists(savedGameFile))
                {
                    try
                    {
                        File.Delete(savedGameFile);
                        Console.WriteLine($"The saved game {savedGameFile} has been deleted because the game is over.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to delete saved game file: {ex.Message}");
                    }
                }

                break;  // Exit the game loop since the game is won
            }

        } while (!hasWinner); 

        Console.WriteLine("Game over!");

        return "Game finished.";
    }


    private static string ChooseConfiguration()
    {
        var configMenuItems = new List<MenuItem>();

        for (var i = 0; i < ConfigRepository.GetConfigurationNames().Count; i++)
        {
            var returnValue = i.ToString();
            configMenuItems.Add(new MenuItem()
            {
                Title = ConfigRepository.GetConfigurationNames()[i],
                Shortcut = (i + 1).ToString(),
                MenuItemAction = () => returnValue
            });
        }

        var configMenu = new Menu(EMenuLevel.Secondary,
            "TIC-TAC-TOE - choose game config",
            configMenuItems,
            isCustomMenu: true
        );

        return configMenu.Run();
    }

    private static int CountPlayerPieces(TicTacTwoBrain gameInstance, EGamePiece player)
    {
        int count = 0;
        for (var y = 0; y < gameInstance.DimY; y++)
        {
            for (var x = 0; x < gameInstance.DimX; x++)
            {
                if (gameInstance.GameBoard[x][y] == player)
                {
                    count++;
                }
            }
        }
        return count;
    }
}
