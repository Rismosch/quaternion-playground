using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabelRotator : MonoBehaviour
{
    // Unity Members
    [SerializeField] private Transform m_Camera;

    [SerializeField] private GlobalControl m_GlobalControl;
    [SerializeField] private TMPro.TMP_Text m_TmpText;
    [SerializeField] private string m_ComplexText;
    [SerializeField] private string m_QuaternionText;
    [SerializeField] private string m_VectorText;

    // Update is called once per frame
    void Update()
    {
        this.transform.localRotation = m_Camera.rotation;

        switch(m_GlobalControl.State.Notation)
        {
            case Notation.Complex:
                m_TmpText.text = m_ComplexText;
                break;
            case Notation.Quaternion:
                m_TmpText.text = m_QuaternionText;
                break;
            case Notation.Vector:
                m_TmpText.text = m_VectorText;
                break;
        }
    }
}
