using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationFrameLineController : MonoBehaviour
{
    // Unity Members
    [SerializeField] private GlobalControl m_GlobalControl;
    [SerializeField] private AxisRotator m_RotationAxis;
    [SerializeField] private AxisRotator m_RotationX;
    [SerializeField] private AxisRotator m_RotationY;
    [SerializeField] private AxisRotator m_RotationZ;

    [SerializeField] private Transform m_Sphere;

    // Unity Event Methods
    void LateUpdate()
    {
        var rotation = new Quaternion(
            m_GlobalControl.State.ThreeSpherePosition.x,
            m_GlobalControl.State.ThreeSpherePosition.y,
            m_GlobalControl.State.ThreeSpherePosition.z,
            m_GlobalControl.State.ThreeSpherePosition.w
        );

        var rotationAxis = new Vector3(
            m_GlobalControl.State.ThreeSphereAngleAxis.x,
            m_GlobalControl.State.ThreeSphereAngleAxis.y,
            m_GlobalControl.State.ThreeSphereAngleAxis.z
        );

        m_RotationAxis.Right = -1f * rotationAxis;
        m_RotationX.Right = rotation * new Vector3(-1, 0, 0);
        m_RotationY.Right = rotation * new Vector3(0, -1, 0);
        m_RotationZ.Right = rotation * new Vector3(0, 0, -1);
        m_Sphere.localRotation = rotation;
    }
}
