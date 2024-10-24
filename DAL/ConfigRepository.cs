using GameBrain;

namespace DAL;

public class ConfigRepository
{
    private static List<GameConfiguration> _gameConfigurations = new List<GameConfiguration>()
    {
        new GameConfiguration("Classical"),

        new GameConfiguration("Big board", 10, 4, 1, 1, 4, 4)
    };

    public List<string> GetConfigurationNames()
    {
        return _gameConfigurations
            .OrderBy(x => x.Name)
            .Select(config => config.Name)
            .ToList();
    }

    public GameConfiguration GetConfigurationByName(string name)
    {
        return _gameConfigurations.Single(c => c.Name == name);
    }
    public static void AddConfiguration(GameConfiguration config)
    {
        _gameConfigurations.Add(config);
        Console.WriteLine($"Configuration '{config.Name}' has been added.");
    }
}