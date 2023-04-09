using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneSphereLineController : MonoBehaviour
{
    // Unity Members
    [SerializeField] private GlobalControl m_GlobalControl;
    [SerializeField] private Transform m_X;
    [SerializeField] private Transform m_Y;
    [SerializeField] private Material m_LineMaterialX;
    [SerializeField] private Material m_LineMaterialY;
    [SerializeField] private Transform m_RadiusLine;

    // Unity Event Methods
    private void LateUpdate()
    {
        // Projection Lines
        float x = m_GlobalControl.State.OneSpherePosition.x;
        float y = m_GlobalControl.State.OneSpherePosition.y;

        m_X.localPosition = new Vector3(0.9f * 0.5f * x, 0, 0);
        m_Y.localPosition = new Vector3(0, 0.9f * 0.5f * y, 0);

        m_LineMaterialX.SetFloat("_Length", 0.9f * y);
        m_LineMaterialY.SetFloat("_Length", 0.9f * x);

        // Radius Line
        var angle = Mathf.Acos(x) * Mathf.Sign(y);
        m_RadiusLine.localRotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, new Vector3(0, 0, 1));
    }
}
