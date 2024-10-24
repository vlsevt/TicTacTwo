﻿using GameBrain;

public class GameState
{
    public EGamePiece[][]? GameBoard { get; set; }
    public EGamePiece NextMoveBy { get; set; } = EGamePiece.X;

    public GameConfiguration GameConfiguration { get; set; } 

    public GameState() { }

    public GameState(EGamePiece[][] gameBoard, GameConfiguration gameConfiguration)
    {
        GameBoard = gameBoard;
        GameConfiguration = gameConfiguration;
    }

    public override string ToString()
    {
        return System.Text.Json.JsonSerializer.Serialize(this);
    }
}