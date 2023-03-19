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

    // Unity Event Methods
    private void Update()
    {
        if (IsPointerOver)
        {
            m_TmpText.fontStyle = TMPro.FontStyles.Bold;
        }
        else
        {
            m_TmpText.fontStyle = TMPro.FontStyles.Normal;
        }
    }
}
