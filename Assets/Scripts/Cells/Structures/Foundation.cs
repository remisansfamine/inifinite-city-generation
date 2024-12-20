using Unity.VisualScripting;
using UnityEngine;

public class Foundation : CellStructure
{
    [SerializeField] private Vector3 maxDimensions = Vector3.one;

    Bounds[] bounds;

    private const float TOP_HEIGHT_SEED = 1987.568f;
    private const float BOT_HEIGHT_SEED =-7549.985f;

    //  NOTE : upgrade noise seed ?
    float Noise(float noiseResolution, float seed = 0)
    {
        float noiseScale = 1f / noiseResolution;
        return Mathf.PerlinNoise(seed + transform.position.x * noiseScale, seed + transform.position.z * noiseScale);
    }

    public override void Generate()
    {
        float topHeight = Noise(1, TOP_HEIGHT_SEED);
        float botHeight = Noise(1, BOT_HEIGHT_SEED);

        if (topHeight + botHeight > 1f)
        {
            bounds = new[] {
                new Bounds(Vector3.zero, maxDimensions)
            };
            return;
        }


        Vector3 bottomDimensions = maxDimensions;
        bottomDimensions.y *= botHeight;

        Vector3 topDimensions = maxDimensions;
        topDimensions.y *= topHeight;


        bounds = new[] {
                new Bounds(Vector3.up * (-maxDimensions.y + bottomDimensions.y) * 0.5f , bottomDimensions),
                new Bounds(Vector3.up * (maxDimensions.y - topDimensions.y) * 0.5f , topDimensions)
            };
    }


    private void DrawShape(Vector3 offset, Vector3 dimensions)
    {
        Gizmos.color = new Color(0, 0.75f, 0.75f, 0.25f);
        Gizmos.DrawCube(transform.position + offset, dimensions);

        Gizmos.color = new Color(0, 0.75f, 0.75f, 1f);
        Gizmos.DrawWireCube(transform.position + offset, dimensions);
    }

    private void OnDrawGizmos()
    {
        if (bounds == null)
            return;

        foreach (Bounds bound in bounds)
        {
            DrawShape(bound.center, bound.size);
        }
    }

}