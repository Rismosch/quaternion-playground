using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisRotator : MonoBehaviour
{
    // Unity Members
    [SerializeField] private Transform m_Camera;

    [SerializeField] private Vector3 m_Right;
    [SerializeField] private bool m_IsY;
    
    // Unity Event Methods
    private void LateUpdate()
    {
        var cameraUp = m_Camera.rotation * Vector3.up;
        var forward = Vector3.Cross(cameraUp, m_Right);
        var upwards = Vector3.Cross(forward, m_Right);

        this.transform.rotation = Quaternion.LookRotation(forward, upwards);

        if (m_IsY)
        {
            this.transform.rotation = Quaternion.AngleAxis((0.5f * Mathf.PI) * Mathf.Rad2Deg, Vector3.up) * this.transform.rotation;
        }
    }
}
