using System.Data.SQLite;

//dove creo i dati, o docv eli vado a prendere
string connStr = @"Data Source=..\..\Data\test.db;Version=3;";

GameDal gamesDal = new GameDal(connStr);
gamesDal.CreateTableGames();

/*
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
*/

//var allGames = gamesDal.GetAllGames();
//foreach (var game in allGames)
//    Console.WriteLine(game);



var zeldaGames = gamesDal.GetGamesByPartialName("zel");
foreach (var game in zeldaGames)
    Console.WriteLine(game);


