using System.Data.Common;
namespace GamesDataAccess;

public class GameDal
{
    //public string ConnectionString { get; }

    private Func<DbConnection> _connectionFactory;
    private string _strConcatOperator;
    public GameDal(Func<DbConnection> connectionFactory, string strConcatOperator)
    {
        _connectionFactory = connectionFactory;
        _strConcatOperator = strConcatOperator;
    }

    private void OpenAndExecute(Action<DbConnection> action)
    {
        using DbConnection conn = _connectionFactory();
        conn.Open();
        action(conn);
    }
    public void CreateTableGame()
    {
        string createGamesStr = $@"
            create table games 
            (
                game_id varchar(20) primary key,
                game_name varchar(255),
                game_description varchar(1024),
                game_tags varchar(5000)
            )
            ";

        ExecuteNonQuery(createGamesStr);
    }
    public void CreateTableStore()
    {
        string createStoresStr = $@"
            create table stores 
            (
                store_id nvarchar(20) PRIMARY KEY,
		        store_name nvarchar(100) UNIQUE,
		        store_description nvarchar(255),
		        store_url nvarchar(100)
            )
            ";

        ExecuteNonQuery(createStoresStr);
    }
    public void CreateTablePlatform()
    {
        string createPlatformsStr = $@"
            create table platforms 
            (
                platform_id nvarchar(20) PRIMARY KEY,
		        platform_name nvarchar(100) UNIQUE,
		        platform_description nvarchar(255)
            )
            ";

        ExecuteNonQuery(createPlatformsStr);
    }
    public void CreateTableTransaction()
    {
        string createTransactionsStr = $@"
            create table game_transactions 
            (
                transaction_id NVARCHAR(20) not null PRIMARY KEY,
		        purchase_date datetime not null,
		        is_virtual int not null,
		        store_id nvarchar(20) not null REFERENCES	stores(store_id),
		        game_id nvarchar(20) not null REFERENCES	videogames(game_id),
		        platform_id nvarchar(20) not null REFERENCES	platforms(platform_id),
		        price decimal(10, 2) not NULL,
                notes clob null
		        CHECK (price >= 0)
            )
            ";

        ExecuteNonQuery(createTransactionsStr);
    }
    private void ExecuteNonQuery(string commandText)
    {
        /*
            //creo connessione
            using (SQLiteConnection conn = new SQLiteConnection(connStr))
            {
                //apro la connessione
                conn.Open();
            }
        */


        /*
               SQLiteConnection conn = new SQLiteConnection(connStr);
               conn.Open();
               conn.Close();
           */

        /*
               SQLiteConnection? conn = null;
               try
               {
                   conn = new SQLiteConnection(connStr);
                   conn.Open();
               }
               finally
               {
                   conn?.Dispose();
               }
           */

        //using SQLiteConnection conn =
        //    new SQLiteConnection(ConnectionString);
        //conn.Open();
        

        Action<DbConnection> action =
            conn =>
            {
                //L' if not exist serve a non far bloccare il programma se esiste già la tabella
             
                //crea un comando da mandare al DB
                using DbCommand cmd = conn.CreateCommand();
                cmd.CommandText = commandText;
                cmd.CommandType = System.Data.CommandType.Text;

                //numero di righe coinvolte
                cmd.ExecuteNonQuery();
            };

        OpenAndExecute(action);
        
    }
    public void CreateAllTables()
    {
        CreateTableGame();
        CreateTablePlatform();
        CreateTableStore();
        CreateTableTransaction();
    }
    public void DropTableGames()
    {
        string dropTableText = $@"drop table games";
        ExecuteNonQuery(dropTableText);
    }
    public void DropTableStores()
    {
        string dropTableText = $@"drop table stores";
        ExecuteNonQuery(dropTableText);
    }
    public void DropTablePlatforms()
    {
        string dropTableText = $@"drop table platforms";
        ExecuteNonQuery(dropTableText);
    }
    public void DropTableTransactions()
    {
        string dropTableText = $@"drop table game_transactions";
        ExecuteNonQuery(dropTableText);
    }
    public void DropAllTables()
    {
        Action action = DropTableTransactions;
        action.SafeExecute();

        action = DropTableGames;
        action.SafeExecute();

        action = DropTablePlatforms;
        action.SafeExecute();

        action = DropTableStores;
        action.SafeExecute();
    }

   
    public int AddNewGame(Game game)
    {
        int affected = 0;

        Action<DbConnection> action =
            conn =>
            {
                string createGamesStr = $@"

             insert into games 
            (
                game_id,
                game_name,
                game_description,
                game_tags
            )

            values
            (
                :game_id,
                :game_name,
                :game_description,
                :game_tags
                
            )
            ";

                using DbCommand cmd = conn.CreateCommand();
                cmd.CommandText = createGamesStr;
                cmd.CommandType = System.Data.CommandType.Text;

                //vado a verificare i parametri
                cmd.AddParameterWithValue("game_id", game.GameId);
                cmd.AddParameterWithValue("game_name", game.GameName);
                cmd.AddParameterWithValue("game_description", game.GameDescription);
                cmd.AddParameterWithValue("game_tags", game.GameTags);


                affected = cmd.ExecuteNonQuery();
            };

        OpenAndExecute(action);

        return affected;
    }

    public Game[] GetAllGames() =>
        GetGamesByPartialName(null, null);

    public Game[] GetGamesByPartialName(string? partialName, string? partialTags)
    {
        List<Game> games = new List<Game>();

        Action<DbConnection> action =
            conn =>
            {

                string selectText = $@"
                    select 
                        game_id,
                        game_name,
                        game_description,
                        game_tags
                    from games
                    where 1 = 1 ";
                
                if (partialName is not null)
                {
                    selectText +=
                        $@"and game_name like '%' {_strConcatOperator} :partialname {_strConcatOperator} '%'";

                }
                if (partialTags is not null)
                {
                    selectText +=
                        $@"and game_tags like '%' {_strConcatOperator} :partialtags {_strConcatOperator} '%' ";
                }

                using DbCommand cmd = conn.CreateCommand();
                cmd.CommandText = selectText;
                cmd.CommandType = System.Data.CommandType.Text;

                if (selectText is not null)
                {
                    cmd.AddParameterWithValue("partialname", partialName);
                }

                if (selectText is not null)
                {
                    cmd.AddParameterWithValue("partialtags", partialTags);
                }

                using var dataReader = cmd.ExecuteReader();


                while (dataReader.Read())
                {
                    string id = dataReader.GetString(0);
                    string name = dataReader.GetString(1);
                    string description = dataReader.GetString(2);
                    string tags = dataReader.GetString(3);

                    Game game = new Game(id, name, description, tags);
                    games.Add(game);
                }
            };

        OpenAndExecute(action);
            
        return games.ToArray();
    }
}
