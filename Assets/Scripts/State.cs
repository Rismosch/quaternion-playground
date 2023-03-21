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
    public void Update(State other, out bool changed) {
        var result = false;
        void Set<T>(ref T left, T right)
        {
            if (left.Equals(right)) {
                return;
            }

            left = right;
            result = true;
        }

        Set(ref this.Sphere, other.Sphere);
        Set(ref this.Notation, other.Notation);
        Set(ref this.Projection, other.Projection);

        changed = result;
    }

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
                        OneSpherePosition.x = Mathf.Clamp(OneSpherePosition.x + delta, -1, 1);
                        if (OneSpherePosition.x < 0.01f && OneSpherePosition.x > 0.01f)
                        {
                            OneSpherePosition.x = 0;
                        }
                        OneSpherePosition.y = Mathf.Sqrt(1 - OneSpherePosition.x * OneSpherePosition.x);
                        break;
                    case QuaternionValue.q2:
                        OneSpherePosition.y = Mathf.Clamp(OneSpherePosition.y + delta, -1, 1);
                        if (OneSpherePosition.y < 0.01f && OneSpherePosition.y > 0.01f)
                        {
                            OneSpherePosition.y = 0;
                        }
                        OneSpherePosition.x = Mathf.Sqrt(1 - OneSpherePosition.y * OneSpherePosition.y);
                        break;
                }
                break;
            case Sphere.Two:
                switch(quaternionValue)
                {
                    case QuaternionValue.q1:
                        TwoSpherePosition.x = Mathf.Clamp(TwoSpherePosition.x + delta, -1, 1);
                        if (TwoSpherePosition.x < 0.01f && TwoSpherePosition.x > -0.01f)
                        {
                            TwoSpherePosition.x = 0;
                        }
                        TwoSphereAngleXY = Mathf.Atan2(TwoSpherePosition.y, TwoSpherePosition.x);
                        TwoSphereAngleXZ = Mathf.Atan2(TwoSpherePosition.z, TwoSpherePosition.x);
                        var radiusZY = Mathf.Sqrt(1 - TwoSpherePosition.x * TwoSpherePosition.x);
                        TwoSpherePosition.y = radiusZY * Mathf.Sin(TwoSphereAngleZY);
                        TwoSpherePosition.z = radiusZY * Mathf.Cos(TwoSphereAngleZY);
                        break;
                    case QuaternionValue.q2:
                        TwoSpherePosition.y = Mathf.Clamp(TwoSpherePosition.y + delta, -1, 1);
                        if (TwoSpherePosition.y < 0.01f && TwoSpherePosition.y > -0.01f)
                        {
                            TwoSpherePosition.y = 0;
                        }
                        TwoSphereAngleXY = Mathf.Atan2(TwoSpherePosition.y, TwoSpherePosition.x);
                        TwoSphereAngleZY = Mathf.Atan2(TwoSpherePosition.y, TwoSpherePosition.z);
                        var radiusXZ = Mathf.Sqrt(1 - TwoSpherePosition.y * TwoSpherePosition.y);
                        TwoSpherePosition.x = radiusXZ * Mathf.Cos(TwoSphereAngleXZ + Mathf.PI);
                        TwoSpherePosition.z = radiusXZ * Mathf.Sin(TwoSphereAngleXZ + Mathf.PI);
                        break;
                    case QuaternionValue.q3:
                        TwoSpherePosition.z = Mathf.Clamp(TwoSpherePosition.z + delta, -1, 1);
                        if (TwoSpherePosition.z < 0.01f && TwoSpherePosition.z > -0.01f)
                        {
                            TwoSpherePosition.z = 0;
                        }
                        TwoSphereAngleXZ = Mathf.Atan2(TwoSpherePosition.z, TwoSpherePosition.x);
                        TwoSphereAngleZY = Mathf.Atan2(TwoSpherePosition.y, TwoSpherePosition.z);
                        var radiusXY = Mathf.Sqrt(1 - TwoSpherePosition.z * TwoSpherePosition.z);
                        TwoSpherePosition.x = radiusXY * Mathf.Cos(TwoSphereAngleXY);
                        TwoSpherePosition.y = radiusXY * Mathf.Sin(TwoSphereAngleXY);
                        break;
                }
                Debug.Log(TwoSpherePosition.x*TwoSpherePosition.x+TwoSpherePosition.y*TwoSpherePosition.y+TwoSpherePosition.z*TwoSpherePosition.z);
                break;
            case Sphere.Three:
                break;
        }
    }

    public override string ToString()
    {
        return $"{{{nameof(Sphere)}: {Sphere}, {nameof(Notation)}: {Notation}, {nameof(Projection)}, {Projection}}}";
    }
}