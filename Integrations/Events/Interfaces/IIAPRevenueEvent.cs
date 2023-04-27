namespace Apps
{
    public interface IIAPRevenueEvent : IEvent
    {
        /// <summary>
        /// To send some products buying in the game.
        /// </summary>
        /// <param name="productIAPID"> product ID of the IAP. </param>
        /// <param name="price"> The price that spended on this product. </param>
        void IAPEvent(InAppInfo info);
    }
}