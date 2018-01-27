using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseTile : MonoBehaviour
{
    public Vector2Int arrayPosition;
    public TileTypes tileType = TileTypes.NULL;

    public virtual void AssignNewTile(Vector2Int arrayPosition)
    {
        this.arrayPosition = arrayPosition;
    }

    public enum TileTypes
    {
        NULL,
        Tile,
        Satalite,
        LightOutput,
        LightTarget,
        Asteroid
    }
}
