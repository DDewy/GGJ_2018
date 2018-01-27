using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOutput : BaseTile
{
    public Vector2Int[] LightPositions;
    public Vector2Int LightDirection;

    private LineRenderer lineRenderer;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(2.0f);

        LightPositions = SquareGridCreator.instance.LightBouncePositions(arrayPosition, LightDirection);
    }

    private void Update()
    {
        for(int i = 0; i < LightPositions.Length - 1; i++)
        {
            Debug.DrawLine((Vector2)LightPositions[i], (Vector2)LightPositions[i + 1]);
        }
    }

    public override void AssignNewTile(Vector2Int arrayPosition)
    {
        base.AssignNewTile(arrayPosition);

        SpriteRenderer sprite = GetComponent<SpriteRenderer>();

        if (sprite != null)
        {
            sprite.color = Color.red;
        }

        tileType = TileTypes.LightOutput;

        lineRenderer = gameObject.AddComponent<LineRenderer>();

    }

    protected virtual void SetUpLineRenderer(LineRenderer renderer)
    {
        renderer.widthMultiplier = 0.3f;
    }
}
