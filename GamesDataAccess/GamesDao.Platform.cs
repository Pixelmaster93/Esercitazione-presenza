using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesDataAccess
{
    partial class GamesDao
    {
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

        public void DropTablePlatforms()
        {
            string dropTableText = $@"drop table platforms";
            ExecuteNonQuery(dropTableText);
        }

        public int AddNewPlatform(PlatformDbItem platform)
        {
            string cmdText = $@"

             insert into platforms 
            (
                platform_id,
                platform_name,
                platform_description
            )

            values
            (
                :platform_id,
                :platform_name,
                :platform_description
                
            )
            ";

            Action<DbCommand> action =
                cmd =>
                {
                    //vado a verificare i parametri
                    cmd.AddParameterWithValue("platform_id", platform.PlatformId);
                    cmd.AddParameterWithValue("platform_name", platform.PlatformName);
                    cmd.AddParameterWithValue("platform_description", platform.PlatformDescription);
                };

            return ExecuteNonQuery(cmdText, action);
        }

        public PlatformDbItem[] GetAllPlatforms() =>
       GetPlatformsByPartialName(null);

        public PlatformDbItem[] GetPlatformsByPartialName(string? partialName)
        {
            string selectText = $@"
        select 
            platform_id, 
            platform_name, 
            platform_description
        from platforms
        where 1 = 1 ";


            if (partialName is not null)
            {
                selectText +=
                $@"and platform_name like '%' {_strConcatOperator} :partialname {_strConcatOperator} '%'";
            }

            Action<DbCommand> addParametersAction =
                cmd =>
                {
                    if (partialName is not null)
                    {
                        cmd.AddParameterWithValue("partialname", partialName);
                    }
                };

            Func<DbDataReader, PlatformDbItem> mapper =
                dataReader =>
                {
                    string id = dataReader.GetString(0);
                    string name = dataReader.GetString(1);
                    string description = dataReader.GetString(2);

                    PlatformDbItem platform = new PlatformDbItem(id, name, description);
                    return platform;
                };

            return
                GetItemsFromDb
                (
                    selectText,
                    addParametersAction,
                    mapper
                );
        }
    }
}
