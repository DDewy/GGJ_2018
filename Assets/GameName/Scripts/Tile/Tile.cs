using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : BaseTile
{
    

    protected void Start()
    {
        
    }

    public override void AssignNewTile(Vector2Int arrayPosition)
    {
        base.AssignNewTile(arrayPosition);

        SpriteRenderer sprite = GetComponent<SpriteRenderer>();

        if (sprite != null)
        {
            sprite.color = Color.black;
        }
    }



}
