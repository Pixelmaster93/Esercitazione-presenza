using System.Data.SQLite;

class GameDal
{
    public string ConnectionString { get; }
    public GameDal( string connectionString)
    {
        ConnectionString = connectionString;
    }

    private void OpenAndExecute(Action<SQLiteConnection> action)
    {
        using SQLiteConnection conn =
            new SQLiteConnection(ConnectionString);
        conn.Open();
        action(conn);
    }

    public void CreateTableGames()
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

        Action<SQLiteConnection> action =
            conn =>
            {
                //L' if not exist serve a non far bloccare il programma se esiste già la tabella
                string createGamesStr = $@"
            create table if not exists games 
            (
                game_id varchar(20) primary key,
                game_name varchar(255),
                game_description varchar(1024),
                game_tags varchar(5000)
            )
            ";

                //crea un comando da mandare al DB
                using SQLiteCommand cmd = conn.CreateCommand();
                cmd.CommandText = createGamesStr;
                cmd.CommandType = System.Data.CommandType.Text;

                //numero di righe coinvolte
                cmd.ExecuteNonQuery();
            };

        OpenAndExecute(action);
        
    }

    public int AddNewGame(Game game)
    {
        //using SQLiteConnection conn =
        //    new SQLiteConnection(ConnectionString);
        //conn.Open();

        int affected = 0;

        Action<SQLiteConnection> action =
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

                using SQLiteCommand cmd = conn.CreateCommand();
                cmd.CommandText = createGamesStr;
                cmd.CommandType = System.Data.CommandType.Text;

                //vado a verificare i parametri
                cmd.Parameters.AddWithValue("game_id", game.GameId);
                cmd.Parameters.AddWithValue("game_name", game.GameName);
                cmd.Parameters.AddWithValue("game_description", game.GameDescription);
                cmd.Parameters.AddWithValue("game_tags", game.GameTags);


                affected = cmd.ExecuteNonQuery();
            };

        OpenAndExecute(action);

        return affected;
    }

    public Game[] GetAllGames() =>
        GetGamesByPartialName(null, null);

    public Game[] GetGamesByPartialName(string? partialName, string? partialTags)
    {

        //using SQLiteConnection conn =
        //    new SQLiteConnection(ConnectionString);
        //conn.Open();
        List<Game> games = new List<Game>();

        Action<SQLiteConnection> action =
            conn =>
            {

                string selectText = $@"
                    select 
                        game_id,
                        game_name,
                        game_description,
                        game_tags
                    from games
                    where 1 = 1";
                
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

                using SQLiteCommand cmd = conn.CreateCommand();
                cmd.CommandText = selectText;
                cmd.CommandType = System.Data.CommandType.Text;

                if (selectText is not null)
                {
                    cmd.Parameters.AddWithValue("partialname", partialName);
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
