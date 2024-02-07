public record OwnedGameDbItem
(
    string TransactionId,
    DateTime PurchaseDate,
    bool IsVirtual,
    string StoreId,
    string StoreName,
    string StoreDescription,
    string PlatformId,
    string PlatformName,
    string PlatformDescription,
    string GameId,
    string GameName,
    string GameDescription,
    string GameTags,
    decimal Price
);

