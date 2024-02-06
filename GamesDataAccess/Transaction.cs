public record Transaction // record è una forma di classe equivale a scrivere una classe con le property e costruttore
(
    string TransactionId,
    string PurchaseDate,
    string GIsVirtual,
    string StoreId,
    string Game_Id,
    string Platform_Id,
    string Price,
    string Currency
);
