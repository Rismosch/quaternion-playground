using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OneSphere : MonoBehaviour
{
    public string Name => nameof(OneSphere);

    public Image Point;
    public Image ProjectionX;
    public Image ProjectionY;
    public Material CoordinateMaterial;
    public float AnimationSpeed = 1f;
    public Color North;
    public Color South;

    public Vector2 Position { get; set; } = Vector2.right;
    public bool Animate { get; set; }

    // Update is called once per frame
    void Update()
    {
        if (Animate)
        {
            var dot = Vector2.Dot(Position, Vector2.right);
            var angle = Mathf.Atan2(Position.y, Position.x);
            angle += AnimationSpeed * Time.deltaTime;

            var newX = Mathf.Cos(angle);
            var newY = Mathf.Sin(angle);

            Position = new Vector2(newX, newY);
        }

        Point.material.SetVector("_Position", new Vector4(Position.x, Position.y, 0, 0));
        ProjectionX.material.SetVector("_Position", new Vector4(Position.x, 0, 0, 0));
        ProjectionY.material.SetVector("_Position", new Vector4(0, Position.y, 0, 0));
        CoordinateMaterial.SetVector("_Position", new Vector4(Position.x, Position.y, 0, 0));

        if (Position.y < 0){
            ProjectionX.material.SetColor("_Color", South);
        } else {
            ProjectionX.material.SetColor("_Color", North);
        }
    }
}
