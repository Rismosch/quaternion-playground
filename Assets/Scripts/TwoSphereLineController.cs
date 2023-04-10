using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoSphereLineController : MonoBehaviour
{
    // Unity Members
    [SerializeField] private GlobalControl m_GlobalControl;
    [SerializeField] private Transform m_X;
    [SerializeField] private Transform m_Y;
    [SerializeField] private Transform m_Z;
    [SerializeField] private Transform m_XZ;
    [SerializeField] private Material m_LineMaterialX;
    [SerializeField] private Material m_LineMaterialY;
    [SerializeField] private Material m_LineMaterialZ;
    [SerializeField] private Material m_LineMaterialXZ;
    [SerializeField] private Material m_RadiusMaterial;

    [SerializeField] private AxisRotator m_AxisRotatorProjectionY;
    [SerializeField] private AxisRotator m_AxisRotatorRadius;

    [SerializeField] private Sphere m_Sphere;

    // Unity Event Methods
    private void LateUpdate()
    {
        float x;
        float y;
        float z;
        float radiusLength;

        switch(m_Sphere)
        {
            case Sphere.Two:
                radiusLength = 1;
                x = m_GlobalControl.State.TwoSpherePosition.x;
                y = m_GlobalControl.State.TwoSpherePosition.y;
                z = m_GlobalControl.State.TwoSpherePosition.z;
                break;
            case Sphere.Three:
                x = m_GlobalControl.State.ThreeSpherePosition.x;
                y = m_GlobalControl.State.ThreeSpherePosition.y;
                z = m_GlobalControl.State.ThreeSpherePosition.z;
                radiusLength = Mathf.Sqrt(x * x + y * y + z * z);
                break;
            default:
                return;
        }

        m_X.localPosition = new Vector3(0.9f * 0.5f * x, 0,0);
        m_Z.localPosition = new Vector3(0, 0, 0.9f * 0.5f * z);
        m_XZ.localPosition = new Vector3(0.9f * 0.5f * x, 0, 0.9f * 0.5f * z);
        m_Y.localPosition = new Vector3(0, 0.9f * 0.5f * y, 0);

        m_LineMaterialX.SetFloat("_Length", 0.9f * z);
        m_LineMaterialZ.SetFloat("_Length", 0.9f * x);
        m_LineMaterialXZ.SetFloat("_Length", 0.9f * y);
        m_LineMaterialY.SetFloat("_Length", 0.9f * Mathf.Sqrt(x * x + z * z));
        m_RadiusMaterial.SetFloat("_Length", 0.9f * radiusLength);

        m_AxisRotatorProjectionY.Right = -1f * new Vector3(x, 0, z);
        m_AxisRotatorRadius.Right = -1f * new Vector3(x, y, z);
    }
}
