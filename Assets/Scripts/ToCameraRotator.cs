using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToCameraRotator : MonoBehaviour
{
    // Unity Members
    [SerializeField] private Transform m_Camera;

    // Unity Methods
    void LateUpdate()
    {
        this.transform.localRotation = m_Camera.rotation;
    }
}
