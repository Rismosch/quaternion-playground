using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabelScaleRotator : MonoBehaviour
{
    // Unity Members
    [SerializeField] private Transform m_Camera;

    [SerializeField] private GlobalControl m_GlobalControl;
    [SerializeField] private TMPro.TMP_Text m_TmpText;
    [SerializeField] private string m_ComplexText;
    [SerializeField] private string m_QuaternionText;
    [SerializeField] private string m_VectorText;

    [Header("DEBUG")]
    [SerializeField] private float m_Distance;
    [SerializeField] private float m_ScaleFactor = 0.5f;

    // Update is called once per frame
    void Update()
    {
        this.transform.localRotation = m_Camera.rotation;

        var diff = this.transform.position - m_Camera.position;
        m_Distance = diff.magnitude;

        this.transform.localScale = m_ScaleFactor * m_Distance * 0.01f * Vector3.one;

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
