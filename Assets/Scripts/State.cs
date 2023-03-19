using UnityEngine;

public class State
{
    // UI
    public Sphere Sphere = Sphere.One;
    public Notation Notation = Notation.Complex;
    public Projection Projection = Projection.Overlapped;

    public Vector2 OneSpherePosition = new Vector2(1, 0);
    public Vector3 TwoSpherePosition = new Vector3(1, 0, 0);
    public Vector4 ThreeSpherePosition = new Vector4(1, 0, 0, 0);

    public void Update(State other, out bool changed) {
        var result = false;
        void Set<T>(ref T left, T right)
        {
            if (left.Equals(right)) {
                return;
            }

            left = right;
            result = true;
        }

        Set(ref this.Sphere, other.Sphere);
        Set(ref this.Notation, other.Notation);
        Set(ref this.Projection, other.Projection);

        changed = result;
    }

    public override string ToString()
    {
        return $"{{{nameof(Sphere)}: {Sphere}, {nameof(Notation)}: {Notation}, {nameof(Projection)}, {Projection}}}";
    }
}