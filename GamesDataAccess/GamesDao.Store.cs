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
        public void CreateTableStore()
        {
            string createStoresStr = $@"
            create table stores 
            (
                store_id nvarchar(20) PRIMARY KEY,
		        store_name nvarchar(100) UNIQUE,
		        store_description nvarchar(255),
		        store_link nvarchar(100)
            )
            ";

            ExecuteNonQuery(createStoresStr);
        }

        public void DropTableStores()
        {
            string dropTableText = $@"drop table stores";
            ExecuteNonQuery(dropTableText);
        }

        public int AddNewStore(StoreDbItem store)
        {
            string cmdText = $@"

             insert into stores 
            (
                store_id,
                store_name,
                store_description,
                store_link
            )

            values
            (
                :store_id,
                :store_name,
                :store_description,
                :store_link
                
            )
            ";

            Action<DbCommand> action =
                cmd =>
                {
                    //vado a verificare i parametri
                    cmd.AddParameterWithValue("store_id", store.StoreId);
                    cmd.AddParameterWithValue("store_name", store.StoreName);
                    cmd.AddParameterWithValue("store_description", store.StoreDescription);
                    cmd.AddParameterWithValue("store_link", store.StoreLink);
                };

            return ExecuteNonQuery(cmdText, action);
        }

        public StoreDbItem[] GetAllStores() =>
        GetStoresByPartialName(null);

        public StoreDbItem[] GetStoresByPartialName(string? partialName)
        {
            string selectText = $@"
        select 
            store_id, 
            store_name, 
            store_description,
            store_link
        from stores
        where 1 = 1 ";

            if (partialName is not null)
            {
                selectText +=
                $@"and store_name like '%' {_strConcatOperator} :partialname {_strConcatOperator} '%'";
            }

            Action<DbCommand> addParametersAction =
                cmd =>
                {
                    if (partialName is not null)
                    {
                        cmd.AddParameterWithValue("partialname", partialName);
                    }
                };

            Func<DbDataReader, StoreDbItem> mapper =
                dataReader =>
                {
                    string id = dataReader.GetString(0);
                    string name = dataReader.GetString(1);
                    string description = dataReader.GetString(2);
                    string link = dataReader.GetString(3);

                    StoreDbItem store = new StoreDbItem(id, name, description, link);
                    return store;
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
