using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageDragController : MonoBehaviour, IDraggable
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

    [SerializeField] private PointMover m_OneSpherePointMover;
    [SerializeField] private PointMover m_TwoSpherePointMover;
    [SerializeField] private PointMover m_ThreeSpherePointMover;

    // Properties
    public bool IsPointerOver { get; set; }

    // Members
    private bool m_IsDraggingRight;
    private bool m_IsDraggingLeft;
    private Vector3 m_PreviousMousePosition;

    // Public Methods
    public void ManualUpdate()
    {
        CameraRig cameraRig = null;

        // Display CameraRig and Text
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

        // Drag Start Logic
        if (!m_IsDraggingRight && IsPointerOver && Input.GetMouseButtonDown(0))
        {
            m_IsDraggingLeft = true;
            m_PreviousMousePosition = Input.mousePosition;
        }

        if (!m_IsDraggingLeft && IsPointerOver && Input.GetMouseButtonDown(1))
        {
            m_IsDraggingRight = true;
            m_PreviousMousePosition = Input.mousePosition;
        }

        // Drag End Logic
        if (Input.GetMouseButtonUp(0))
        {
            m_IsDraggingLeft = false;
        }
        
        if (Input.GetMouseButtonUp(1))
        {
            m_IsDraggingRight = false;
        }

        // Drag Point Logic
        if (m_IsDraggingLeft)
        {
            var rectTransform = (RectTransform)this.transform;
            var rect = rectTransform.rect;
            var position = Input.mousePosition - rectTransform.position;

            // Orthographic camera has size of 0.6f. The image has a width of 1f, so in the unit
            // of camerasize, the camera has a size of 0.5f. To undo this ratio, we multiply by
            // (0.6f / 0.5f).
            // 
            // Since every UI shader I wrote has a padding of 0.9f, we need to reverse this aswell.
            // We do this by multiplying the inverse of 0.9f: (1f / 0.9f).
            // 
            // These two numbers convert local screen space coordinates to local normalized coordinates,
            // where the circle has a radius of 0.5f units. Because we want this radius to be 1, we
            // finally multiply by 2.
            // 
            // The result should be exactly 2.66...
            const float magic = (0.6f / 0.5f) * (1f / 0.9f) * 2;
            var normalizedPosition = new Vector2(
                magic * position.x / rect.width,
                magic * position.y / rect.height
            );

            switch(m_GlobalControl.State.Sphere)
            {
                case Sphere.One:
                    m_OneSpherePointMover?.Drag(normalizedPosition);
                    break;
                case Sphere.Two:
                    m_TwoSpherePointMover?.Drag(normalizedPosition);
                    break;
                case Sphere.Three:
                    m_ThreeSpherePointMover?.Drag(normalizedPosition);
                    break;
            }
        }

        // Drag CameraRig Logic
        if (m_IsDraggingRight && cameraRig.CanRotate)
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
