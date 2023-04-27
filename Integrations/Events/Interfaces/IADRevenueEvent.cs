namespace Apps
{
    public interface IADRevenueEvent : IEvent
    {
        /// <summary>
        /// To send the revenue of the ads.
        /// </summary>
        /// <param name="mediation"> The mediation name ex: Ironsource, Applovin... </param>
        /// <param name="impressionData"> The data of the impression </param>
        void ADRevenueEvent(object impressionData);
    }
}