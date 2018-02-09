using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseTile : MonoBehaviour
{
    public SquareGridCreator creator;
    [ShowOnly] public Vector2Int arrayPosition;
    public TileTypes tileType = TileTypes.NULL;

    //1 Unit Per second. 1 unit = 1 square
    protected const float moveRate = 20f;
    protected const bool bInstantBeam = false;

    public virtual void AssignNewTile(Vector2Int arrayPosition, SquareGridCreator creator)
    {
        this.arrayPosition = arrayPosition;
        this.creator = creator;

        creator.SetTile(arrayPosition, this);

        //Assign Tile Colour

        //Assign TileType

        //Any Extra stuff such as additional models
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
                newTile = originalTile.gameObject.AddComponent<Satellite>();
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

            case TileTypes.LightTrigger:
                newTile = originalTile.gameObject.AddComponent<LightTrigger>();
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
        CombineSatellite,
        LightTrigger
    }
}

public struct LightHitInfo
{
    public Vector2Int lightPosition;
    public ITileHit hitTile;

    public LightHitInfo(BaseTile tile, Vector2Int WorldOffset)
    {
        this.hitTile = tile.GetComponent<ITileHit>();
        this.lightPosition = tile.arrayPosition + WorldOffset;
    }

    public LightHitInfo(Vector2Int tilePos, Vector2Int WorldOffset)
    {
        this.hitTile = null;
        this.lightPosition = tilePos + WorldOffset;
    }
}