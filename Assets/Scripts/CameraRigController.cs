using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraRigController : MonoBehaviour, IDraggable
{
    // Unity Members
    [SerializeField] private GlobalControl m_GlobalControl;
    [SerializeField] private CameraRig m_OneSphereRig;
    [SerializeField] private CameraRig m_TwoSphereRig;
    [SerializeField] private CameraRig m_ThreeSphereRig;

    [SerializeField] private TMPro.TMP_Text m_TmpText;
    [SerializeField] private string m_OneSphereText;
    [SerializeField] private string m_TwoSphereText;
    [SerializeField] private string m_ThreeSphereText;

    [SerializeField] private RawImage m_RawImage;
    [SerializeField] private RenderTexture m_OneSphereRenderTexture;
    [SerializeField] private RenderTexture m_TwoSphereRenderTexture;
    [SerializeField] private RenderTexture m_ThreeSphereRenderTexture;

    // Properties
    public bool IsPointerOver { get; set; }

    // Members
    private bool m_IsDragging;
    private Vector3 m_PreviousMousePosition;

    // Public Methods
    public void ManualUpdate()
    {
        CameraRig cameraRig = null;

        // Display
        switch(m_GlobalControl.State.Sphere)
        {
            case Sphere.One:
                m_OneSphereRig.Sphere.SetActive(true);
                m_TwoSphereRig.Sphere.SetActive(false);
                m_ThreeSphereRig.Sphere.SetActive(false);
                m_TmpText.text = m_OneSphereText;
                m_RawImage.texture = m_OneSphereRenderTexture;
                cameraRig = m_OneSphereRig;
                break;
            case Sphere.Two:
                m_OneSphereRig.Sphere.SetActive(false);
                m_TwoSphereRig.Sphere.SetActive(true);
                m_ThreeSphereRig.Sphere.SetActive(false);
                m_TmpText.text = m_TwoSphereText;
                m_RawImage.texture = m_TwoSphereRenderTexture;
                cameraRig = m_TwoSphereRig;
                break;
            case Sphere.Three:
                m_OneSphereRig.Sphere.SetActive(false);
                m_TwoSphereRig.Sphere.SetActive(false);
                m_ThreeSphereRig.Sphere.SetActive(true);
                m_TmpText.text = m_ThreeSphereText;
                m_RawImage.texture = m_ThreeSphereRenderTexture;
                cameraRig = m_ThreeSphereRig;
                break;
        }

        // Drag Logic
        if (IsPointerOver && Input.GetMouseButtonDown(1))
        {
            m_IsDragging = true;
            m_PreviousMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(1))
        {
            m_IsDragging = false;
        }

        if (m_IsDragging && cameraRig.CanRotate)
        {
            var delta = Input.mousePosition - m_PreviousMousePosition;
            delta *= 0.03f;

            var horizontalAngle = cameraRig.HorizontalAngle + delta.x;
            while (horizontalAngle < 0)
            {
                horizontalAngle += 2 * Mathf.PI;
            }
            while (horizontalAngle > 2 * Mathf.PI)
            {
                horizontalAngle -= 2 * Mathf.PI;
            }
            cameraRig.HorizontalAngle = horizontalAngle;

            var verticalAngle = cameraRig.VerticalAngle + delta.y;
            verticalAngle = Mathf.Clamp(verticalAngle, -0.5f * Mathf.PI, 0.5f * Mathf.PI);
            cameraRig.VerticalAngle = verticalAngle;

            m_PreviousMousePosition = Input.mousePosition;
        }
    }
}
