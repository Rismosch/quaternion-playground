using UnityEngine;

public class State
{
    // Public Members
    public Sphere Sphere = Sphere.One;
    public Notation Notation = Notation.Complex;

    // Properties
    public Vector2 OneSpherePosition { get; set; }

    public Vector3 TwoSpherePosition { get; set; }

    public Vector4 ThreeSpherePosition
    {
        get => m_ThreeSpherePosition;
        set
        {
            m_ThreeSpherePosition = value;

            var theta = 2 * Mathf.Acos(value.w);
            if (theta == 0)
            {
                m_ThreeSphereAngleAxis = new Vector4(float.NaN, float.NaN, float.NaN, 0);
            }
            else
            {
                var x = value.x / Mathf.Sin(0.5f * theta);
                var y = value.y / Mathf.Sin(0.5f * theta);
                var z = value.z / Mathf.Sin(0.5f * theta);

                m_ThreeSphereAngleAxis = new Vector4(x, y, z, theta);
            }
        }
    }

    public Vector4 ThreeSphereAngleAxis
    {
        get => m_ThreeSphereAngleAxis;
        set
        {
            var magnitude = Mathf.Sqrt(value.x * value.x + value.y * value.y + value.z * value.z);
            var xN = value.x / magnitude;
            var yN = value.y / magnitude;
            var zN = value.z / magnitude;
            var theta = value.w;

            while(theta > 2 * Mathf.PI)
            {
                theta -= 2 * Mathf.PI;
            }

            while(theta < 0)
            {
                theta += 2 * Mathf.PI;
            }

            var cosTheta = Mathf.Cos(0.5f * theta);
            var sinTheta = Mathf.Sin(0.5f * theta);
            var w = cosTheta;
            var x = xN * sinTheta;
            var y = yN * sinTheta;
            var z = zN * sinTheta;

            m_ThreeSphereAngleAxis = new Vector4(xN, yN, zN, theta);
            m_ThreeSpherePosition = new Vector4(x, y, z, w);
        }
    }

    // Private Members
    private Vector2 m_OneSpherePosition = new Vector2(1, 0);
    private Vector3 m_TwoSpherePosition = new Vector3(1, 0, 0);
    private Vector4 m_ThreeSpherePosition = new Vector4(0, 0, 0, 1);
    private Vector4 m_ThreeSphereAngleAxis = new Vector4(0, float.NaN, float.NaN, float.NaN);

    private Vector2 m_TwoSphereCachedVectorXY = new Vector2(1, 0);
    private Vector2 m_TwoSphereCachedVectorXZ = new Vector2(1, 0);
    private Vector2 m_TwoSphereCachedVectorYZ = new Vector2(1, 0);

