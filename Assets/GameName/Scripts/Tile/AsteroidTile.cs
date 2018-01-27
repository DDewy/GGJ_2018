using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidTile : BaseTile
{

    public override void AssignNewTile(Vector2Int arrayPosition)
    {
        base.AssignNewTile(arrayPosition);

        SpriteRenderer sprite = GetComponent<SpriteRenderer>();

        if (sprite != null)
        {
            sprite.color = Color.magenta;
        }

        tileType = TileTypes.Asteroid;
    }
}
