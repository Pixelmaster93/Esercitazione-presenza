﻿using System.Data.SQLite;
using GamesDataAccess;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

//dove creo i dati, o docv eli vado a prendere
string connStr = @"Data Source=..\..\Data\test.db;Version=3;";

GameDal gamesDal = 
    new GameDal
    (
        () => new SQLiteConnection(connStr),
        "||"
    );

//Vado a creare le tabelle
gamesDal.CreateAllTables();


int affected =
    gamesDal
    .AddNewGame
        (
            new Game
            (
                "zelda-botw",
                "The Legend of Zelda Breath of the Wild",
                "The best Zelda of all time?",
                "zelda;nintendo;gdr;adevnture"
            )
        );

affected +=
gamesDal
.AddNewGame
    (
        new Game
        (
            "elden-ring",
            "Elden Ring",
            "GOTY 2022",
            "soulslike;gdr;adevnture"
        )
    );

Console.WriteLine($"Added {affected} game(s)");



var allGames = gamesDal.GetAllGames();
foreach (var game in allGames)
    Console.WriteLine(game);




var games = gamesDal.GetGamesByPartialName(null, null);
foreach (var game in games)
    Console.WriteLine(game);




gamesDal.DropAllTables();


