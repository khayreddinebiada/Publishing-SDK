using System.Text;
using UnityEngine;
using System;

namespace Apps.RemoteConfig.Linq
{
    public static class AnimationCurveSerializetion
    {
        private const char SeparatorValues = ',';
        private const char SeparatorLines = ':';

        public static string SerializeToString(this AnimationCurve curve)
        {
            if (curve == null) throw new ArgumentNullException(nameof(AnimationCurve));

            int totalKeys = curve.keys.Length;

            StringBuilder allValues = new StringBuilder(totalKeys);
            for (int i = 0; i < totalKeys; i++)
            {
                StringBuilder key = new StringBuilder(totalKeys);
                Keyframe keyframe = curve.keys[i];

                key.Append(keyframe.time.ToString());
                key.Append(SeparatorValues);
                key.Append(keyframe.value.ToString());
                key.Append(SeparatorValues);
                key.Append(keyframe.inTangent.ToString());
                key.Append(SeparatorValues);
                key.Append(keyframe.outTangent.ToString());
                key.Append(SeparatorValues);
                key.Append(keyframe.inWeight.ToString());
                key.Append(SeparatorValues);
                key.Append(keyframe.outWeight.ToString());
                key.Append(SeparatorValues);
                key.Append(((int)keyframe.weightedMode).ToString());

                if (i < totalKeys - 1)
                    key.Append(SeparatorLines);

                allValues.Append(key.ToString());
            }

            return allValues.ToString();
        }

        public static AnimationCurve ParseToAnimationCurve(this string serialized)
        {
            string[] values = serialized.Split(new char[] { SeparatorLines }, StringSplitOptions.RemoveEmptyEntries);
            Keyframe[] keyframes = new Keyframe[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                string[] key = values[i].Split(SeparatorValues);

                if (key.Length < 7)
                    return null;

                if (!float.TryParse(key[0], out float time))
                    return null;
                if (!float.TryParse(key[1], out float value))
                    return null;
                if (!float.TryParse(key[2], out float inTangent))
                    return null;
                if (!float.TryParse(key[3], out float outTangent))
                    return null;
                if (!float.TryParse(key[4], out float inWeight))
                    return null;
                if (!float.TryParse(key[5], out float outWeight))
                    return null;
                if (!int.TryParse(key[6], out int weightedMode))
                    return null;

                keyframes[i] = new Keyframe(time, value, inTangent, outTangent, inWeight, outWeight);
                keyframes[i].weightedMode = (WeightedMode)weightedMode;
            }

            return new AnimationCurve(keyframes);
        }
    }
}