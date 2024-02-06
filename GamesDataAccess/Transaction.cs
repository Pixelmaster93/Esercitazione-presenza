public record Transaction // record è una forma di classe equivale a scrivere una classe con le property e costruttore
(
    string TransactionId,
    string PurchaseDate,
    string IsVirtual,
    string StoreId,
    string GameId,
    string PlatformId,
    string Price,
    string Currency
);
