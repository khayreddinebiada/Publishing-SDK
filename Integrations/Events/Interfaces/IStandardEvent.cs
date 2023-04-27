namespace Apps
{
    public interface IStandardEvent : IEvent
    {
        /// <summary>
        /// To send a custom event.
        /// </summary>
        /// <param name="eventName"> The event name that we will send. </param>
        void CustomEvent(string eventName);

        /// <summary>
        /// To send a session event.
        /// </summary>
        /// <param name="sessionName"> The session name that we will send. </param>
        /// <param name="statue"> The session statue that we will send it can be started or completed. </param>
        void SessionEvent(string sessionName, SessionStatue statue);

        /// <summary>
        /// To send some error events.
        /// </summary>
        /// <param name="severity"> The error type. </param>
        /// <param name="message"> The message content in the error. </param>
        void ErrorEvent(ErrorSeverity severity, string message);
    }
}