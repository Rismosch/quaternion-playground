using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuaternionNotation : MonoBehaviour
{
    // Unity Members
    [SerializeField] private GlobalControl m_GlobalControl;
    [SerializeField] private TMPro.TMP_Text m_TmpText;

    // Update is called once per frame
    void Update()
    {
        m_TmpText.enabled = m_GlobalControl.State.Notation == Notation.Quaternion;

        switch(m_GlobalControl.State.Sphere)
        {
            case Sphere.One:
                m_TmpText.text = "<mspace=0.75em>        q(     ,       )</mspace>";
                break;
            case Sphere.Two:
                m_TmpText.text = "<mspace=0.75em>        q(     ,       ,       )</mspace>";
                break;
            case Sphere.Three:
                m_TmpText.text = "<mspace=0.75em>q(     ,       ,       ,       )</mspace>";
                break;
        }
    }
}
