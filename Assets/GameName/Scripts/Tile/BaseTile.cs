using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseTile : MonoBehaviour
{
    public SquareGridCreator creator;
    public Vector2Int arrayPosition;
    public TileTypes tileType = TileTypes.NULL;

    public virtual void AssignNewTile(Vector2Int arrayPosition, SquareGridCreator creator)
    {
        this.arrayPosition = arrayPosition;
        this.creator = creator;

        creator.SetTile(arrayPosition, this);
    }

    public virtual void RemoveTile()
    {

    }

    /// <summary>
    /// Function that can change the type of the tile and replace its self in the grid array
    /// </summary>
    /// <param name="newType">New Type to switch to</param>
    /// <param name="originalTile">The Tile to switch from</param>
    /// <returns>Reference to the new Tile</returns>
    public static BaseTile ChangeTo(Tile.TileTypes newType, BaseTile originalTile)
    {
        BaseTile newTile = null;

        switch (newType)
        {
            case TileTypes.Tile:
                newTile = originalTile.gameObject.AddComponent<Tile>();
                break;

            case TileTypes.Satalite:
                newTile = originalTile.gameObject.AddComponent<ReflectSatellite>();
                break;

            case TileTypes.RotateSatellite:
                newTile = originalTile.gameObject.AddComponent<RotateableSatellite>();
                break;

            case TileTypes.CombineSatellite:
                newTile = originalTile.gameObject.AddComponent<CombineSatellite>();
                break;

            case TileTypes.LightOutput:
                newTile = originalTile.gameObject.AddComponent<LightOutput>();
                break;

            case TileTypes.LightTarget:
                newTile = originalTile.gameObject.AddComponent<TargetTile>();
                break;

            case TileTypes.Asteroid:
                newTile = originalTile.gameObject.AddComponent<AsteroidTile>();
                break;
        }

        if (newTile == null)
        {
            //report an Error and Back out
            Debug.LogError("Could not Change Tile target");
            return null;
        }
        originalTile.RemoveTile();
        newTile.AssignNewTile(originalTile.arrayPosition, originalTile.creator);

        DestroyImmediate(originalTile);
        return newTile;
    }

    public enum TileTypes
    {
        NULL,
        Tile,
        Satalite,
        LightOutput,
        LightTarget,
        Asteroid,
        RotateSatellite,
        CombineSatellite
    }
}
