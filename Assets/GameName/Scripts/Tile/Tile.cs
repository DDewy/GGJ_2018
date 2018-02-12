using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : BaseTile
{
    

    protected void Start()
    {
        
    }

    public override void AssignNewTile(Vector2Int arrayPosition, SquareGridCreator creator, Color tileColour)
    {
        base.AssignNewTile(arrayPosition, creator, Color.black);

        tileType = TileTypes.Tile;
    }



}
