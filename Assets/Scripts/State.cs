using UnityEngine;

public class State
{
    // Public Members
    public Sphere Sphere = Sphere.One;
    public Notation Notation = Notation.Complex;
    public Projection Projection = Projection.Overlapped;

    public Vector2 OneSpherePosition = new Vector2(0, 1);
    public Vector3 TwoSpherePosition = new Vector3(0, 0, 1);
    public Vector4 ThreeSpherePosition = new Vector4(0, 0, 0, 1);

    // Private Members
    private Vector2 TwoSphereCachedVectorXY = new Vector2(1, 0);
    private Vector2 TwoSphereCachedVectorXZ = new Vector2(0, 1);
    private Vector2 TwoSphereCachedVectorYZ = new Vector2(0, 1);


    // Public Methods
    public void Reset(QuaternionValue quaternionValue)
    {
        var quaternion = new Vector4();
        switch(Sphere)
        {
            case Sphere.One:
                quaternion.x = OneSpherePosition.x;
                quaternion.y = OneSpherePosition.y;
                break;
            case Sphere.Two:
                quaternion.x = TwoSpherePosition.x;
                quaternion.y = TwoSpherePosition.y;
                quaternion.z = TwoSpherePosition.z;
                break;
            case Sphere.Three:
                quaternion.w = ThreeSpherePosition.w;
                quaternion.x = ThreeSpherePosition.x;
                quaternion.y = ThreeSpherePosition.y;
                quaternion.z = ThreeSpherePosition.z;
                break;
        }

        switch(quaternionValue)
        {
            case QuaternionValue.q0:
                Drag(-quaternion.w, quaternionValue);
                break;
            case QuaternionValue.q1:
                Drag(-quaternion.x, quaternionValue);
                break;
            case QuaternionValue.q2:
                Drag(-quaternion.y, quaternionValue);
                break;
            case QuaternionValue.q3:
                Drag(-quaternion.z, quaternionValue);
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
                        var radiusYZ = Mathf.Sqrt(1 - TwoSpherePosition.x * TwoSpherePosition.x);
                        var scaledYZ = radiusYZ * TwoSphereCachedVectorYZ.normalized;
                        TwoSpherePosition.y = Clamp(scaledYZ.x);
                        TwoSpherePosition.z = Clamp(scaledYZ.y);

                        TwoSphereCachedVectorXY = new Vector2(TwoSpherePosition.x, TwoSpherePosition.y);
                        TwoSphereCachedVectorXZ = new Vector2(TwoSpherePosition.x, TwoSpherePosition.z);
                        break;
                    case QuaternionValue.q2:
                        TwoSpherePosition.y = Clamp(TwoSpherePosition.y + delta);
                        var radiusXZ = Mathf.Sqrt(1 - TwoSpherePosition.y * TwoSpherePosition.y);
                        var scaledXZ = radiusXZ * TwoSphereCachedVectorXZ.normalized;
                        TwoSpherePosition.x = Clamp(scaledXZ.x);
                        TwoSpherePosition.z = Clamp(scaledXZ.y);

                        TwoSphereCachedVectorXY = new Vector2(TwoSpherePosition.x, TwoSpherePosition.y);
                        TwoSphereCachedVectorYZ = new Vector2(TwoSpherePosition.y, TwoSpherePosition.z);
                        break;
                    case QuaternionValue.q3:
                        TwoSpherePosition.z = Clamp(TwoSpherePosition.z + delta);
                        var radiusXY = Mathf.Sqrt(1 - TwoSpherePosition.z * TwoSpherePosition.z);
                        var scaledXY = radiusXY * TwoSphereCachedVectorXY.normalized;
                        TwoSpherePosition.x = Clamp(scaledXY.x);
                        TwoSpherePosition.y = Clamp(scaledXY.y);
                        
                        TwoSphereCachedVectorXZ = new Vector2(TwoSpherePosition.x, TwoSpherePosition.z);
                        TwoSphereCachedVectorYZ = new Vector2(TwoSpherePosition.y, TwoSpherePosition.z);
                        break;
                }
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