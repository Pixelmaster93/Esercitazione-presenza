﻿using System.Data.SQLite;
using GamesDataAccess;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

//dove creo i dati, o docv eli vado a prendere
string dbFile = @"..\..\Data\test.db";
string dbFilePath = Path.GetDirectoryName(dbFile)!;
if (!Directory.Exists(dbFilePath))
{
    Directory.CreateDirectory(dbFilePath);
}

string connStr = $@"Data source={dbFile}; Version=3;";

GamesDao gamesDao = 
    new GamesDao
    (
        () => new SQLiteConnection(connStr),
        "||"
    );

//vado a cancellare tutte le tabelle
gamesDao.DropAllTables();

//Vado a creare le tabelle
gamesDao.CreateAllTables();

DataPopulator dataPopular = new DataPopulator(gamesDao);
dataPopular.AddSomeGames();
dataPopular.AddSomeStores();
dataPopular.AddSomePlatforms();
dataPopular.AddSomeTransactions();

/*
//Console.WriteLine($"Added {plat} game(s)");

//Console.Write("Vuoi vedere tutti i giochi sul DB?(SI/NO) ");
//string reponse = Console.ReadLine().ToString().ToUpper();

//if (reponse == "SI")
//{
//    var allGames = gamesDal.GetAllGames();
//    foreach (var game in allGames)
//        Console.WriteLine(game);
//}
*/
GameDbItem[] games = gamesDao.GetAllGames();
foreach (var game in games)
{
    Console.WriteLine(game);
}

StoreDbItem[] stores = gamesDao.GetAllStores();

foreach (var store in stores)
{
    Console.WriteLine(store);
}

PlatformDbItem[] platforms = gamesDao.GetAllPlatforms();

foreach (var platform in platforms)
{
    Console.WriteLine(platform);
}

TransactionDbItem[] transactions = gamesDao.GetAllTransactions();

foreach (var tarnsaction in transactions)
{
    Console.WriteLine(tarnsaction);
}
/*
//Console.WriteLine("Inserire nome del gioco!");
//string partialname = Console.ReadLine();
//Console.WriteLine("Inserire il tag!");
//string tags = Console.ReadLine();

//var gamespart = gamesDao.GetGamesByPartialName(null, null);
//foreach (var game in games)
//    Console.WriteLine(game);



//Console.WriteLine("Cancello le tabelle!");
//File.Delete("..\\..\\Data\\test.db");
*/




