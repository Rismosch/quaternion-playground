using UnityEngine;

public class State
{
    // Public Members
    public Sphere Sphere = Sphere.One;
    public Notation Notation = Notation.Complex;
    public Projection Projection = Projection.Overlapped;

    public Vector2 OneSpherePosition = new Vector2(1, 0);
    public Vector3 TwoSpherePosition = new Vector3(1, 0, 0);
    public Vector4 ThreeSpherePosition = new Vector4(0, 0, 0, 1);

    // Private Members
    private Vector2 TwoSphereCachedVectorXY = new Vector2(1, 0);
    private Vector2 TwoSphereCachedVectorXZ = new Vector2(1, 0);
    private Vector2 TwoSphereCachedVectorYZ = new Vector2(1, 0);

    private Vector3 ThreeSphereCachedVectorWXY = new Vector3(1, 0, 0);
    private Vector3 ThreeSphereCachedVectorWXZ = new Vector3(1, 0, 0);
    private Vector3 ThreeSphereCachedVectorWYZ = new Vector3(1, 0, 0);
    private Vector3 ThreeSphereCachedVectorXYZ = new Vector3(1, 0, 0);


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

                        RecalculateTwoSphereCachedVectors(true, true, false);
                        break;
                    case QuaternionValue.q2:
                        TwoSpherePosition.y = Clamp(TwoSpherePosition.y + delta);
                        var radiusXZ = Mathf.Sqrt(1 - TwoSpherePosition.y * TwoSpherePosition.y);
                        var scaledXZ = radiusXZ * TwoSphereCachedVectorXZ.normalized;
                        TwoSpherePosition.x = Clamp(scaledXZ.x);
                        TwoSpherePosition.z = Clamp(scaledXZ.y);

                        RecalculateTwoSphereCachedVectors(true, false, true);
                        break;
                    case QuaternionValue.q3:
                        TwoSpherePosition.z = Clamp(TwoSpherePosition.z + delta);
                        var radiusXY = Mathf.Sqrt(1 - TwoSpherePosition.z * TwoSpherePosition.z);
                        var scaledXY = radiusXY * TwoSphereCachedVectorXY.normalized;
                        TwoSpherePosition.x = Clamp(scaledXY.x);
                        TwoSpherePosition.y = Clamp(scaledXY.y);

