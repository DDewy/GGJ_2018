using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidTile : BaseTile
{
    public override void AssignNewTile(Vector2Int arrayPosition, SquareGridCreator creator)
    {
        base.AssignNewTile(arrayPosition, creator);

        SpriteRenderer sprite = GetComponent<SpriteRenderer>();

        if (sprite != null)
        {
            sprite.color = Color.magenta;
        }

        tileType = TileTypes.Asteroid;
    }
}
