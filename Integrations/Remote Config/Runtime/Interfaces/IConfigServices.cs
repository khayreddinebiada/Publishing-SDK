namespace Apps.RemoteConfig
{
    public interface IConfigServices
    {
        string TagConfig { get; }

        bool IsConfiguredSave { get; }

        /// <summary>
        /// True if the player can receive multiple configurations.
        /// False just one and stop update configurations.
        /// </summary>
        bool HasMultipleConfig { get; }

        bool IsReady { get; }

        event Delegates.OnConfigured OnConfigured;

        IConfigCollection Values { get; }
    }
}