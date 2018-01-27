using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOutput : BaseTile
{
    public Vector2Int[] LightPositions;
    public Vector2Int LightDirection;

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
    }
}
