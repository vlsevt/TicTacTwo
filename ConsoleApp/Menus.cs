using DAL;
using GameBrain;
using MenuSystem;

namespace ConsoleApp;

public static class Menus
{
    public static EGamePiece FirstMoveBy { get; set; } = EGamePiece.X;

    private static readonly Menu CreateConfigMenu = new Menu(
        EMenuLevel.Deep,
        "Create New Configuration",
        new List<MenuItem>
        {
            new MenuItem
            {
                Shortcut = "1",
                Title = "Set Board Size",
                MenuItemAction = () =>
                {
                    Console.WriteLine("Enter the board size (3-20, e.g., 5 for a 5x5 board): ");
                    if (int.TryParse(Console.ReadLine(), out int boardSize))
                    {
                        if (boardSize >= 3 && boardSize <= 20)
                        {
                            TempConfig.BoardSize = boardSize;
                            Console.WriteLine($"Board size set to {boardSize}x{boardSize}.");
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please enter a number between 3 and 20.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a valid number.");
                    }

                    return "";
                }
            },

            new MenuItem
            {
                Shortcut = "2",
                Title = "Set Grid Size",
                MenuItemAction = () =>
                {
                    if (TempConfig.BoardSize == 0)
                    {
                        Console.WriteLine("Please set the Board Size first.");
                        return "R";
                    }

                    Console.WriteLine(
                        $"Enter the grid size (minimum 3, and no larger than the board size {TempConfig.BoardSize}, e.g., 3 for a 3x3 grid): ");
                    if (int.TryParse(Console.ReadLine(), out int gridSize))
                    {
                        if (gridSize >= 3 && gridSize <= TempConfig.BoardSize)
                        {
                            TempConfig.GridSize = gridSize;
                            Console.WriteLine($"Grid size set to {gridSize}x{gridSize}.");
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please enter a valid grid size.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a valid number.");
                    }

                    return "";
                }
            },

            new MenuItem
            {
                Shortcut = "3",
                Title = "Set Win Condition",
                MenuItemAction = () =>
                {
                    if (TempConfig.GridSize == 0)
                    {
                        Console.WriteLine("Please set the Grid Size first.");
                        return "R";
                    }

                    Console.WriteLine(
                        $"Enter the win condition (minimum 3, and no larger than the grid size {TempConfig.GridSize}, e.g., 3 for 3 in a row): ");
                    if (int.TryParse(Console.ReadLine(), out int winCondition))
                    {
                        if (winCondition >= 3 && winCondition <= TempConfig.GridSize)
                        {
                            TempConfig.WinCondition = winCondition;
                            Console.WriteLine($"Win condition set to {winCondition}.");
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please enter a valid win condition.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a valid number.");
                    }

                    return "";
                }
            },

            new MenuItem
            {
                Shortcut = "4",
                Title = "Set Number of Pieces",
                MenuItemAction = () =>
                {
                    if (TempConfig.WinCondition == 0)
                    {
                        Console.WriteLine("Please set the Win Condition first.");
                        return "R";
                    }

                    Console.WriteLine("Enter the number of pieces to place on the board: ");
                    if (int.TryParse(Console.ReadLine(), out int numberOfPieces))
                    {
                        if (numberOfPieces > 0 && numberOfPieces >= TempConfig.WinCondition)
                        {
                            TempConfig.NumberOfPieces = numberOfPieces;
                            Console.WriteLine($"Number of pieces set to {numberOfPieces}.");
                        }
                        else
                        {
                            Console.WriteLine(
                                "Invalid input. Number of pieces should be greater than or equal to the win condition.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a valid number of pieces.");
                    }

                    return "";
                }
            },

            new MenuItem
            {
                Shortcut = "S",
                Title = "Save Configuration",
                MenuItemAction = () =>
                {
                    // Проверка на корректность заполнения всех настроек конфигурации
                    if (TempConfig.BoardSize == 0 || TempConfig.GridSize == 0 || TempConfig.WinCondition == 0 ||
                        TempConfig.NumberOfPieces == 0)
                    {
                        Console.WriteLine("Please complete all configuration settings before saving.");
                        return "R";
                    }

                    // Ввод имени конфигурации
                    Console.WriteLine("Enter a name for the new configuration: ");
                    var configName = Console.ReadLine();
                    TempConfig.Name = configName ?? "Unnamed";

                    // Создание новой конфигурации
                    var newConfig = new GameConfiguration
                    {
                        Name = TempConfig.Name,
                        BoardSize = TempConfig.BoardSize,
                        GridSize = TempConfig.GridSize,
                        GridStartX = TempConfig.GridStartX,
                        GridStartY = TempConfig.GridStartY,
                        WinCondition = TempConfig.WinCondition,
                        NumberOfPieces = TempConfig.NumberOfPieces
                    };

                    // Использование репозитория для сохранения конфигурации в JSON файл
                    var configRepository = new ConfigRepositoryJson();
                    var optionJsonStr = System.Text.Json.JsonSerializer.Serialize(newConfig);
                    System.IO.File.WriteAllText(FileHelper.BasePath + newConfig.Name + FileHelper.ConfigExtension, optionJsonStr);

                    Console.WriteLine($"Configuration '{TempConfig.Name}' saved.");
                    return "R";
                }
            }
        }
    );

    private static readonly Menu OptionsMenu =
        new Menu(
            EMenuLevel.Secondary,
            "TIC-TAC-TOE Options",
            new List<MenuItem>
            {
                new MenuItem
                {
                    Shortcut = "X",
                    Title = "X Starts",
                    MenuItemAction = () =>
                    {
                        FirstMoveBy = EGamePiece.X;
                        Console.WriteLine("X will start the game!"); 
                        return "M"; 
                    }
                },
                new MenuItem
                {
                    Shortcut = "O",
                    Title = "O Starts",
                    MenuItemAction = () =>
                    {
                        FirstMoveBy = EGamePiece.O;
                        Console.WriteLine("O will start the game!"); 
                        return "M"; 
                    }
                },
                new MenuItem
                {
                    Shortcut = "C",
                    Title = "New Configuration",
                    MenuItemAction = CreateConfigMenu.Run
                }
            });

    private static readonly Menu LoadGameMenu = new Menu(
        EMenuLevel.Secondary,
        "Load Saved Game",
        new List<MenuItem>(), 
        isCustomMenu: true
    );


    private static void PopulateLoadGameMenu()
    {
        var gameRepo = new GameRepositoryJson();
        var savedGames = gameRepo.GetSavedGames(); 
        LoadGameMenu.MenuItems.Clear(); 

        if (savedGames.Count > 0)
        {
            for (int i = 0; i < savedGames.Count; i++)
            {
                var game = savedGames[i]; 
                LoadGameMenu.MenuItems.Add(new MenuItem
                {
                    Shortcut = (i + 1).ToString(), 
                    Title = game,
                    MenuItemAction = () =>
                    {
                        Console.WriteLine($"Loading game: {game}");
                    
                        var gameFilePath = $"{FileHelper.BasePath}{game}.json"; 

                        var jsonStateString = File.ReadAllText(gameFilePath);
                        var gameState = System.Text.Json.JsonSerializer.Deserialize<GameState>(jsonStateString);

                        if (gameState != null)
                        {
                            Console.WriteLine("Loaded GameBoard:");
                            foreach (var row in gameState.GameBoard!)
                            {
                                Console.WriteLine(string.Join(" | ", row.Select(piece => piece.ToString())));
                            }

                            var gameBrain = new TicTacTwoBrain(gameState.GameConfiguration);
                            gameBrain._gameState = gameState;
                            GameController.LoadExistingGame(gameBrain, gameFilePath);
                        }

                        return ""; 
                    }
                });
            }
        }
        LoadGameMenu.MenuItems.Add(new MenuItem
        {
            Shortcut = "X",
            Title = "No saved games available or returned to main menu",
            MenuItemAction = () => { return "R"; } 
        });
    }
    
    public static Menu MainMenu = new Menu(
        EMenuLevel.Main,
        "TIC-TAC-TOE",
        new List<MenuItem>
        {
            new MenuItem
            {
                Shortcut = "O",
                Title = "Options",
                MenuItemAction = OptionsMenu.Run
            },
            new MenuItem
            {
                Shortcut = "N",
                Title = "New game",
                MenuItemAction = () => GameController.MainLoop() 
            },
            new MenuItem
            {
                Shortcut = "L",
                Title = "Load Game",
                MenuItemAction = () =>
                {
                    Menus.PopulateLoadGameMenu(); 
                    return Menus.LoadGameMenu.Run(); 
                }
            }
        });


    private static class TempConfig
    {
        public static string Name { get; set; } = "";
        public static int BoardSize { get; set; }= 5;
        public static int GridSize { get; set; } = 0;
        public static int GridStartX { get; set; } = 1; // praegu muuta ei saa
        public static int GridStartY { get; set; } = 1; // praegu muuta ei saa
        public static int WinCondition { get; set; } = 0;
        public static int NumberOfPieces { get; set; } = 0;
    }
}