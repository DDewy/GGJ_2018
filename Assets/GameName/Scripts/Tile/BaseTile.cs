using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseTile : MonoBehaviour
{
    public Vector2Int arrayPosition;

    public virtual void AssignNewTile(Vector2Int arrayPosition)
    {
        this.arrayPosition = arrayPosition;
    }

    public enum TileTypes
    {
        Tile,
        Satalite,
        LightOutput,
        LightTarget
    }
}
