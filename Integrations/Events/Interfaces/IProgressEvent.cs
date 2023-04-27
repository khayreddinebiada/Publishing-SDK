namespace Apps
{
    public interface IProgressEvent : IEvent
    {
        /// <summary>
        /// To send a ProgressEvent event.
        /// </summary>
        void ProgressStartedEvent(ProgressStartInfo progressInfo);

        /// <summary>
        /// To send a ProgressEvent event.
        /// </summary>
        void ProgressFailedEvent(ProgressFailedInfo progressInfo);

        /// <summary>
        /// To send a ProgressEvent event.
        /// </summary>
        void ProgressCompletedEvent(ProgressCompletedInfo progressInfo);

    }
}