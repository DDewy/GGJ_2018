using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidTile : BaseTile
{
    public override void AssignNewTile(Vector2Int arrayPosition, SquareGridCreator creator, Color tileColour)
    {
        base.AssignNewTile(arrayPosition, creator, Color.magenta);

        tileType = TileTypes.Asteroid;

        GameObject refPrefab = (GameObject)Resources.Load("AsteroidPrefab");
        Instantiate(refPrefab, transform);
    }
}
