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

    public virtual void RemoveTile()
    {

    }

    public static void ChangeTo(Tile.TileTypes newType, BaseTile originalTile)
    {
        BaseTile newTile = null;

        switch (newType)
        {
            case Tile.TileTypes.Tile:
                newTile = originalTile.gameObject.AddComponent<Tile>();
                break;

            case Tile.TileTypes.Satalite:
                newTile = originalTile.gameObject.AddComponent<ReflectSatellite>();
                break;

            case Tile.TileTypes.LightOutput:
                newTile = originalTile.gameObject.AddComponent<LightOutput>();
                break;

            case Tile.TileTypes.LightTarget:
                newTile = originalTile.gameObject.AddComponent<TargetTile>();
                break;

            case Tile.TileTypes.Asteroid:
                newTile = originalTile.gameObject.AddComponent<AsteroidTile>();
                break;
        }

        if (newTile == null)
        {
            //report an Error and Back out
            Debug.LogError("Could not Change Tile target");
            return;
        }
        originalTile.RemoveTile();
        newTile.AssignNewTile(originalTile.arrayPosition);

        DestroyImmediate(originalTile);
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