    private Vector3 m_ThreeSphereCachedVectorWXY = new Vector3(1, 0, 0);
    private Vector3 m_ThreeSphereCachedVectorWXZ = new Vector3(1, 0, 0);
    private Vector3 m_ThreeSphereCachedVectorWYZ = new Vector3(1, 0, 0);
    private Vector3 m_ThreeSphereCachedVectorXYZ = new Vector3(1, 0, 0);


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
                        m_OneSpherePosition.x = Clamp(OneSpherePosition.x + delta);
                        m_OneSpherePosition.y = Mathf.Sqrt(1 - OneSpherePosition.x * OneSpherePosition.x);
                        break;
                    case QuaternionValue.q2:
                        m_OneSpherePosition.y = Clamp(OneSpherePosition.y + delta);
                        m_OneSpherePosition.x = Mathf.Sqrt(1 - OneSpherePosition.y * OneSpherePosition.y);
                        break;
                }
                break;
            case Sphere.Two:
                switch(quaternionValue)
                {
                    case QuaternionValue.q1:
                        m_TwoSpherePosition.x = Clamp(TwoSpherePosition.x + delta);
                        var radiusYZ = Mathf.Sqrt(1 - TwoSpherePosition.x * TwoSpherePosition.x);
                        var scaledYZ = radiusYZ * m_TwoSphereCachedVectorYZ.normalized;
                        m_TwoSpherePosition.y = Clamp(scaledYZ.x);
                        m_TwoSpherePosition.z = Clamp(scaledYZ.y);

                        RecalculateTwoSphereCachedVectors(true, true, false);
                        break;
                    case QuaternionValue.q2:
                        m_TwoSpherePosition.y = Clamp(TwoSpherePosition.y + delta);
                        var radiusXZ = Mathf.Sqrt(1 - TwoSpherePosition.y * TwoSpherePosition.y);
                        var scaledXZ = radiusXZ * m_TwoSphereCachedVectorXZ.normalized;
                        m_TwoSpherePosition.x = Clamp(scaledXZ.x);
                        m_TwoSpherePosition.z = Clamp(scaledXZ.y);

                        RecalculateTwoSphereCachedVectors(true, false, true);
                        break;
                    case QuaternionValue.q3:
                        m_TwoSpherePosition.z = Clamp(TwoSpherePosition.z + delta);
                        var radiusXY = Mathf.Sqrt(1 - TwoSpherePosition.z * TwoSpherePosition.z);
                        var scaledXY = radiusXY * m_TwoSphereCachedVectorXY.normalized;
                        m_TwoSpherePosition.x = Clamp(scaledXY.x);
                        m_TwoSpherePosition.y = Clamp(scaledXY.y);

                        RecalculateTwoSphereCachedVectors(false, true, true);
                        break;
                }
                break;
            case Sphere.Three:
                switch(quaternionValue)
                {
                    case QuaternionValue.q0:
                        m_ThreeSpherePosition.w = Clamp(ThreeSpherePosition.w + delta);
                        var radiusXYZ = Mathf.Sqrt(1 - ThreeSpherePosition.w * ThreeSpherePosition.w);
                        var scaledXYZ = radiusXYZ * m_ThreeSphereCachedVectorXYZ.normalized;
                        m_ThreeSpherePosition.x = Clamp(scaledXYZ.x);
                        m_ThreeSpherePosition.y = Clamp(scaledXYZ.y);
                        m_ThreeSpherePosition.z = Clamp(scaledXYZ.z);

                        RecalculateThreeSphereCachedVectors(true, true, true, false);
                        break;
                    case QuaternionValue.q1:
                        m_ThreeSpherePosition.x = Clamp(ThreeSpherePosition.x + delta);
                        var radiusWYZ = Mathf.Sqrt(1 - ThreeSpherePosition.x * ThreeSpherePosition.x);
                        var scaledWYZ = radiusWYZ * m_ThreeSphereCachedVectorWYZ.normalized;
                        m_ThreeSpherePosition.w = Clamp(scaledWYZ.x);
                        m_ThreeSpherePosition.y = Clamp(scaledWYZ.y);
                        m_ThreeSpherePosition.z = Clamp(scaledWYZ.z);

                        RecalculateThreeSphereCachedVectors(true, true, false, true);
                        break;
                    case QuaternionValue.q2:
                        m_ThreeSpherePosition.y = Clamp(ThreeSpherePosition.y + delta);
                        var radiusWXZ = Mathf.Sqrt(1 - ThreeSpherePosition.y * ThreeSpherePosition.y);
                        var scaledWXZ = radiusWXZ * m_ThreeSphereCachedVectorWXZ.normalized;
                        m_ThreeSpherePosition.w = Clamp(scaledWXZ.x);
                        m_ThreeSpherePosition.x = Clamp(scaledWXZ.y);
                        m_ThreeSpherePosition.z = Clamp(scaledWXZ.z);

                        RecalculateThreeSphereCachedVectors(true, false, true, true);
                        break;
                    case QuaternionValue.q3:
                        m_ThreeSpherePosition.z = Clamp(ThreeSpherePosition.z + delta);
                        var radiusWXY = Mathf.Sqrt(1 - ThreeSpherePosition.z * ThreeSpherePosition.z);
                        var scaledWXY = radiusWXY * m_ThreeSphereCachedVectorWXY.normalized;
                        m_ThreeSpherePosition.w = Clamp(scaledWXY.x);
                        m_ThreeSpherePosition.x = Clamp(scaledWXY.y);
                        m_ThreeSpherePosition.y = Clamp(scaledWXY.z);

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
            m_TwoSphereCachedVectorXY = new Vector2(TwoSpherePosition.x, TwoSpherePosition.y);
        }

        if (xz)
        {
            m_TwoSphereCachedVectorXZ = new Vector2(TwoSpherePosition.x, TwoSpherePosition.z);
        }

        if (yz)
        {
            m_TwoSphereCachedVectorYZ = new Vector2(TwoSpherePosition.y, TwoSpherePosition.z);
        }

        if (m_TwoSphereCachedVectorXY.magnitude < 0.01f)
        {
            m_TwoSphereCachedVectorXY = new Vector2(1, 0);
        }

        if (m_TwoSphereCachedVectorXZ.magnitude < 0.01f)
        {
            m_TwoSphereCachedVectorXZ = new Vector2(0, 1);
        }

        if (m_TwoSphereCachedVectorYZ.magnitude < 0.01f)
        {
            m_TwoSphereCachedVectorYZ = new Vector2(0, 1);
        }
    }

    public void RecalculateThreeSphereCachedVectors(bool wxy, bool wxz, bool wyz, bool xyz)
    {
        if (wxy)
        {
            m_ThreeSphereCachedVectorWXY = new Vector3(ThreeSpherePosition.w, ThreeSpherePosition.x, ThreeSpherePosition.y);
        }
        
        if (wxz)
        {
            m_ThreeSphereCachedVectorWXZ = new Vector3(ThreeSpherePosition.w, ThreeSpherePosition.x, ThreeSpherePosition.z);
        }
        
        if (wyz)
        {
            m_ThreeSphereCachedVectorWYZ = new Vector3(ThreeSpherePosition.w, ThreeSpherePosition.y, ThreeSpherePosition.z);
        }
        
        if (xyz)
        {
            m_ThreeSphereCachedVectorXYZ = new Vector3(ThreeSpherePosition.x, ThreeSpherePosition.y, ThreeSpherePosition.z);
        }

        if (m_ThreeSphereCachedVectorWXY.magnitude < 0.01f)
        {
            m_ThreeSphereCachedVectorWXY = new Vector3(1, 0, 0);
        }

        if (m_ThreeSphereCachedVectorWXZ.magnitude < 0.01f)
        {
            m_ThreeSphereCachedVectorWXZ = new Vector3(1, 0, 0);
        }

        if (m_ThreeSphereCachedVectorWYZ.magnitude < 0.01f)
        {
            m_ThreeSphereCachedVectorWYZ = new Vector3(1, 0, 0);
        }

        if (m_ThreeSphereCachedVectorXYZ.magnitude < 0.01f)
        {
            m_ThreeSphereCachedVectorXYZ = new Vector3(1, 0, 0);
        }
    }

    public override string ToString()
    {
        return $"{{{nameof(Sphere)}: {Sphere}, {nameof(Notation)}: {Notation}}}";
    }

    // Private Methods
    private float Clamp(float value)
    {
        return Mathf.Clamp(value, -1f, 1f);
    }
}