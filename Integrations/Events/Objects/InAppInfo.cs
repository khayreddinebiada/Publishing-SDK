namespace Apps
{
    public struct InAppInfo
    {
        public InAppInfo(string inAppID, string currency, double price, string inAppType)
        {
            InAppID = inAppID;
            Currency = currency;
            Price = price;
            InAppType = inAppType;

            Quantity = "1";
            Receipt = "Null";
            SignatureAndTransactionID = "Null";
        }

        public InAppInfo(string inAppID, string currency, string quantity, double price, string inAppType, string receipt, string signatureAndTransactionID)
        {
            InAppID = inAppID;
            Currency = currency;
            Quantity = quantity;
            Price = price;
            InAppType = inAppType;
            Receipt = receipt;
            SignatureAndTransactionID = signatureAndTransactionID;
        }

        public string InAppID;
        public string Currency;
        public string Quantity;
        public double Price;
        public string InAppType;
        public string Receipt;
        public string SignatureAndTransactionID;
    }
}