using System.Numerics;

namespace Etoile.Lite.Parser.Utility;

public static class UnityEngineExtensions
{
    public static bool Approximately(this double a, double b, double epsilon = 1e-9)
    {
        return Math.Abs(a - b) < epsilon;
    }

    public static bool Approximately(this float a, float b, double epsilon = 1e-9)
    {
        return Math.Abs(a - b) < epsilon;
    }


    /// <summary>
    /// created by ChatGPT according to https://gist.github.com/HelloKitty/91b7af87aac6796c3da9
    /// </summary>
    public static Vector3 ToEulerAngles(this Quaternion rotation)
    {
        const float rad2Deg = 57.29577951308232f;
        
        float sqw = rotation.W * rotation.W;
        float sqx = rotation.X * rotation.X;
        float sqy = rotation.Y * rotation.Y;
        float sqz = rotation.Z * rotation.Z;

        float unit = sqx                     + sqy + sqz + sqw;
        float test = rotation.X * rotation.W - rotation.Y * rotation.Z;

        Vector3 v;

        // Singularity at north pole.
        if (test > 0.4995f * unit)
        {
            v.Y = 2f       * MathF.Atan2(rotation.Y, rotation.X);
            v.X = MathF.PI / 2f;
            v.Z = 0f;

            return NormalizeAngles(v * rad2Deg);
        }

        // Singularity at south pole.
        if (test < -0.4995f * unit)
        {
            v.Y = -2f       * MathF.Atan2(rotation.Y, rotation.X);
            v.X = -MathF.PI / 2f;
            v.Z = 0f;

            return NormalizeAngles(v * rad2Deg);
        }

        // This reordering is important.
        //
        // Unity/Gist:
        // Quaternion q = new Quaternion(w, z, x, y);
        //
        // System.Numerics uses X/Y/Z/W, so:
        var qx = rotation.W;
        var qy = rotation.Z;
        var qz = rotation.X;
        var qw = rotation.Y;

        v.Y = MathF.Atan2(
            2f * qx * qw + 2f * qy * qz,
            1f           - 2f * (qz * qz + qw * qw)
        );

        v.X = MathF.Asin(
            2f * (qx * qz - qw * qy)
        );

        v.Z = MathF.Atan2(
            2f * qx * qy + 2f * qz * qw,
            1f           - 2f * (qy * qy + qz * qz)
        );

        return NormalizeAngles(v * rad2Deg);
    }

    private static Vector3 NormalizeAngles(Vector3 angles)
    {
        angles.X = NormalizeAngle(angles.X);
        angles.Y = NormalizeAngle(angles.Y);
        angles.Z = NormalizeAngle(angles.Z);

        return angles;
    }

    private static float NormalizeAngle(float angle)
    {
        angle %= 360f;
        if (angle < 0f) angle += 360f;
        angle = CleanAngle(angle);
        if (angle >= 360f) angle = 0f;

        return angle;

        float CleanAngle(float a)
        {
            if (MathF.Abs(a) < 1e-5f) return 0f;
            float nearestInteger = MathF.Round(a);
            
            return MathF.Abs(a - nearestInteger) < 1e-5f ? nearestInteger : a;
        }
    }
}