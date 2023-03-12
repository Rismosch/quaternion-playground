using UnityEngine;
using UnityEditor;

public class GUIWindowOneSphere{
    public void Draw(OneSphere oneSphere){
        var newPosition = EditorGUILayout.Vector2Field("Position", oneSphere.Position);

        if (newPosition.x != oneSphere.Position.x)
        {
            newPosition.x = Mathf.Clamp(newPosition.x, -1, 1f);
            newPosition.y = Mathf.Sqrt(1 - (newPosition.x * newPosition.x));
        } else if (newPosition.y != oneSphere.Position.y)
        {
            newPosition.y = Mathf.Clamp(newPosition.y, -1, 1f);
            newPosition.x = Mathf.Sqrt(1 - (newPosition.y * newPosition.y));
        }

        oneSphere.Position = newPosition;
        
        oneSphere.Animate = EditorGUILayout.Toggle("Animate", oneSphere.Animate);
    }
}