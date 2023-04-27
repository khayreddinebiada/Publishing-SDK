using Apps.RemoteConfig;
using UnityEngine;
using Engine.DI;
using NaughtyAttributes;
using Apps.RemoteConfig.Linq;

public class RemoteConfig : MonoBehaviour
{
    public AnimationCurve Curve;

    [TextArea(5,10)]
    public string SerializationCurve;


    private IConfigServices _remoteConfig;

    private int _position = 1;
    private float _scale = 1;

    public void Start()
    {
        _remoteConfig = DIContainer.AsSingle<IConfigServices>();

        if (!_remoteConfig.IsReady)
            _remoteConfig.OnConfigured += OnConfigReady;
        else
        {
            OnConfigReady(_remoteConfig.Values);
        }
    }

    public void OnDestroy()
    {
        _remoteConfig.OnConfigured -= OnConfigReady;
    }

    public void OnConfigReady(IConfigCollection collection)
    {
        // Force Parse.
        try
        {
            _position = collection.ParseInt("Position");
            transform.position = new Vector3(_position, _position, _position);
            Debug.Log($"_position {_position}");
        }
        catch
        {
            _position = 1;
        }

        // Try Parse
        if (collection.TryParseFloat("Scale", out _scale))
        {
            transform.localScale = new Vector3(_scale, _scale, _scale);
            Debug.Log($"_scale {_scale}");
        }

        Debug.Log($"_remoteConfig : {_remoteConfig.TagConfig}");
    }
}
