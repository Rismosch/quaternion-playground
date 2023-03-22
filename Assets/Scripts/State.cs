using UnityEngine;

public class State
{
    // Members
    public Sphere Sphere = Sphere.One;
    public Notation Notation = Notation.Complex;
    public Projection Projection = Projection.Overlapped;

    public Vector2 OneSpherePosition = new Vector2(0, 1);

    public Vector3 TwoSpherePosition = new Vector3(0, 0, 1);
    public float TwoSphereAngleXY;
    public float TwoSphereAngleXZ;
    public float TwoSphereAngleZY;

    public Vector4 ThreeSpherePosition = new Vector4(1, 0, 0, 0);

    // Public Methods
    public void Reset(QuaternionValue quaternionValue)
    {
        if (Notation == Notation.AngleAxisRad || Notation == Notation.AngleAxisDeg)
        {
            Debug.LogError("not implemented yet");
            return;
        }

        switch(Sphere)
        {
            case Sphere.One:
                switch(quaternionValue)
                {
                    case QuaternionValue.q1:
                    OneSpherePosition = new Vector2(1, 0);
                        break;
                    case QuaternionValue.q2:
                    OneSpherePosition = new Vector2(0, 1);
                        break;
                }
                break;
            case Sphere.Two:
                switch(quaternionValue)
                {
                    case QuaternionValue.q1:
                        TwoSpherePosition = new Vector3(1, 0, 0);
                        break;
                    case QuaternionValue.q2:
                        TwoSpherePosition = new Vector3(0, 1, 0);
                        break;
                    case QuaternionValue.q3:
                        TwoSpherePosition = new Vector3(0, 0, 1);
                        break;
                }
                TwoSphereAngleXY = 0;
                TwoSphereAngleXZ = 0;
                TwoSphereAngleZY = 0;
                break;
            case Sphere.Three:
                break;
        }
    }

    public void Drag(float delta, QuaternionValue quaternionValue)
    {
        if (Notation == Notation.AngleAxisRad || Notation == Notation.AngleAxisDeg)
        {
            Debug.LogError("not implemented yet");
            return;
        }

        switch(Sphere)
        {
            case Sphere.One:
                switch(quaternionValue)
                {
                    case QuaternionValue.q1:
                        OneSpherePosition.x = Clamp(OneSpherePosition.x + delta);
                        OneSpherePosition.y = Mathf.Sqrt(1 - OneSpherePosition.x * OneSpherePosition.x);
                        break;
                    case QuaternionValue.q2:
                        OneSpherePosition.y = Clamp(OneSpherePosition.y + delta);
                        OneSpherePosition.x = Mathf.Sqrt(1 - OneSpherePosition.y * OneSpherePosition.y);
                        break;
                }
                break;
            case Sphere.Two:
                switch(quaternionValue)
                {
                    case QuaternionValue.q1:
                        TwoSpherePosition.x = Clamp(TwoSpherePosition.x + delta);
                        TwoSphereAngleXY = Mathf.Atan2(TwoSpherePosition.y, TwoSpherePosition.x);
                        TwoSphereAngleXZ = Mathf.Atan2(TwoSpherePosition.z, TwoSpherePosition.x);
                        var radiusZY = Mathf.Sqrt(1 - TwoSpherePosition.x * TwoSpherePosition.x);
                        TwoSpherePosition.y = Clamp(radiusZY * Mathf.Sin(TwoSphereAngleZY));
                        TwoSpherePosition.z = Clamp(radiusZY * Mathf.Cos(TwoSphereAngleZY));
                        break;
                    case QuaternionValue.q2:
                        TwoSpherePosition.y = Clamp(TwoSpherePosition.y + delta);
                        TwoSphereAngleXY = Mathf.Atan2(TwoSpherePosition.y, TwoSpherePosition.x);
                        TwoSphereAngleZY = Mathf.Atan2(TwoSpherePosition.y, TwoSpherePosition.z);
                        var radiusXZ = Mathf.Sqrt(1 - TwoSpherePosition.y * TwoSpherePosition.y);
                        TwoSpherePosition.x = Clamp(radiusXZ * Mathf.Cos(TwoSphereAngleXZ + Mathf.PI));
                        TwoSpherePosition.z = (radiusXZ * Mathf.Sin(TwoSphereAngleXZ + Mathf.PI));
                        break;
                    case QuaternionValue.q3:
                        TwoSpherePosition.z = Clamp(TwoSpherePosition.z + delta);
                        TwoSphereAngleXZ = Mathf.Atan2(TwoSpherePosition.z, TwoSpherePosition.x);
                        TwoSphereAngleZY = Mathf.Atan2(TwoSpherePosition.y, TwoSpherePosition.z);
                        var radiusXY = Mathf.Sqrt(1 - TwoSpherePosition.z * TwoSpherePosition.z);
                        TwoSpherePosition.x = Clamp(radiusXY * Mathf.Cos(TwoSphereAngleXY));
                        TwoSpherePosition.y = Clamp(radiusXY * Mathf.Sin(TwoSphereAngleXY));
                        break;
                }
                Debug.Log($"({TwoSpherePosition.x}, {TwoSpherePosition.y}, {TwoSpherePosition.z})");
                break;
            case Sphere.Three:
                break;
        }
    }

    public override string ToString()
    {
        return $"{{{nameof(Sphere)}: {Sphere}, {nameof(Notation)}: {Notation}, {nameof(Projection)}, {Projection}}}";
    }

    // Private Methods
    private float Clamp(float value)
    {
        if (value < 0.01f && value > -0.01f)
        {
            return 0;
        }

        return Mathf.Clamp(value, -1f, 1f);
    }
}