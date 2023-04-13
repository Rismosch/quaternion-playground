using UnityEngine;

public class State
{
    // Public Members
    public Sphere Sphere = Sphere.One;
    public Notation Notation = Notation.Complex;

    // Properties
    public Vector2 OneSpherePosition
    {
        get => m_OneSpherePosition;
        set => m_OneSpherePosition = value;
    }

    public Vector3 TwoSpherePosition
    {
        get => m_TwoSpherePosition;
        set => m_TwoSpherePosition = value;
    }

    public Vector4 ThreeSpherePosition
    {
        get => m_ThreeSpherePosition;
        set
        {
            m_ThreeSpherePosition = value;

            var theta = 2 * Mathf.Acos(value.w);
            if (theta == 0)
            {
                m_ThreeSphereAngleAxis = new Vector4(
                    m_ThreeSphereAngleAxis.x,
                    m_ThreeSphereAngleAxis.y,
                    m_ThreeSphereAngleAxis.z,
                    0
                );
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
            m_ThreeSphereAngleAxis = value;

            var cosTheta = Mathf.Cos(0.5f * value.w);
            var sinTheta = Mathf.Sin(0.5f * value.w);
            var w = cosTheta;
            var x = value.x * sinTheta;
            var y = value.y * sinTheta;
            var z = value.z * sinTheta;

            m_ThreeSpherePosition = new Vector4(x, y, z, w);
        }
    }

    // Private Members
    private Vector2 m_OneSpherePosition = new Vector2(1, 0);
    private Vector3 m_TwoSpherePosition = new Vector3(1, 0, 0);
    private Vector4 m_ThreeSpherePosition = new Vector4(0, 0, 0, 1);
    private Vector4 m_ThreeSphereAngleAxis = new Vector4(1, 0, 0, 0);

    private float m_CachedSignX = 1;
    private float m_CachedSignY = 1;

    private Vector2 m_TwoSphereCachedVectorXY = new Vector2(1, 0);
    private Vector2 m_TwoSphereCachedVectorXZ = new Vector2(1, 0);
    private Vector2 m_TwoSphereCachedVectorYZ = new Vector2(1, 0);

    private Vector3 m_ThreeSphereCachedVectorWXY = new Vector3(1, 0, 0);
    private Vector3 m_ThreeSphereCachedVectorWXZ = new Vector3(1, 0, 0);
    private Vector3 m_ThreeSphereCachedVectorWYZ = new Vector3(1, 0, 0);
    private Vector3 m_ThreeSphereCachedVectorXYZ = new Vector3(1, 0, 0);

    private Vector2 m_ThreeSphereCachedAxisXY = new Vector2(1, 0);
    private Vector2 m_ThreeSphereCachedAxisXZ = new Vector2(1, 0);
    private Vector2 m_ThreeSphereCachedAxisYZ = new Vector2(1, 0);


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
                if (Notation == Notation.AngleAxisRad || Notation == Notation.AngleAxisDeg)
                {
                    quaternion.w = ThreeSphereAngleAxis.w;
                    quaternion.x = ThreeSphereAngleAxis.x;
                    quaternion.y = ThreeSphereAngleAxis.y;
                    quaternion.z = ThreeSphereAngleAxis.z;
                }
                else
                {
                    quaternion.w = ThreeSpherePosition.w;
                    quaternion.x = ThreeSpherePosition.x;
                    quaternion.y = ThreeSpherePosition.y;
                    quaternion.z = ThreeSpherePosition.z;
                }
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
            switch(quaternionValue)
            {
                case QuaternionValue.q0:
                {
                    var scaledDelta = delta * Mathf.PI;
                    var w = Mathf.Clamp(m_ThreeSphereAngleAxis.w + scaledDelta, 0, 2 * Mathf.PI);
                    var x = m_ThreeSphereAngleAxis.x;
                    var y = m_ThreeSphereAngleAxis.y;
                    var z = m_ThreeSphereAngleAxis.z;

                    ThreeSphereAngleAxis = new Vector4(x, y, z, w);
                    RecalculateThreeSphereCachedVectors(true, true, true, true, true, true, true);
                    break;
                }
                case QuaternionValue.q1:
                {
                    var x = Mathf.Clamp(m_ThreeSphereAngleAxis.x + delta, -1f, 1f);
                    var radiusYZ = Mathf.Sqrt(1 - x * x);
                    var scaledYZ = radiusYZ * m_ThreeSphereCachedAxisYZ.normalized;
                    var y = scaledYZ.x;
                    var z = scaledYZ.y;

                    ThreeSphereAngleAxis = new Vector4(x, y, z, m_ThreeSphereAngleAxis.w);
                    RecalculateThreeSphereCachedVectors(true, true, true, true, true, true, false);
                    break;
                }
                case QuaternionValue.q2:
                {
                    var y = Mathf.Clamp(m_ThreeSphereAngleAxis.y + delta, -1f, 1f);
                    var radiusXZ = Mathf.Sqrt(1 - y * y);
                    var scaledXZ = radiusXZ * m_ThreeSphereCachedAxisXZ.normalized;
                    var x = scaledXZ.x;
                    var z = scaledXZ.y;

                    ThreeSphereAngleAxis = new Vector4(x, y, z, m_ThreeSphereAngleAxis.w);
                    RecalculateThreeSphereCachedVectors(true, true, true, true, true, false, true);
                    break;
                }
                case QuaternionValue.q3:
                {
                    var z = Mathf.Clamp(m_ThreeSphereAngleAxis.z + delta, -1f, 1f);
                    var radiusXY = Mathf.Sqrt(1 - z * z);
                    var scaledXY = radiusXY * m_ThreeSphereCachedAxisXY.normalized;
                    var x = scaledXY.x;
                    var y = scaledXY.y;

                    ThreeSphereAngleAxis = new Vector4(x, y, z, m_ThreeSphereAngleAxis.w);
                    RecalculateThreeSphereCachedVectors(true, true, true, true, false, true, true);
                    break;
                }
            }
            return;
        }

