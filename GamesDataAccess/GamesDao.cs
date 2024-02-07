using System.Data;
using System.Data.Common;
using static System.Formats.Asn1.AsnWriter;
namespace GamesDataAccess;

public partial class GamesDao
{
    //public string ConnectionString { get; }

    private Func<DbConnection> _connectionFactory;
    private string _strConcatOperator;
    public GamesDao(Func<DbConnection> connectionFactory, string strConcatOperator)
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
    
    private void ExecuteNonQuery(string commandText) =>
        ExecuteNonQuery(commandText, null);
    
    public void CreateAllTables()
    {
        CreateTableGame();
        CreateTablePlatform();
        CreateTableStore();
        CreateTableTransaction();
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

    private int ExecuteNonQuery
        (
        string sqlText, 
        Action<DbCommand>? addParameterAction
        )
    {
        int affected = 0;

        Action<DbConnection> action =
            conn =>
            {
                string cmdText = sqlText;
                using DbCommand cmd = conn.CreateCommand();
                cmd.CommandText = cmdText;
                cmd.CommandType = System.Data.CommandType.Text;

                //vado a verificare i parametri
                addParameterAction?.Invoke(cmd);

                affected = cmd.ExecuteNonQuery();
            };

        OpenAndExecute(action);

        return affected;
    }

    private T[] GetItemsFromDb<T>
    (
        string selectText, 
        Action<DbCommand>? addParameterAction,
        Func<DbDataReader, T> mapper
    )
    {
        List<T> items = new List<T>();

        Action<DbConnection> action =
            conn =>
            {
                using DbCommand cmd = conn.CreateCommand();
                cmd.CommandText = selectText;
                cmd.CommandType = System.Data.CommandType.Text;

                addParameterAction?.Invoke(cmd);

                using var dataReader = cmd.ExecuteReader();
               
                while (dataReader.Read())
                {
                    T item = mapper(dataReader);
                    items.Add(item);
                }
            };

        OpenAndExecute(action);

        return items.ToArray();
    }
}
