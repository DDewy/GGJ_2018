using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTile : BaseTile
{
    public override void AssignNewTile(Vector2Int arrayPosition)
    {
        base.AssignNewTile(arrayPosition);

        SpriteRenderer sprite = GetComponent<SpriteRenderer>();

        if (sprite != null)
        {
            sprite.color = Color.green;
        }

        tileType = TileTypes.LightTarget;
    }
}
