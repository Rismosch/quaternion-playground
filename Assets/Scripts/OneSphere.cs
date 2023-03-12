using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class OneSphere : MonoBehaviour
{
    public string Name => nameof(OneSphere);

    // Unity Serialized Fields
    public Image CoordinateSystem;
    public Image Point;
    public Image ProjectionX;
    public Image ProjectionY;
    public float AnimationSpeed = 1f;
    public Color North;
    public Color South;

    public GraphicRaycaster Raycaster;
    public EventSystem EventSystem;

    // Properties
    public Vector2 Position { get; set; } = new Vector2(Mathf.Sqrt(0.5f), Mathf.Sqrt(0.5f));
    public bool Animate { get; set; }

    // Members
    private bool m_IsDraggingPosition;

    // Update is called once per frame
    void Update()
    {
        if (Animate)
        {
            var dot = Vector2.Dot(Position, Vector2.right);
            var angle = Mathf.Atan2(Position.y, Position.x);
            angle += AnimationSpeed * Time.deltaTime;

            var newX = Mathf.Cos(angle);
            var newY = Mathf.Sin(angle);
            Position = new Vector2(newX, newY);
        }

        CoordinateSystem.material.SetVector("_Position", new Vector4(Position.x, Position.y, 0, 0));
        Point.material.SetVector("_Position", new Vector4(Position.x, Position.y, 0, 0));
        ProjectionX.material.SetVector("_Position", new Vector4(Position.x, 0, 0, 0));
        ProjectionY.material.SetVector("_Position", new Vector4(0, Position.y, 0, 0));

        if (Position.y < 0){
            ProjectionX.material.SetColor("_Color", South);
        } else {
            ProjectionX.material.SetColor("_Color", North);
        }

        var pointerEventData = new PointerEventData(EventSystem);
        pointerEventData.position = Input.mousePosition;
        var results = new List<RaycastResult>();
        Raycaster.Raycast(pointerEventData, results);
        if (results.Count > 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                m_IsDraggingPosition = true;
            }

            if (m_IsDraggingPosition)
            {
                var hit = results[0];
                var imageOriginX = 0f;
                var imageOriginY = 0.5f * (Screen.height - CoordinateSystem.rectTransform.rect.height);
                var imageOrigin = new Vector2(imageOriginX, imageOriginY);
                var imageHitPosition = hit.screenPosition - imageOrigin;

                var width = Point.rectTransform.rect.width;
                var normalizedPosition = 2f * imageHitPosition / width - Vector2.one;

                var angle = Mathf.Atan2(normalizedPosition.y, normalizedPosition.x);
                var newX = Mathf.Cos(angle);
                var newY = Mathf.Sin(angle);
                Position = new Vector2(newX, newY);
            }
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            m_IsDraggingPosition = false;
        }
    }
}
