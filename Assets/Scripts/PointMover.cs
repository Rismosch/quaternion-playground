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
}
