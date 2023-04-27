namespace Apps
{
    public interface IImpressionEvent : IEvent
    {
        void AdImpressionEvent(EventADSName eventADSName, AdType adType, string placement, EventADSResult result);
    }
}