using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesDataAccess
{
    partial class GamesDao
    {
        public void CreateTableTransaction()
        {
            string createTransactionsStr = $@"
            create table games_transactions 
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

        public void DropTableTransactions()
        {
            string dropTableText = $@"drop table games_transactions";
            ExecuteNonQuery(dropTableText);
        }

        public int AddNewTransaction(TransactionDbItem gameTx)
        {
            string cmdText = $@"

             insert into games_transactions 
            (
                transaction_id,
                purchase_date,
                is_virtual,
                store_id,
                game_id,
                platform_id,
                price,
                notes

            )

            values
            (
                :transaction_id,
                :purchase_date,
                :is_virtual,
                :store_id,
                :game_id,
                :platform_id,
                :price,
                :notes
                
            )
            ";

            Action<DbCommand> action =
                cmd =>
                {
                    //vado a verificare i parametri
                    cmd.AddParameterWithValue("transaction_id", gameTx.TransactionId);
                    cmd.AddParameterWithValue("purchase_date", gameTx.PurchaseDate, DbType.DateTime);
                    cmd.AddParameterWithValue("is_virtual", gameTx.IsVirtual, DbType.Boolean);
                    cmd.AddParameterWithValue("store_id", gameTx.StoreId);
                    cmd.AddParameterWithValue("game_id", gameTx.GameId);
                    cmd.AddParameterWithValue("platform_id", gameTx.PlatformId);
                    cmd.AddParameterWithValue("price", gameTx.Price);
                    cmd.AddParameterWithValue("notes", gameTx.Notes);
                };

            return ExecuteNonQuery(cmdText, action);
        }

        public TransactionDbItem[] GetAllTransactions() =>
       GetTransactionsById(null);

        public TransactionDbItem[] GetTransactionsById(string? partialName)
        {
            string selectText = $@"
        select 
            transaction_id,
            purchase_date,
            is_virtual,
            store_id,
            game_id,
            platform_id,
            price,
            notes

        from games_transactions
        where 1 = 1 ";

            if (partialName is not null)
            {
                selectText +=
                $@"and transaction_id = :partialname";
            }

            Action<DbCommand> addParametersAction =
                cmd =>
                {
                    if (partialName is not null)
                    {
                        cmd.AddParameterWithValue("partialname", partialName);
                    }
                };

            Func<DbDataReader, TransactionDbItem> mapper =
                dataReader =>
                {
                    string id = dataReader.GetString(0);
                    var date = dataReader.GetDateTime(1);
                    bool isVirtual = dataReader.GetBoolean(2);
                    string storeId = dataReader.GetString(3);
                    string gameId = dataReader.GetString(4);
                    string platformId = dataReader.GetString(5);
                    decimal price = dataReader.GetDecimal(6);
                    string notes = dataReader.GetString(7);

                    TransactionDbItem transaction = new TransactionDbItem(id, date, isVirtual, storeId, gameId, platformId, price, notes);
                    return transaction;
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
