using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRig : MonoBehaviour
{
    // Unity Members
    [SerializeField] public bool CanRotate;
    [SerializeField] public float HorizontalAngle = -Mathf.PI / 4;
    [SerializeField] public float VerticalAngle = -Mathf.PI / 4;
    [SerializeField] public GameObject Sphere;

    // Unity Methods
    private void Update()
    {
        if (!CanRotate)
        {
            return;
        }

        var rotation1 = Quaternion.AngleAxis(HorizontalAngle * Mathf.Rad2Deg, Vector3.up);
        var rotation2 = Quaternion.AngleAxis(-1f * VerticalAngle * Mathf.Rad2Deg, Vector3.right);

        this.transform.localRotation = rotation1 * rotation2;
    }
}
