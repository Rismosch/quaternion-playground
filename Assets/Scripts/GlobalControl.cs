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
        var sphereChanged = false;
        switch(sphereDropdown.captionText.text)
        {
            case "2D":
                if (State.Sphere != Sphere.One)
                {
                    State.Sphere = Sphere.One;
                    sphereChanged = true;
                }
                break;
            case "3D":
                if (State.Sphere != Sphere.Two)
                {
                    State.Sphere = Sphere.Two;
                    sphereChanged = true;
                }
                break;
            case "4D":
                if (State.Sphere != Sphere.Three)
                {
                    State.Sphere = Sphere.Three;
                    sphereChanged = true;
                }
                break;
        }

        if (sphereChanged)
        {
            var index = 0;
            switch(notationDropdown.captionText.text)
            {
                case "Complex":
                    index = 0;
                    break;
                case "Quaternion":
                    index = 1;
                    break;
                case "Vector":
                    index = 2;
                    break;
                case "Angle Axis RAD":
                    index = 3;
                    break;
                case "Angle Axis DEG":
                    index = 4;
                    break;
            }

            var options = new List<TMPro.TMP_Dropdown.OptionData>();
            options.Add(new TMPro.TMP_Dropdown.OptionData("Complex"));
            options.Add(new TMPro.TMP_Dropdown.OptionData("Quaternion"));
            options.Add(new TMPro.TMP_Dropdown.OptionData("Vector"));
            if (State.Sphere == Sphere.Three)
            {
                options.Add(new TMPro.TMP_Dropdown.OptionData("Angle Axis RAD"));
                options.Add(new TMPro.TMP_Dropdown.OptionData("Angle Axis DEG"));
                index = Mathf.Clamp(index, 0, 2);
            }

            notationDropdown.ClearOptions();
            notationDropdown.AddOptions(options);
            notationDropdown.SetValueWithoutNotify(index);
        }

        switch(notationDropdown.captionText.text)
        {
            case "Complex":
                State.Notation = Notation.Complex;
                break;
            case "Quaternion":
                State.Notation = Notation.Quaternion;
                break;
            case "Vector":
                State.Notation = Notation.Vector;
                break;
            case "Angle Axis RAD":
                State.Notation = Notation.AngleAxisRad;
                break;
            case "Angle Axis DEG":
                State.Notation = Notation.AngleAxisDeg;
                break;
        }

        switch(projectionDropdown.captionText.text)
        {
            case "Overlapped":
                State.Projection = Projection.Overlapped;
                break;
            case "Seperated":
                State.Projection = Projection.Seperated;
                break;
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
