using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisLabel : MonoBehaviour
{
    // Unity Members
    [SerializeField] private GlobalControl m_GlobalControl;
    [SerializeField] private TMPro.TMP_Text m_TmpText;
    [SerializeField] private string m_ComplexText;
    [SerializeField] private string m_QuaternionText;
    [SerializeField] private string m_VectorText;

    // Unity Methods
    void Update()
    {
        switch(m_GlobalControl.State.Notation)
        {
            case Notation.Complex:
                m_TmpText.text = m_ComplexText;
                break;
            case Notation.Quaternion:
                m_TmpText.text = m_QuaternionText;
                break;
            case Notation.Vector:
            case Notation.AngleAxisRad:
            case Notation.AngleAxisDeg:
                m_TmpText.text = m_VectorText;
                break;
        }
    }
}
