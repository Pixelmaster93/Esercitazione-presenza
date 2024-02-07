using GamesDataAccess;
class DataPopulator
{
    private GamesDao _gamesDao;
    public DataPopulator(GamesDao gamesDao)
    {
        _gamesDao = gamesDao;
    }

    public int AddSomeData()
    {
        int affected = AddSomeGames();
        affected += AddSomeStores();
        affected += AddSomePlatforms();
        affected += AddSomeTransactions();

        return affected;
    }

    public int AddSomeGames()
    {
        int affected = 0;
        foreach (var game in GetGames())
        {
            affected += _gamesDao.AddNewGame(game);
        }
        return affected;
    }
    private IEnumerable<GameDbItem> GetGames()
    {
        yield return
            new GameDbItem
            (
                "elden-ring",
                "Elden Ring",
                "GOTY 2022",
                "soulslike;gdr;adventure"
            );

        yield return
            new GameDbItem
            (
                "zelda-botw",
                "Zelda Breath of the Wild",
                "The Legend of Zelda: Breath of the Wild",
                "zelda;gdr;nintendo;adventure"
            );

        yield return
            new GameDbItem
            (
                "super-mario-wonder",
                "Super Mario Wonder",
                "Super Mario Wonder",
                "mario;platform;nintendo"
            );

        yield return
            new GameDbItem
            (
                "palworld",
                "Palworld",
                "Pokemon clone",
                "pokemon;survival"
            );

        yield return
            new GameDbItem
            (
                "alan-wake-2",
                "Alan Wake 2",
                "Alan Wake 2",
                "adventure;survival;horror"
            );
    }


    public int AddSomeStores()
    {
        int affected = 0;
        foreach (var store in GetStores())
        {
            affected += _gamesDao.AddNewStore(store);
        }
        return affected;
    }
    private IEnumerable<StoreDbItem> GetStores()
    {
        yield return
            new StoreDbItem
            (
                "ps-store",
                "Playstation Store",
                "Store ufficale Sony",
                "https://store.playstation.com/it-it/pages/latest"
            );

        yield return
            new StoreDbItem
            (
                "steam",
                "Steam Store",
                "Store ufficiale Steam",
                "https://store.steampowered.com"
            );

        yield return
            new StoreDbItem
            (
                "ns-store",
                "Nintendo Store",
                "Store ufficiale Nintendo",
                "https://store.nintendo.it"
            );

        yield return
           new StoreDbItem
           (
               "instantstore",
               "Instant Gamin",
               "Le migliori offert sui videogiochi",
               "https://www.instant-gaming.com"
           );

        yield return
           new StoreDbItem
           (
               "gamestopfisic",
               "Game Stop",
               "Non danno mai il centesimo di resto",
               "https://www.gamestop.it"
           );

    }
    

    public int AddSomePlatforms()
    {
        int affected = 0;
        foreach (var platform in GetPlatforms())
        {
            affected += _gamesDao.AddNewPlatform(platform);
        }
        return affected;
    }
    private IEnumerable<PlatformDbItem> GetPlatforms()
    {
        yield return
            new PlatformDbItem
            (
                "ps5",
                "Playstation 5",
                "Console grossa"
            );

        yield return
            new PlatformDbItem
            (
                "nswt",
                "Nintendo Switch",
                "E' a metà del suo ciclo vitale"
            );

        yield return
            new PlatformDbItem
            (
                "mxs",
                "X-Box S",
                "E' verde"
            );

    }


    public int AddSomeTransactions()
    {
        int affected = 0;
        foreach (var gameTx in GetTransactions())
        {
            affected += _gamesDao.AddNewTransaction(gameTx);
        }
        return affected;
    }
    private IEnumerable<TransactionDbItem> GetTransactions()
    {
        yield return
            new TransactionDbItem
            (
                "TX0001",
                new DateTime(2024,01,18),
                false,
                "ns-store",
                "super-mario-wonder",
                "nswt",
                80,
                ""
            );

        yield return
            new TransactionDbItem
            (
                "TX0002",
                new DateTime(2022, 05, 18),
                true,
                "ps5",
                "elden-ring",
                "instantstore",
                45,
                ""
            );

        yield return
           new TransactionDbItem
           (
               "TX0003",
               new DateTime(2017, 03, 03),
               true,
               "ns-store",
               "zelda-botw",
               "nswt",
               68,
               ""
           );

        yield return
           new TransactionDbItem
           (
               "TX0004",
               new DateTime(2024, 02, 07),
               false,
               "gamestopfisic",
               "alan-wake-2",
               "ps5",
               59,
               "Via San Quirico, 165, 50013 Campi Bisenzio FI"
           );


    }
}


