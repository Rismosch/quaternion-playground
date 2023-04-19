using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoSphereProjectionLineController : MonoBehaviour
{
    // Unity Members
    [SerializeField] private GlobalControl m_GlobalControl;
    [SerializeField] private Transform m_X;
    [SerializeField] private Transform m_Z;
    [SerializeField] private Transform m_RadiusLine;
    [SerializeField] private Material m_LineMaterialX;
    [SerializeField] private Material m_LineMaterialZ;
    [SerializeField] private Material m_RadiusMaterial;

    [SerializeField] float m_Debug;

    // Unity Event Methods
    private void LateUpdate()
    {
        // Projection Lines
        float x = m_GlobalControl.State.TwoSpherePosition.x;
        float z = m_GlobalControl.State.TwoSpherePosition.z;

        m_X.localPosition = new Vector3(0.9f * 0.5f * x, 0, 0);
        m_Z.localPosition = new Vector3(0, 0.9f * 0.5f * z, 0);

        m_LineMaterialX.SetFloat("_Length", 0.9f * z);
        m_LineMaterialZ.SetFloat("_Length", 0.9f * x);

        // Radius Line
        var magnitude = Mathf.Sqrt(x * x + z * z);
        if (magnitude != 0)
        {
            var angle = Mathf.Acos(x / magnitude) * Mathf.Sign(z);
            m_Debug = angle;
            m_RadiusLine.localRotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, new Vector3(0, 0, 1));
        }

        m_RadiusMaterial.SetFloat("_Length", 0.9f * magnitude);
    }
}
