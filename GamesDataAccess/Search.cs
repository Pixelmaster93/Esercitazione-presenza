using GamesDataAccess.Criteria;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesDataAccess
{
    partial class GamesDao
    {
        public OwnedGameDbItem[] GetAllOwnedGames() =>
        GetOwnedGamesByCriteria(new GamesCriteria { });

        public OwnedGameDbItem[] GetOwnedGamesByCriteria
            (GamesCriteria criteria)
        {
            string selectText = $@"
        select 
            GT.transaction_id,
            GT.purchase_date,
            GT.is_virtual,
            S.store_id,
            S.store_name,
            S.store_description,
            P.platform_id,
            P.platform_name,
            P.platform_description,
            G.game_id,
            G.game_name,
            G.game_description,
            G.game_tags,
            GT.price

        from games_transactions GT
        inner join stores S on GT.store_id = S.store_id
        inner join platforms P on GT.platform_id = P.platform_id
        inner join games G on GT.game_id = G.game_id

        where 1 = 1 ";

            if (criteria?.PurchaseDateFrom is not null)
            {
                selectText +=
                $@" and GT.purchase_date >= :purchasedatefrom";
            }

            if (criteria?.PurchaseDateTo is not null)
            {
                selectText +=
                $@" and GT.purchase_date <= :purchasedateto";
            }

            if (criteria?.IsVirtual is not null)
            {
                selectText +=
                $@" and GT.is_virtual = :isVirtual";
            }

            if (criteria?.StoreName is not null)
            {
                selectText +=
                $@" and S.store_name like '%' {_strConcatOperator} :partialstorename {_strConcatOperator} '%'";
            }

            if (criteria?.StoreDescription is not null)
            {
                selectText +=
                $@" and S.store_description like '%' {_strConcatOperator} :partialstoredesc {_strConcatOperator} '%'";
            }

            if (criteria?.PlatformName is not null)
            {
                selectText +=
                $@" and S.platform_name like '%' {_strConcatOperator} :partialplatname {_strConcatOperator} '%'";
            }

            if (criteria?.PlatformDescription is not null)
            {
                selectText +=
                $@" and S.platform_description like '%' {_strConcatOperator} :partialplatdesc {_strConcatOperator} '%'";
            }

            if (criteria?.GameName is not null)
            {
                selectText +=
                $@" and G.game_name like '%' {_strConcatOperator} :partialgamename {_strConcatOperator} '%'";
            }

            if (criteria?.GameDescription is not null)
            {
                selectText +=
                $@" and G.game_description like '%' {_strConcatOperator} :partialgamedesc {_strConcatOperator} '%'";
            }

            if (criteria?.GameTags is not null)
            {
                selectText +=
                $@" and G.game_tags like '%' {_strConcatOperator} :partialgametags {_strConcatOperator} '%'";
            }

            if (criteria?.PriceFrom is not null)
            {
                selectText +=
                $@" and GT.price >= :pricefrom";
            }

            if (criteria?.PriceTo is not null)
            {
                selectText +=
                $@" and GT.price <= :priceto";
            }

            Action<DbCommand> addParametersAction =
                cmd =>
                {
                    if (criteria?.PurchaseDateFrom is not null)
                    {
                        cmd.AddParameterWithValue("purchasedatefrom", criteria.PurchaseDateFrom.Value, DbType.DateTime);
                    }

                    if (criteria?.PurchaseDateTo is not null)
                    {
                        cmd.AddParameterWithValue("purchasedateto", criteria.PurchaseDateTo.Value, DbType.DateTime);
                    }

                    if (criteria?.IsVirtual is not null)
                    {
                        cmd.AddParameterWithValue("isVirtual", criteria.IsVirtual.Value, DbType.Boolean);
                    }

                    if (criteria?.StoreName is not null)
                    {
                        cmd.AddParameterWithValue("partialstorename", criteria.StoreName);
                    }

                    if (criteria?.StoreDescription is not null)
                    {
                        cmd.AddParameterWithValue("partialstoredesc", criteria.StoreDescription);
                    }

                    if (criteria?.PlatformName is not null)
                    {
                        cmd.AddParameterWithValue("partialplatname", criteria.PlatformName);
                    }

                    if (criteria?.PlatformDescription is not null)
                    {
                        cmd.AddParameterWithValue("partialplatdesc", criteria.PlatformDescription);
                    }

                    if (criteria?.GameName is not null)
                    {
                        cmd.AddParameterWithValue("partialgamename", criteria.GameName);
                    }

                    if (criteria?.GameDescription is not null)
                    {
                        cmd.AddParameterWithValue("partialgamedesc", criteria.GameDescription);
                    }

                    if (criteria?.GameTags is not null)
                    {
                        cmd.AddParameterWithValue("partialgametags", criteria.GameTags);
                    }

                    if (criteria?.PriceFrom is not null)
                    {
                        cmd.AddParameterWithValue("pricefrom", criteria.PriceFrom.Value, DbType.Decimal);
                    }

                    if (criteria?.PriceTo is not null)
                    {
                        cmd.AddParameterWithValue("priceto", criteria.PriceTo.Value, DbType.Decimal);
                    }

                };

            Func<DbDataReader, OwnedGameDbItem> mapper =
                dataReader =>
                {
                    string id = dataReader.GetString(0);
                    var date = dataReader.GetDateTime(1);
                    bool isVirtual = dataReader.GetBoolean(2);
                    string storeId = dataReader.GetString(3);
                    string storeName = dataReader.GetString(4);
                    string storeDescription = dataReader.GetString(5);
                    string platformId = dataReader.GetString(6);
                    string platformName = dataReader.GetString(7);
                    string platformDescription = dataReader.GetString(8);
                    string gameId = dataReader.GetString(9);
                    string gameName = dataReader.GetString(10);
                    string gameDescription = dataReader.GetString(11);
                    string gameTags = dataReader.GetString(12);
                    decimal price = dataReader.GetDecimal(13);

                    OwnedGameDbItem ownedGame = 
                        new OwnedGameDbItem
                        (
                            id, 
                            date, 
                            isVirtual, 
                            storeId,
                            storeName, 
                            storeDescription, 
                            platformId,
                            platformName,
                            platformDescription,
                            gameId,
                            gameName, 
                            gameDescription, 
                            gameTags,
                            price

                        );
                    return ownedGame;
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
