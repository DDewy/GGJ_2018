using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : BaseTile
{
    

    protected void Start()
    {
        
    }

    public override void AssignNewTile(Vector2Int arrayPosition, SquareGridCreator creator)
    {
        base.AssignNewTile(arrayPosition, creator);

        SpriteRenderer sprite = GetComponent<SpriteRenderer>();

        if (sprite != null)
        {
            sprite.color = Color.black;
        }

        tileType = TileTypes.Tile;
    }



}
