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

    // Unity Event Methods
    private void LateUpdate()
    {
        float x = 0.9f * m_GlobalControl.State.OneSpherePosition.x;
        float y = 0.9f * m_GlobalControl.State.OneSpherePosition.y;

        m_X.localPosition = new Vector3(0.5f * x, 0, 0);
        m_LineMaterialX.SetFloat("_Length", y);
    }
}
