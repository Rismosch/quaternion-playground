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

    [SerializeField] private Vector3 m_Debug;

    // Members
    private float m_CachedSignW;

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
                break;
            case Sphere.Two:
                position = m_GlobalControl.State.TwoSpherePosition;
                q0 = m_GlobalControl.State.TwoSpherePosition.y;
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

        this.transform.position = 0.9f * 0.5f * position;

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
                m_CachedSignW = previousW != 0 ? Mathf.Sign(previousW) : m_CachedSignW;
                var w = m_CachedSignW * Mathf.Sqrt(1 - magnitudeSquared);

                m_GlobalControl.State.ThreeSpherePosition = new Vector4(
                    rotatedPositionXYZ.x,
                    rotatedPositionXYZ.y,
                    rotatedPositionXYZ.z,
                    w
                );
                m_GlobalControl.State.RecalculateThreeSphereCachedVectors(true, true, true, true);
                break;
            }
        }
    }
}
