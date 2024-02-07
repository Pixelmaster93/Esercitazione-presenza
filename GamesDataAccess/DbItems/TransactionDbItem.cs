public record TransactionDbItem
(
    string TransactionId,
    DateTime PurchaseDate,
    bool IsVirtual,
    string StoreId,
    string GameId,
    string PlatformId,
    decimal Price,
    string Notes
);

