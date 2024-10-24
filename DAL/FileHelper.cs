﻿namespace DAL;

public static class FileHelper
{
    public static string BasePath = Environment
                                        .GetFolderPath(System.Environment.SpecialFolder.UserProfile)
                                    + Path.DirectorySeparatorChar + "RiderProjects/" + "icd0008-24f" + Path.DirectorySeparatorChar;


    public static string ConfigExtension = ".config.json";
    public static string GameExtension = ".game.json";
}