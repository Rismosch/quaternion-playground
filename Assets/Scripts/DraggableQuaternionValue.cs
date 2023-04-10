using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableQuaternionValue : MonoBehaviour, IDraggable
{
    // Unity Members
    [SerializeField] private GlobalControl m_GlobalControl;
    [SerializeField] private QuaternionValue Value;
    [SerializeField] private TMPro.TMP_Text m_TmpText;

    // Properties
    public bool IsPointerOver { get; set; }

    // Public Methods
    public void ManualUpdate()
    {
        // ScrollWheel
        var mouseScrollDelta = Input.mouseScrollDelta;
        if (IsPointerOver && mouseScrollDelta != Vector2.zero)
        {
            mouseScrollDelta.y *= 0.1f;

            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            {
                mouseScrollDelta.y *= 0.1f;
            }

            m_GlobalControl.State.Drag(mouseScrollDelta.y, Value);
        }

        // Drag
        if (IsPointerOver && Input.GetKeyDown(KeyCode.Mouse0))
        {
            m_GlobalControl.CurrentlyDragging = this;
            m_GlobalControl.PreviousDragPosition = Input.mousePosition;
        }

        var isDraggingMe = m_GlobalControl.CurrentlyDragging == this;
        var isDragging = m_GlobalControl.CurrentlyDragging != null;
        var isHoveringMe = !isDragging && IsPointerOver;
        if (isDraggingMe || isHoveringMe)
        {
            m_TmpText.fontStyle = TMPro.FontStyles.Bold;
        } else
        {
            m_TmpText.fontStyle = TMPro.FontStyles.Normal;
        }

        if (isDraggingMe)
        {
            var currentPosition = Input.mousePosition;
            var delta = Input.mousePosition - m_GlobalControl.PreviousDragPosition;
            m_GlobalControl.PreviousDragPosition = currentPosition;

            delta *= 0.01f;

            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            {
                delta *= 0.1f;
            }

            m_GlobalControl.State.Drag(delta.y, Value);
        }

        // Reset
        if (IsPointerOver && Input.GetKeyDown(KeyCode.Mouse1))
        {
            m_GlobalControl.State.Reset(Value);
        }

        // Handle Visibility
        var isAngleAxis =
            m_GlobalControl.State.Notation == Notation.AngleAxisRad ||
            m_GlobalControl.State.Notation == Notation.AngleAxisDeg;
        bool isVisible;

        if (isAngleAxis)
        {
            isVisible = true;
        }
        else
        {
            switch(m_GlobalControl.State.Sphere)
            {
                case Sphere.One:
                    isVisible = Value == QuaternionValue.q1 || Value == QuaternionValue.q2;
                    break;

                case Sphere.Two:
                    isVisible = Value == QuaternionValue.q1 || Value == QuaternionValue.q2 || Value == QuaternionValue.q3;
                    break;

                case Sphere.Three:
                default:
                    isVisible = true;
                    break;
            }
        }

        gameObject.SetActive(isVisible);

        // Handle Values
        float q0 = 0;
        float q1 = 0;
        float q2 = 0;
        float q3 = 0;

        switch(m_GlobalControl.State.Sphere)
        {
            case Sphere.One:
                q1 = m_GlobalControl.State.OneSpherePosition.x;
                q2 = m_GlobalControl.State.OneSpherePosition.y;
                break;
            case Sphere.Two:
                q1 = m_GlobalControl.State.TwoSpherePosition.x;
                q2 = m_GlobalControl.State.TwoSpherePosition.y;
                q3 = m_GlobalControl.State.TwoSpherePosition.z;
                break;
            case Sphere.Three:
                if (m_GlobalControl.State.Notation == Notation.AngleAxisRad ||
                    m_GlobalControl.State.Notation == Notation.AngleAxisDeg)
                {
                    q0 = m_GlobalControl.State.ThreeSphereAngleAxis.w;
                    q1 = m_GlobalControl.State.ThreeSphereAngleAxis.x;
                    q2 = m_GlobalControl.State.ThreeSphereAngleAxis.y;
                    q3 = m_GlobalControl.State.ThreeSphereAngleAxis.z;
                }
                else
                {
                    q0 = m_GlobalControl.State.ThreeSpherePosition.w;
                    q1 = m_GlobalControl.State.ThreeSpherePosition.x;
                    q2 = m_GlobalControl.State.ThreeSpherePosition.y;
                    q3 = m_GlobalControl.State.ThreeSpherePosition.z;
                }

                break;
        }

        // Handle DisplayText
        string displayText = "";
        switch(m_GlobalControl.State.Notation)
        {
            case Notation.Complex:
                switch(Value)
                {
                    case QuaternionValue.q0:
                        displayText = $" {Sign(q0, false)} {Format(q0)} ";
                        break;
                    case QuaternionValue.q1:
                        displayText = $"{Sign(q1, m_GlobalControl.State.Sphere == Sphere.Three)} {Format(q1)}{(m_GlobalControl.State.Sphere != Sphere.Three ? ' ' : 'i')} ";
                        break;
                    case QuaternionValue.q2:
                        displayText = $"{Sign(q2, true)} {Format(q2)}{(m_GlobalControl.State.Sphere != Sphere.Three ? 'i' : 'j')} ";
                        break;
                    case QuaternionValue.q3:
                        displayText = $"{Sign(q3, true)} {Format(q3)}{(m_GlobalControl.State.Sphere != Sphere.Three ? 'j' : 'k')} ";
                        break;
                }
                break;

            case Notation.Quaternion:
                switch(Value)
                {
                    case QuaternionValue.q0:
                        displayText = $"  {Sign(q0, false)}{Format(q0)}";
                        break;
                    case QuaternionValue.q1:
                        displayText = $"  {Sign(q1, false)}{Format(q1)}";
                        break;
                    case QuaternionValue.q2:
                        displayText = $"  {Sign(q2, false)}{Format(q2)}";
                        break;
                    case QuaternionValue.q3:
                        displayText = $"  {Sign(q3, false)}{Format(q3)}";
                        break;
                }
                break;

            case Notation.Vector:
                switch(Value)
                {
                    case QuaternionValue.q0:
                        displayText = $"w={Sign(q0, false)}{Format(q0)}";
                        break;
                    case QuaternionValue.q1:
                        displayText = $"x={Sign(q1, false)}{Format(q1)}";
                        break;
                    case QuaternionValue.q2:
                        displayText = $"y={Sign(q2, false)}{Format(q2)}";
                        break;
                    case QuaternionValue.q3:
                        displayText = $"z={Sign(q3, false)}{Format(q3)}";
                        break;
                }
                break;

            case Notation.AngleAxisRad:
            case Notation.AngleAxisDeg:
                switch(Value)
                {
                    case QuaternionValue.q0:
                        string angleToDisplay;
                        if (m_GlobalControl.State.Notation == Notation.AngleAxisDeg)
                        {
                            angleToDisplay = FormatWithoutComma(q0 * Mathf.Rad2Deg);
                        }
                        else
                        {
                            angleToDisplay = Format(q0);
                        }
                        displayText = $"\u03b8={Sign(q0, false)}{angleToDisplay}";
                        break;
                    case QuaternionValue.q1:
                        if (q0 == 0)
                        {
                            displayText = $"x= ???";
                        }
                        else
                        {
                            displayText = $"x={Sign(q1, false)}{Format(q1)}";
                        }
                        break;
                    case QuaternionValue.q2:
                        if (q0 == 0)
                        {
                            displayText = $"y= ???";
                        }
                        else
                        {
                            displayText = $"y={Sign(q2, false)}{Format(q2)}";
                        }
                        break;
                    case QuaternionValue.q3:
                        if (q0 == 0)
                        {
                            displayText = $"z= ???";
                        }
                        else
                        {
                            displayText = $"z={Sign(q3, false)}{Format(q3)}";
                        }
                        break;
                }
                break;
        }

        m_TmpText.text = $"<mspace=0.75em>{displayText}</mspace>";
    }

    // Private Methods
    private char Sign(float value, bool showPlus)
    {
        if (value < 0)
        {
            return '-';
        } else
        {
            if (showPlus)
            {
                return '+';
            }
            else
            {
                return ' ';
            }
        }
    }

    private string Format(float value){
        var scaled = (int)(100 * Mathf.Abs(value));
        var hundred = (scaled / 100) % 10;
        var ten = (scaled / 10) % 10;
        var one = scaled % 10;

        return $"{hundred}.{ten}{one}";
    }

    private string FormatWithoutComma(float value){
        var scaled = (int)Mathf.Abs(value);
        var hundred = (scaled / 100) % 10;
        var ten = (scaled / 10) % 10;
        var one = scaled % 10;

        if (hundred != 0)
        {
            return $"{hundred}{ten}{one}";
        }
        else
        {
            if (ten != 0)
            {
                return $" {ten}{one}";
            }
            else
            {
                return $"  {one}";
            }
        }
    }
}
