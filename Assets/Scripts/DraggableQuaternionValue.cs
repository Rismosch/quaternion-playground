using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//                00000000111111112222222233333333
// <mspace=0.75em> + 1.00 + 0.00i + 0.00j + 0.00k</mspace>
// <mspace=0.75em>q(+1.00,  +0.00,  +0.00,  +0.00)</mspace>
// <mspace=0.75em>w=+1.00 x=+0.00 y=+0.00 z=+0.00</mspace>
// <mspace=0.75em>θ=+1.00 x=+0.00 y=+0.00 z=+0.00</mspace>

public class DraggableQuaternionValue : MonoBehaviour
{
    public enum Sample {
        q0,
        q1,
        q2,
        q3
    }

    // Unity Members
    [SerializeField] private GlobalControl m_GlobalControl;
    [SerializeField] private Sample ValueSample;
    [SerializeField] private TMPro.TMP_Text m_TmpText;

    // Properties
    public bool IsPointerOver { get; set; }

    // Members
    private Vector3 m_StartPosition;

    // Public Methods
    public void ManualUpdate()
    {
        // Handle PointerOver and Drag
        if (IsPointerOver && Input.GetKeyDown(KeyCode.Mouse0))
        {
            m_GlobalControl.CurrentlyDragging = this;
            m_StartPosition = Input.mousePosition;
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
            var delta = Input.mousePosition - m_StartPosition;

            Debug.Log(delta);
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
                    isVisible = ValueSample == Sample.q1 || ValueSample == Sample.q2;
                    break;

                case Sphere.Two:
                    isVisible = ValueSample == Sample.q1 || ValueSample == Sample.q2 || ValueSample == Sample.q3;
                    break;

                case Sphere.Three:
                default:
                    isVisible = true;
                    break;
            }
        }

        gameObject.SetActive(isVisible);

        // Handle DisplayText
        switch(m_GlobalControl.State.Notation)
        {
            case Notation.Complex:
                switch(ValueSample)
                {
                    case Sample.q0:
                        break;
                    case Sample.q1:
                        break;
                    case Sample.q2:
                        break;
                    case Sample.q3:
                        break;
                }
                break;

            case Notation.Quaternion:
                switch(ValueSample)
                {
                    case Sample.q0:
                        break;
                    case Sample.q1:
                        break;
                    case Sample.q2:
                        break;
                    case Sample.q3:
                        break;
                }
                break;

            case Notation.Vector:
                switch(ValueSample)
                {
                    case Sample.q0:
                        break;
                    case Sample.q1:
                        break;
                    case Sample.q2:
                        break;
                    case Sample.q3:
                        break;
                }
                break;

            case Notation.AngleAxisRad:
                switch(ValueSample)
                {
                    case Sample.q0:
                        break;
                    case Sample.q1:
                        break;
                    case Sample.q2:
                        break;
                    case Sample.q3:
                        break;
                }
                break;

            case Notation.AngleAxisDeg:
                switch(ValueSample)
                {
                    case Sample.q0:
                        break;
                    case Sample.q1:
                        break;
                    case Sample.q2:
                        break;
                    case Sample.q3:
                        break;
                }
                break;
        }
    }
}
