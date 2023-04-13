using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomAspectRatioFitter : MonoBehaviour
{
    // Unity Members
    [SerializeField] private float m_Ratio;
    [SerializeField] private float m_Debug;

    // Members
    private RectTransform m_MyTransform;

    // Unity Event Methods
    private void Awake()
    {
        m_MyTransform = (RectTransform)this.transform;
    }

    private void Update()
    {
        var screenRatio = (float)Screen.width / (float)Screen.height;
        m_Debug = screenRatio;
        float width;
        float height;
        if (screenRatio > m_Ratio)
        {
            height = Screen.height;
            width = m_Ratio * height;
        }
        else
        {
            width = Screen.width;
            height = width / m_Ratio;
        }

        m_MyTransform.sizeDelta = new Vector2(width, height);
    }
}
