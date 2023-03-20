using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GlobalControl : MonoBehaviour
{
    // Unity Members
    [SerializeField] private TMPro.TMP_Dropdown sphereDropdown;
    [SerializeField] private TMPro.TMP_Dropdown notationDropdown;
    [SerializeField] private TMPro.TMP_Dropdown projectionDropdown;
    [SerializeField] private GraphicRaycaster m_GraphicRaycaster;
    [SerializeField] private List<DraggableQuaternionValue> m_DraggableQuaternionValues;

    // Properties
    public DraggableQuaternionValue CurrentlyDragging { get; set; } = null;
    public Vector3 PreviousDragPosition { get; set; }
    public State State { get; }= new State();

    // Unity Event Methods
    private void Awake()
    {
        m_GraphicRaycaster = GetComponent<GraphicRaycaster>();
    }

    private void Update()
    {
        // Update UI State
        var newState = new State();

        switch(sphereDropdown.captionText.text)
        {
            case "2D":
                newState.Sphere = Sphere.One;
                break;
            case "3D":
                newState.Sphere = Sphere.Two;
                break;
            case "4D":
                newState.Sphere = Sphere.Three;
                break;
        }

        switch(notationDropdown.captionText.text)
        {
            case "Complex":
                newState.Notation = Notation.Complex;
                break;
            case "Quaternion":
                newState.Notation = Notation.Quaternion;
                break;
            case "Vector":
                newState.Notation = Notation.Vector;
                break;
            case "Angle Axis RAD":
                newState.Notation = Notation.AngleAxisRad;
                break;
            case "Angle Axis DEG":
                newState.Notation = Notation.AngleAxisDeg;
                break;
        }

        switch(projectionDropdown.captionText.text)
        {
            case "Overlapped":
                newState.Projection = Projection.Overlapped;
                break;
            case "Seperated":
                newState.Projection = Projection.Seperated;
                break;
        }

        State.Update(newState, out var changed);
        if (changed)
        {
            Debug.Log($"state changed: {State}");
        }

        // IsPointerOver and Drag
        foreach(var draggableQuaternionValue in m_DraggableQuaternionValues)
        {
            draggableQuaternionValue.IsPointerOver = false;
        }

        var pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, results);
        foreach(var result in results)
        {
            var draggableQuaternionValue = result.gameObject?.GetComponent<DraggableQuaternionValue>();
            if (draggableQuaternionValue == null)
            {
                continue;
            }

            draggableQuaternionValue.IsPointerOver = true;
        }

        if (!Input.GetKey(KeyCode.Mouse0))
        {
            CurrentlyDragging = null;
        }

        // Update DraggableQuaternionValues
        foreach(var draggableQuaternionValue in m_DraggableQuaternionValues)
        {
            draggableQuaternionValue.ManualUpdate();
        }
    }
}
