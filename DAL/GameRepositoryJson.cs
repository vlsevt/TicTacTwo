using System.Text.RegularExpressions;
using GameBrain;

namespace DAL;
public class GameRepositoryJson : IGameRepository
{
    public void SaveGame(string jsonStateString, string gameConfigName)
    {
        string sanitizedConfigName = Regex.Replace(gameConfigName, @"[<>:""/\\|?*]", "-");

        string timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH-mm-ss");

        string fileName = $"{sanitizedConfigName} {timestamp}.game.json";

        string filePath = Path.Combine(@"C:\Users\user\RiderProjects\icd0008-24f", fileName);

        File.WriteAllText(filePath, jsonStateString);
    }
    
    public List<string> GetSavedGames()
    {
        Console.Write(FileHelper.BasePath, "*" + FileHelper.GameExtension);
        return Directory
            .GetFiles(FileHelper.BasePath, "*" + FileHelper.GameExtension)
            .Select(fullFileName =>
                Path.GetFileNameWithoutExtension(fullFileName))
            .ToList();
    }
}