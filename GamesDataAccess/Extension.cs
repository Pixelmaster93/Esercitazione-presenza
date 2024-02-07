using System.Data;
using System.Data.Common;

public static class Extension
{
    public static void AddParameterWithValue
    (
        this DbCommand command,
        string paramName,
        object value,
        DbType dbType = DbType.String
    )
    {
        var parameter = command.CreateParameter();
        parameter.ParameterName = paramName;
        parameter.Value = value;
        parameter.DbType = dbType;

        command.Parameters.Add(parameter);
    }

    public static void SafeExecute(this Action action)
    {
        try
        {
            action();
        }
        catch (Exception)
        { 

        }
    }
}
