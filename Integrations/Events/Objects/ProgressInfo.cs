using Apps.KPIs;
using System.Collections.Generic;

namespace Apps
{
    public class ProgressStartInfo
    {
        public int PlayerLevel;
        public string LevelName;
        public int LevelCount;
        public string Difficulty;
        public int LevelLoop;
        public bool IsRandom;
        public string LevelType;
        public string GameMode;

        public ProgressStartInfo(int playerLevel, string levelName, int levelCount, string difficulty, int levelLoop, bool isRandom, string levelType, string gameMode)
        {
            PlayerLevel = playerLevel;
            LevelName = levelName;
            LevelCount = levelCount;
            Difficulty = difficulty;
            LevelLoop = levelLoop;
            IsRandom = isRandom;
            LevelType = levelType;
            GameMode = gameMode;
        }

        public virtual Dictionary<string, object> ToDictionary()
        {
            Dictionary<string, object> keys = new Dictionary<string, object>();

            Dictionary<string, object> momenet = new Dictionary<string, object>();
            momenet.Add(PlayerLevel.ToString(), PlayTimeInfo.TimeRange);

            keys.Add("level_number", momenet);
            keys.Add("level_name", LevelName);
            keys.Add("level_count", LevelCount);
            keys.Add("level_diff", Difficulty);
            keys.Add("level_loop", LevelLoop);
            keys.Add("level_random", (IsRandom) ? 1 : 0);
            keys.Add("level_type", LevelType);
            keys.Add("game_mode", GameMode);

            return keys;
        }
    }

    public class ProgressFailedInfo : ProgressStartInfo
    {
        public string Time;
        public string Reason;
        public int Progress;
        public int ContinueValue;

        public ProgressFailedInfo(int playerLevel, string levelName, int levelCount, string difficulty, int levelLoop, bool isRandom, string levelType, string gameMode,
            string time, string reason, int progress, int continueValue)
            : base(playerLevel, levelName, levelCount, difficulty, levelLoop, isRandom, levelType, gameMode)
        {
            Time = time;
            Reason = reason;
            Progress = progress;
            ContinueValue = continueValue;
        }

        public override Dictionary<string, object> ToDictionary()
        {
            Dictionary<string, object> keys = new Dictionary<string, object>();

            Dictionary<string, object> momenet = new Dictionary<string, object>();
            momenet.Add(PlayerLevel.ToString(), PlayTimeInfo.TimeRange);

            keys.Add("level_number", momenet);
            keys.Add("level_name", LevelName);
            keys.Add("level_count", LevelCount);
            keys.Add("level_diff", Difficulty);
            keys.Add("level_loop", LevelLoop);
            keys.Add("level_random", (IsRandom) ? 1 : 0);
            keys.Add("level_type", LevelType);
            keys.Add("game_mode", GameMode);
            keys.Add("time", Time);
            keys.Add("reason", Reason);
            keys.Add("result", "lose");
            keys.Add("progress", Progress);
            keys.Add("continue", ContinueValue);

            return keys;
        }
    }

    public class ProgressCompletedInfo : ProgressStartInfo
    {
        public string Time;
        public int Progress;
        public int ContinueValue;

        public ProgressCompletedInfo(int playerLevel, string levelName, int levelCount, string difficulty, int levelLoop, bool isRandom, string levelType, string gameMode,
            string time, int progress, int continueValue)
            : base(playerLevel, levelName, levelCount, difficulty, levelLoop, isRandom, levelType, gameMode)
        {
            Time = time;
            Progress = progress;
            ContinueValue = continueValue;
        }

        public override Dictionary<string, object> ToDictionary()
        {
            Dictionary<string, object> keys = new Dictionary<string, object>();

            Dictionary<string, object> momenet = new Dictionary<string, object>();
            momenet.Add(PlayerLevel.ToString(), PlayTimeInfo.TimeRange);

            keys.Add("level_number", momenet);
            keys.Add("level_name", LevelName);
            keys.Add("level_count", LevelCount);
            keys.Add("level_diff", Difficulty);
            keys.Add("level_loop", LevelLoop);
            keys.Add("level_random", (IsRandom) ? 1 : 0);
            keys.Add("level_type", LevelType);
            keys.Add("game_mode", GameMode);
            keys.Add("time", Time);
            keys.Add("result", "win");
            keys.Add("progress", Progress);
            keys.Add("continue", ContinueValue);

            return keys;
        }
    }
}