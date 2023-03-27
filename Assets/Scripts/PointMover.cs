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

    // Unity Methods
    private void LateUpdate()
    {
        this.transform.position = 0.9f * 0.5f * m_GlobalControl.State.TwoSpherePosition;

        if (m_GlobalControl.State.TwoSpherePosition.z < 0)
        {
            m_PointMaterial.SetColor("_Color", m_SouthColor);
        }
        else
        {
            m_PointMaterial.SetColor("_Color", m_NorthColor);
        }
    }
}
