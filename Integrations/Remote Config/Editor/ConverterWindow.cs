using Apps.RemoteConfig.Linq;
using NaughtyAttributes;
using UnityEngine;

namespace Apps.RemoteConfig.Editor
{
    [CreateAssetMenu(fileName = "Curve Converter", menuName = "RemoteConfig/Curve Converter", order = 1)]
    public class ConverterWindow : ScriptableObject
    {
        [SerializeField] private AnimationCurve _curve;
        [TextArea(5, 10), SerializeField] private string _result;
        public string Result
        {
            get => _result;
        }


        [Button]
        public void AnimationCurveToString()
        {
            _result = _curve.SerializeToString();
        }

        [Button]
        public void Validate()
        {
            AnimationCurve curve = _result.ParseToAnimationCurve();
            if (curve == null)
                Debug.LogError("The curve is unserializable!...");
            else
            {
                _curve = curve;
                Debug.Log("The curve is validated!...");
            }

        }
    }
}