                        RecalculateTwoSphereCachedVectors(false, true, true);
                        break;
                }
                break;
            case Sphere.Three:
                switch(quaternionValue)
                {
                    case QuaternionValue.q0:
                        ThreeSpherePosition.w = Clamp(ThreeSpherePosition.w + delta);
                        var radiusXYZ = Mathf.Sqrt(1 - ThreeSpherePosition.w * ThreeSpherePosition.w);
                        var scaledXYZ = radiusXYZ * ThreeSphereCachedVectorXYZ.normalized;
                        ThreeSpherePosition.x = Clamp(scaledXYZ.x);
                        ThreeSpherePosition.y = Clamp(scaledXYZ.y);
                        ThreeSpherePosition.z = Clamp(scaledXYZ.z);

                        RecalculateThreeSphereCachedVectors(true, true, true, false);
                        break;
                    case QuaternionValue.q1:
                        ThreeSpherePosition.x = Clamp(ThreeSpherePosition.x + delta);
                        var radiusWYZ = Mathf.Sqrt(1 - ThreeSpherePosition.x * ThreeSpherePosition.x);
                        var scaledWYZ = radiusWYZ * ThreeSphereCachedVectorWYZ.normalized;
                        ThreeSpherePosition.w = Clamp(scaledWYZ.x);
                        ThreeSpherePosition.y = Clamp(scaledWYZ.y);
                        ThreeSpherePosition.z = Clamp(scaledWYZ.z);

                        RecalculateThreeSphereCachedVectors(true, true, false, true);
                        break;
                    case QuaternionValue.q2:
                        ThreeSpherePosition.y = Clamp(ThreeSpherePosition.y + delta);
                        var radiusWXZ = Mathf.Sqrt(1 - ThreeSpherePosition.y * ThreeSpherePosition.y);
                        var scaledWXZ = radiusWXZ * ThreeSphereCachedVectorWXZ.normalized;
                        ThreeSpherePosition.w = Clamp(scaledWXZ.x);
                        ThreeSpherePosition.x = Clamp(scaledWXZ.y);
                        ThreeSpherePosition.z = Clamp(scaledWXZ.z);

                        RecalculateThreeSphereCachedVectors(true, false, true, true);
                        break;
                    case QuaternionValue.q3:
                        ThreeSpherePosition.z = Clamp(ThreeSpherePosition.z + delta);
                        var radiusWXY = Mathf.Sqrt(1 - ThreeSpherePosition.z * ThreeSpherePosition.z);
                        var scaledWXY = radiusWXY * ThreeSphereCachedVectorWXY.normalized;
                        ThreeSpherePosition.w = Clamp(scaledWXY.x);
                        ThreeSpherePosition.x = Clamp(scaledWXY.y);
                        ThreeSpherePosition.y = Clamp(scaledWXY.z);

                        RecalculateThreeSphereCachedVectors(false, true, true, true);
                        break;
                }
                break;
        }
    }

    public void RecalculateTwoSphereCachedVectors(bool xy, bool xz, bool yz)
    {
        if (xy)
        {
            TwoSphereCachedVectorXY = new Vector2(TwoSpherePosition.x, TwoSpherePosition.y);
        }

        if (xz)
        {
            TwoSphereCachedVectorXZ = new Vector2(TwoSpherePosition.x, TwoSpherePosition.z);
        }

        if (yz)
        {
            TwoSphereCachedVectorYZ = new Vector2(TwoSpherePosition.y, TwoSpherePosition.z);
        }

        if (TwoSphereCachedVectorXY.magnitude < 0.01f)
        {
            TwoSphereCachedVectorXY = new Vector2(1, 0);
        }

        if (TwoSphereCachedVectorXZ.magnitude < 0.01f)
        {
            TwoSphereCachedVectorXZ = new Vector2(0, 1);
        }

        if (TwoSphereCachedVectorYZ.magnitude < 0.01f)
        {
            TwoSphereCachedVectorYZ = new Vector2(0, 1);
        }
    }

    public void RecalculateThreeSphereCachedVectors(bool wxy, bool wxz, bool wyz, bool xyz)
    {
        if (wxy)
        {
            ThreeSphereCachedVectorWXY = new Vector3(ThreeSpherePosition.w, ThreeSpherePosition.x, ThreeSpherePosition.y);
        }
        
        if (wxz)
        {
            ThreeSphereCachedVectorWXZ = new Vector3(ThreeSpherePosition.w, ThreeSpherePosition.x, ThreeSpherePosition.z);
        }
        
        if (wyz)
        {
            ThreeSphereCachedVectorWYZ = new Vector3(ThreeSpherePosition.w, ThreeSpherePosition.y, ThreeSpherePosition.z);
        }
        
        if (xyz)
        {
            ThreeSphereCachedVectorXYZ = new Vector3(ThreeSpherePosition.x, ThreeSpherePosition.y, ThreeSpherePosition.z);
        }

        if (ThreeSphereCachedVectorWXY.magnitude < 0.01f)
        {
            ThreeSphereCachedVectorWXY = new Vector3(1, 0, 0);
        }

        if (ThreeSphereCachedVectorWXZ.magnitude < 0.01f)
        {
            ThreeSphereCachedVectorWXZ = new Vector3(1, 0, 0);
        }

        if (ThreeSphereCachedVectorWYZ.magnitude < 0.01f)
        {
            ThreeSphereCachedVectorWYZ = new Vector3(1, 0, 0);
        }

        if (ThreeSphereCachedVectorXYZ.magnitude < 0.01f)
        {
            ThreeSphereCachedVectorXYZ = new Vector3(1, 0, 0);
        }
    }

    public override string ToString()
    {
        return $"{{{nameof(Sphere)}: {Sphere}, {nameof(Notation)}: {Notation}, {nameof(Projection)}, {Projection}}}";
    }

    // Private Methods
    private float Clamp(float value)
    {
        return Mathf.Clamp(value, -1f, 1f);
    }
}