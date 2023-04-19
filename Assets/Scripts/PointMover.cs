using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointMover : MonoBehaviour
{
    // Unity Members
    [SerializeField] private GlobalControl m_GlobalControl;
    [SerializeField] private Material m_PointMaterial;

    [SerializeField] private Color m_NorthColor;
    [SerializeField] private Color m_SouthColor;

    [SerializeField] private Sphere m_Sphere;
    [SerializeField] private bool m_Project;

    // Members
    private float m_OneSphereCachedSignY = 1;
    private float m_TwoSphereCachedSignY = 1;
    private float m_ThreeSphereCachedSignW = 1;

    // Unity Event Methods
    private void LateUpdate()
    {
        var position = Vector3.zero;
        var q0 = 0f;
        switch(m_Sphere)
        {
            case Sphere.One:
                position = m_GlobalControl.State.OneSpherePosition;
                q0 = m_GlobalControl.State.OneSpherePosition.y;
                if (m_Project)
                {
                    position.y = 0;
                }
                break;
            case Sphere.Two:
                position = m_GlobalControl.State.TwoSpherePosition;
                q0 = m_GlobalControl.State.TwoSpherePosition.y;
                if (m_Project)
                {
                    position.y = position.z;
                    position.z = 0;
                }
                break;
            case Sphere.Three:
                position = new Vector3(
                    m_GlobalControl.State.ThreeSpherePosition.x,
                    m_GlobalControl.State.ThreeSpherePosition.y,
                    m_GlobalControl.State.ThreeSpherePosition.z
                );
                q0 = m_GlobalControl.State.ThreeSpherePosition.w;
                break;
        }

        this.transform.localPosition = 0.9f * 0.5f * position;

        if (q0 < 0)
        {
            m_PointMaterial.SetColor("_Color", m_SouthColor);
        }
        else
        {
            m_PointMaterial.SetColor("_Color", m_NorthColor);
        }
    }

    // Public Methods
    public void Drag(Vector2 normalizedImagePosition)
    {
        if (m_Project)
        {
            switch(m_Sphere)
            {
                case Sphere.One:
                {
                    var x = Mathf.Clamp(normalizedImagePosition.x, -1f, 1f);
                    var previousY = m_GlobalControl.State.OneSpherePosition.y;
                    m_OneSphereCachedSignY = previousY == 0 ?  m_OneSphereCachedSignY : Mathf.Sign(previousY);
                    var y = m_OneSphereCachedSignY * Mathf.Sqrt(1 - x * x);

                    m_GlobalControl.State.OneSpherePosition = new Vector2(x, y);
                    break;
                }
                case Sphere.Two:
                {
                    Vector2 positionXZ;

                    var magnitudeSquared = Vector2.Dot(normalizedImagePosition, normalizedImagePosition);
                    if (magnitudeSquared > 1)
                    {
                        positionXZ = normalizedImagePosition / Mathf.Sqrt(magnitudeSquared);
                        magnitudeSquared = 1;
                    }
                    else
                    {
                        positionXZ = normalizedImagePosition;
                    }

                    var previousY = m_GlobalControl.State.TwoSpherePosition.y;
                    m_TwoSphereCachedSignY = previousY == 0 ? m_TwoSphereCachedSignY : Mathf.Sign(previousY);
                    var y = m_TwoSphereCachedSignY * Mathf.Sqrt(1 - magnitudeSquared);

                    m_GlobalControl.State.TwoSpherePosition = new Vector3(positionXZ.x, y, positionXZ.y);

                    break;
                }
                case Sphere.Three:
                {
                    Vector3 positionXYZ;

                    var magnitudeSquared = Vector2.Dot(normalizedImagePosition, normalizedImagePosition);
                    if (magnitudeSquared > 1f)
                    {
                        positionXYZ = normalizedImagePosition / Mathf.Sqrt(magnitudeSquared);
                        magnitudeSquared = 1;
                    }
                    else
                    {
                        positionXYZ = new Vector3
                        (
                            normalizedImagePosition.x,
                            normalizedImagePosition.y,
                            0
                        );
                    }

                    var rotatedPositionXYZ = this.transform.rotation * positionXYZ;
                    var previousW = m_GlobalControl.State.ThreeSpherePosition.w;
                    m_ThreeSphereCachedSignW = previousW == 0 ? m_ThreeSphereCachedSignW : Mathf.Sign(previousW);
                    var w = m_ThreeSphereCachedSignW * Mathf.Sqrt(1 - magnitudeSquared);

                    m_GlobalControl.State.ThreeSpherePosition = new Vector4(
                        rotatedPositionXYZ.x,
                        rotatedPositionXYZ.y,
                        rotatedPositionXYZ.z,
                        w
                    );
                    m_GlobalControl.State.RecalculateThreeSphereCachedVectors(true, true, true, true, true, true, true);
                    break;
                }
            }
        }
        else
        {
            switch(m_Sphere)
            {
                case Sphere.One:
                {
                    m_GlobalControl.State.OneSpherePosition = normalizedImagePosition.normalized;
                    break;
                }
                case Sphere.Two:
                {
                    Vector2 positionXY;

                    var magnitudeSquared = Vector2.Dot(normalizedImagePosition, normalizedImagePosition);
                    if (magnitudeSquared > 1f)
                    {
                        positionXY = normalizedImagePosition / Mathf.Sqrt(magnitudeSquared);
                        magnitudeSquared = 1;
                    }
                    else
                    {
                        positionXY = normalizedImagePosition;
                    }

                    var positionZ = -1f * Mathf.Sqrt(1 - magnitudeSquared);

                    var position = new Vector3(positionXY.x, positionXY.y, positionZ);
                    var rotatedPosition = this.transform.rotation * position;

                    m_GlobalControl.State.TwoSpherePosition = rotatedPosition;
                    m_GlobalControl.State.RecalculateTwoSphereCachedVectors(true, true, true);
                    break;
                }
            }
        }
    }
}
