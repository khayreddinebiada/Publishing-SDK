using System.Threading.Tasks;
using Firebase.RemoteConfig;
using Firebase.Extensions;
using Firebase;
using System;

namespace Apps.Firebase
{
    public class FirebaseServices
    {
        private Action _onInitialized;
        private Action _onConfigSuccess;
        private Action<FetchFailureReason> _onConfigFailed;

        public FirebaseServices(Action onInitialized, Action onConfigSuccess, Action<FetchFailureReason> onConfigFailed)
        {
            _onInitialized = onInitialized;
            _onConfigSuccess = onConfigSuccess;
            _onConfigFailed = onConfigFailed;

            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(Continuation);
        }

        private void Continuation(Task<DependencyStatus> task)
        {
            DependencyStatus dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
                InitializeFirebase();
            else
                Debuger.Error(typeof(FirebaseServices), "Could not resolve all Firebase dependencies: " + dependencyStatus);
        }

        private void InitializeFirebase()
        {
            _onInitialized?.Invoke();

            Debuger.Log(typeof(FirebaseServices), "Firebase is ready!");
            Debuger.Log(typeof(FirebaseServices), "Fetching data...");
            Task fetchTask = FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
            fetchTask.ContinueWithOnMainThread(FetchComplete);
        }

        private void FetchComplete(Task fetchTask)
        {
            if (fetchTask.IsCanceled)
                Debuger.Log(typeof(FirebaseServices), "Fetch canceled.");
            else if (fetchTask.IsFaulted)
                Debuger.Log(typeof(FirebaseServices), "Fetch encountered an error.");
            else if (fetchTask.IsCompleted)
                Debuger.Log(typeof(FirebaseServices), "Fetch completed successfully!");

            ConfigInfo info = FirebaseRemoteConfig.DefaultInstance.Info;
            switch (info.LastFetchStatus)
            {
                case LastFetchStatus.Success:
                    LastFetchStatusSuccess(info);
                    break;
                case LastFetchStatus.Failure:
                    LastFetchStatusFailure(info);
                    break;
                case LastFetchStatus.Pending:
                    LastFetchStatusPending();
                    break;
            }
        }

        private void LastFetchStatusSuccess(ConfigInfo info)
        {
            FirebaseRemoteConfig.DefaultInstance.ActivateAsync().ContinueWithOnMainThread(task =>
                Debuger.Log(typeof(FirebaseServices), $"Remote data loaded and ready (last fetch time {info.FetchTime})."));

            _onConfigSuccess?.Invoke();
        }

        private void LastFetchStatusFailure(ConfigInfo info)
        {
            switch (info.LastFetchFailureReason)
            {
                case FetchFailureReason.Error:
                    Debuger.Error(typeof(FirebaseServices), "Fetch failed for unknown reason");
                    break;
                case FetchFailureReason.Throttled:
                    Debuger.Error(typeof(FirebaseServices), "Fetch throttled until " + info.ThrottledEndTime);
                    break;
            }

            _onConfigFailed?.Invoke(FetchFailureReason.Throttled);
        }

        private void LastFetchStatusPending() =>
            Debuger.Log(typeof(FirebaseServices), "Latest Fetch call still pending.");
    }
}