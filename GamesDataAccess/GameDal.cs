using System.Data.Common;
namespace GamesDataAccess;

public class GameDal
{
    //public string ConnectionString { get; }

    private Func<DbConnection> _connectionFactory;
    public GameDal(Func<DbConnection> connectionFactory)
    {
        _connectionFactory = connectionFactory;
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

        CreateTable(createGamesStr);
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

        CreateTable(createStoresStr);
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

        CreateTable(createPlatformsStr);
    }
    public void CreateTableTransaction()
    {
        string createTransactionsStr = $@"
            create table transactions 
            (
                transaction_id NVARCHAR(20) PRIMARY KEY,
		        purchase_date datetime2,
		        is_virtual tinyint,
		        store_id nvarchar (20) REFERENCES	[stores]([store_id]),
		        game_id nvarchar (20) REFERENCES	[videogames]([game_id]),
		        platform_id nvarchar (20) REFERENCES	[platforms]([platform_id]),
		        price numeric(6, 2) NULL,
		        currency char(3) DEFAULT 'EUR',
		        CHECK (price >= 0)
            )
            ";

        CreateTable(createTransactionsStr);
    }
    private void CreateTable(string table)
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
        string createStr = table;

        Action<DbConnection> action =
            conn =>
            {
                //L' if not exist serve a non far bloccare il programma se esiste già la tabella
             

                //crea un comando da mandare al DB
                using DbCommand cmd = conn.CreateCommand();
                cmd.CommandText = createStr;
                cmd.CommandType = System.Data.CommandType.Text;

                //numero di righe coinvolte
                cmd.ExecuteNonQuery();
            };

        OpenAndExecute(action);
        
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
                        $@"and game_name like '%' || :partialname || '%'";

                }
                if (partialTags is not null)
                {
                    selectText +=
                        $@"and game_tags like '%' || :partialtags || '%' ";
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
