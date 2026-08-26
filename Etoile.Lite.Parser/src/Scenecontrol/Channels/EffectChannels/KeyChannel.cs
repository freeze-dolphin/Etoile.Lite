using Etoile.Lite.Parser.Scenecontrol.IO;
using Etoile.Lite.Parser.Utility;

namespace Etoile.Lite.Parser.Scenecontrol.Channels.EffectChannels;

public class KeyChannel : ValueChannel, IComparer<Key>
{
    private readonly CachedBinarySearch<Key, int>         keySearch;
    private readonly List<Key>                            keys;
    private          Func<double, double, double, double> defaultEasing;
    private          string                               defaultEasingString;

    public KeyChannel()
    {
        keySearch = new CachedBinarySearch<Key, int>(new List<Key>(), k => k.Timing, this);
        keys = keySearch.List;
    }

    public int KeyCount => keys.Count;

    public KeyChannel SetDefaultEasing(string easing)
    {
        defaultEasing = Easing.FromString(easing);
        defaultEasingString = easing;
        return this;
    }

    public override float ValueAt(int timing)
    {
        if (keys.Count == 0)
        {
            return 0;
        }

        if (keys.Count == 1)
        {
            return keys[0].Value;
        }

        // Extrapolate
        if (timing <= keys[0].Timing)
        {
            return keys[0].Value;
        }

        if (timing >= keys[keys.Count - 1].Timing)
        {
            return keys[keys.Count - 1].Value;
        }

        int index = keySearch.Search(timing);
        int timing1 = keys[index].Timing;
        int timing2 = keys[index + 1].Timing;
        Key key1 = keys[index];
        Key key2 = keys[index + 1];

        if (timing1 == timing2)
        {
            return key1.OverrideIndex > key2.OverrideIndex ? key1.Value : key2.Value;
        }

        float p = (float)(timing - timing1) / (timing2 - timing1);
        float value = (float)key1.Easing(key1.Value, key2.Value, p);

        return value;
    }

    public KeyChannel AddKey(
        int    timing,
        float  value,
        string easing = null)
    {
        Func<double, double, double, double> e;
        string estr;
        if (easing == null)
        {
            e = defaultEasing          ?? Easing.Linear;
            estr = defaultEasingString ?? "s";
        }
        else
        {
            e = Easing.FromString(easing);
            estr = easing;
        }

        int overrideIndex = 0;
        if (keys.Count > 0 && keys[keySearch.Search(timing)].Timing == timing)
        {
            overrideIndex += 1;
        }

        Key key = new Key
        {
            Timing = timing,
            Value = value,
            Easing = e,
            EasingString = estr,
            OverrideIndex = overrideIndex,
        };

        keys.Add(key);
        keySearch.Sort();
        return this;
    }

    public KeyChannel RemoveKeyAtTiming(int timing)
    {
        int index = keySearch.Search(timing);
        if (keys[index].Timing == timing)
        {
            keys.RemoveAt(index);
        }

        keySearch.Sort();
        return this;
    }

    public int Compare(Key x, Key y)
    {
        if (x.Timing == y.Timing)
        {
            return x.OverrideIndex.CompareTo(y.OverrideIndex);
        }

        return x.Timing.CompareTo(y.Timing);
    }

    public override List<object>? SerializeProperties(ScenecontrolSerialization serialization)
    {
        List<object> result = new List<object>(keys.Count);

        foreach (var key in keys)
        {
            result.Add(key.Serialize());
        }

        return result;
    }

    protected override IEnumerable<ValueChannel> GetChildrenChannels()
    {
        yield break;
    }
}