        switch(Sphere)
        {
            case Sphere.One:
                switch(quaternionValue)
                {
                    case QuaternionValue.q1:
                        m_OneSpherePosition.x = Mathf.Clamp(OneSpherePosition.x + delta, -1f, 1f);
                        var previousY = m_OneSpherePosition.y;
                        m_CachedSignY = previousY == 0 ? m_CachedSignY : Mathf.Sign(previousY);
                        m_OneSpherePosition.y = m_CachedSignY * Mathf.Sqrt(1 - OneSpherePosition.x * OneSpherePosition.x);
                        break;
                    case QuaternionValue.q2:
                        m_OneSpherePosition.y = Mathf.Clamp(OneSpherePosition.y + delta, -1f, 1f);
                        var previousX = m_OneSpherePosition.x;
                        m_CachedSignX = previousX == 0 ? m_CachedSignX : Mathf.Sign(previousX);
                        m_OneSpherePosition.x = m_CachedSignX * Mathf.Sqrt(1 - OneSpherePosition.y * OneSpherePosition.y);
                        break;
                }
                break;
            case Sphere.Two:
                switch(quaternionValue)
                {
                    case QuaternionValue.q1:
                        m_TwoSpherePosition.x = Mathf.Clamp(TwoSpherePosition.x + delta, -1f, 1f);
                        var radiusYZ = Mathf.Sqrt(1 - TwoSpherePosition.x * TwoSpherePosition.x);
                        var scaledYZ = radiusYZ * m_TwoSphereCachedVectorYZ.normalized;
                        m_TwoSpherePosition.y = scaledYZ.x;
                        m_TwoSpherePosition.z = scaledYZ.y;

                        RecalculateTwoSphereCachedVectors(true, true, false);
                        break;
                    case QuaternionValue.q2:
                        m_TwoSpherePosition.y = Mathf.Clamp(TwoSpherePosition.y + delta, -1f, 1f);
                        var radiusXZ = Mathf.Sqrt(1 - TwoSpherePosition.y * TwoSpherePosition.y);
                        var scaledXZ = radiusXZ * m_TwoSphereCachedVectorXZ.normalized;
                        m_TwoSpherePosition.x = scaledXZ.x;
                        m_TwoSpherePosition.z = scaledXZ.y;

                        RecalculateTwoSphereCachedVectors(true, false, true);
                        break;
                    case QuaternionValue.q3:
                        m_TwoSpherePosition.z = Mathf.Clamp(TwoSpherePosition.z + delta, -1f, 1f);
                        var radiusXY = Mathf.Sqrt(1 - TwoSpherePosition.z * TwoSpherePosition.z);
                        var scaledXY = radiusXY * m_TwoSphereCachedVectorXY.normalized;
                        m_TwoSpherePosition.x = scaledXY.x;
                        m_TwoSpherePosition.y = scaledXY.y;

                        RecalculateTwoSphereCachedVectors(false, true, true);
                        break;
                }
                break;
            case Sphere.Three:
                switch(quaternionValue)
                {
                    case QuaternionValue.q0:
                    {
                        var w = Mathf.Clamp(m_ThreeSpherePosition.w + delta, -1f, 1f);
                        var radiusXYZ = Mathf.Sqrt(1 - w * w);
                        var scaledXYZ = radiusXYZ * m_ThreeSphereCachedVectorXYZ.normalized;
                        var x = scaledXYZ.x;
                        var y = scaledXYZ.y;
                        var z = scaledXYZ.z;

                        ThreeSpherePosition = new Vector4(x, y, z, w);
                        RecalculateThreeSphereCachedVectors(true, true, true, false, true, true, true);
                        break;
                    }
                    case QuaternionValue.q1:
                    {
                        var x = Mathf.Clamp(m_ThreeSpherePosition.x + delta, -1f, 1f);
                        var radiusWYZ = Mathf.Sqrt(1 - x * x);
                        var scaledWYZ = radiusWYZ * m_ThreeSphereCachedVectorWYZ.normalized;
                        var w = scaledWYZ.x;
                        var y = scaledWYZ.y;
                        var z = scaledWYZ.z;

                        ThreeSpherePosition = new Vector4(x, y, z, w);
                        RecalculateThreeSphereCachedVectors(true, true, false, true, true, true, true);
                        break;
                    }
                    case QuaternionValue.q2:
                    {
                        var y = Mathf.Clamp(m_ThreeSpherePosition.y + delta, -1f, 1f);
                        var radiusWXZ = Mathf.Sqrt(1 - y * y);
                        var scaledWXZ = radiusWXZ * m_ThreeSphereCachedVectorWXZ.normalized;
                        var w = scaledWXZ.x;
                        var x = scaledWXZ.y;
                        var z = scaledWXZ.z;

                        ThreeSpherePosition = new Vector4(x, y, z, w);
                        RecalculateThreeSphereCachedVectors(true, false, true, true, true, true, true);
                        break;
                    }
                    case QuaternionValue.q3:
                    {
                        var z = Mathf.Clamp(ThreeSpherePosition.z + delta, -1f, 1f);
                        var radiusWXY = Mathf.Sqrt(1 - z * z);
                        var scaledWXY = radiusWXY * m_ThreeSphereCachedVectorWXY.normalized;
                        var w = scaledWXY.x;
                        var x = scaledWXY.y;
                        var y = scaledWXY.z;

                        ThreeSpherePosition = new Vector4(x, y, z, w);
                        RecalculateThreeSphereCachedVectors(false, true, true, true, true, true, true);
                        break;
                    }
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

    public void RecalculateThreeSphereCachedVectors(bool wxy, bool wxz, bool wyz, bool xyz, bool axisXY, bool axisXZ, bool axisYZ)
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
        
        if (axisXY)
        {
            m_ThreeSphereCachedAxisXY = new Vector2(ThreeSphereAngleAxis.x, ThreeSphereAngleAxis.y);
        }
        
        if (axisXZ)
        {
            m_ThreeSphereCachedAxisXZ = new Vector2(ThreeSphereAngleAxis.x, ThreeSphereAngleAxis.z);
        }
        
        if (axisYZ)
        {
            m_ThreeSphereCachedAxisYZ = new Vector2(ThreeSphereAngleAxis.y, ThreeSphereAngleAxis.z);
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

        if (m_ThreeSphereCachedAxisXY.magnitude < 0.01f)
        {
            m_ThreeSphereCachedAxisXY = new Vector3(1, 0);
        }

        if (m_ThreeSphereCachedAxisXZ.magnitude < 0.01f)
        {
            m_ThreeSphereCachedAxisXZ = new Vector3(1, 0);
        }

        if (m_ThreeSphereCachedAxisYZ.magnitude < 0.01f)
        {
            m_ThreeSphereCachedAxisYZ = new Vector3(1, 0);
        }
    }

    public override string ToString()
    {
        return $"{{{nameof(Sphere)}: {Sphere}, {nameof(Notation)}: {Notation}}}";
    }
}