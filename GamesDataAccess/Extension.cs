using System.Data.Common;

public static class Extension
{
    public static void AddParameterWithValue
    (
        this DbCommand command,
        string paramName, 
        object value
    )
    {
        var parameter = command.CreateParameter();
        parameter.ParameterName = paramName;
        parameter.Value = value;

        command.Parameters.Add(parameter);
    }
}
