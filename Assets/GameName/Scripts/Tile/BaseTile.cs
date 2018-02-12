using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseTile : MonoBehaviour
{
    public SquareGridCreator creator;
    [ShowOnly] public Vector2Int arrayPosition;
    public TileTypes tileType = TileTypes.NULL;

    //1 Unit Per second. 1 unit = 1 square
    protected const float moveRate = 5f;
    protected const bool bInstantBeam = false;

    public virtual void AssignNewTile(Vector2Int arrayPosition, SquareGridCreator creator, Color tileColour)
    {
        this.arrayPosition = arrayPosition;
        this.creator = creator;

        creator.SetTile(arrayPosition, this);

        //Assign Tile Colour
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        if (sprite != null)
            sprite.color = tileColour;

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

            case TileTypes.LightGate:
                newTile = originalTile.gameObject.AddComponent<LightGate>();
                break;

            case TileTypes.SatelliteSplitter:
                newTile = originalTile.gameObject.AddComponent<SplitterSatellite>();
                break;
        }

        if (newTile == null)
        {
            //report an Error and Back out
            Debug.LogError("Could not Change Tile target");
            return null;
        }
        originalTile.RemoveTile();
        newTile.AssignNewTile(originalTile.arrayPosition, originalTile.creator, Color.black); //Tile will sort its self out

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
        LightTrigger,
        LightGate,
        SatelliteSplitter
    }
}

public struct LightHitInfo
{
    public Vector2Int lightPosition;
    public ITileHit hitTile;

    /// <summary>
    /// Saves Details and DOES CHECK for ITileHit interfaces
    /// </summary>
    /// <param name="tile"></param>
    /// <param name="WorldOffset"></param>
    public LightHitInfo(BaseTile tile, Vector2Int WorldOffset)
    {
        this.hitTile = tile.GetComponent<ITileHit>();
        this.lightPosition = tile.arrayPosition + WorldOffset;
    }
    /// <summary>
    /// Adds details but DOES NOT CHECK for ITileHit Interface
    /// </summary>
    /// <param name="tilePos"></param>
    /// <param name="WorldOffset"></param>
    public LightHitInfo(Vector2Int tilePos, Vector2Int WorldOffset)
    {
        this.hitTile = null;
        this.lightPosition = tilePos + WorldOffset;
    }
}

[System.Serializable]
public struct TileColor
{
    public bool R, G, B;

    public TileColor(bool Red, bool Green, bool Blue)
    {
        this.R = Red;
        this.G = Green;
        this.B = Blue;
    }

    public TileColor SetColour(bool Red, bool Green, bool Blue)
    {
        this.R = Red;
        this.G = Green;
        this.B = Blue;

        return this;
    }

    public Color ToColour()
    {
        return new Color(R ? 1.0f : 0.0f, G ? 1.0f : 0.0f, B ? 1.0f : 0.0f);
    }

    public override string ToString()
    {
        return (R ? "1" : "0") + (G ? "1" : "0") + (B ? "1" : "0");
    }

    public static TileColor ReadString(string inputValue)
    {
        TileColor outputVariable = new TileColor();

        if (inputValue.Length != 3)
        {
            Debug.LogError("Can't read string, String need to be 3 Characters long with Using 0 or 1 to describe RGB values");
            return outputVariable;
        }

        outputVariable.R = inputValue[0] == 1;
        outputVariable.G = inputValue[1] == 1;
        outputVariable.B = inputValue[2] == 1;

        return outputVariable;
    }


    public static bool ExactSameColour(TileColor A, TileColor B)
    {
        return A.R == B.R && A.G == B.G == A.B == B.B;
    }

    public static bool ContainColour(TileColor IncomingColour, TileColor FilterColour)
    {
        return (FilterColour.R == true && FilterColour.R == IncomingColour.R) || (FilterColour.G == true && FilterColour.G == IncomingColour.G) || (FilterColour.B == true && FilterColour.B == IncomingColour.B);

        //if (FilterColour.R == true && FilterColour.R == IncomingColour.R) If filter contains Red and Filter and Incoming colour are the same, then it incoming colour does contain Red
        //    return true;

        //if (FilterColour.G == true && FilterColour.G == IncomingColour.G) If filter contains Green and Filter and Incoming colour are the same, then it incoming colour does contain Green
        //    return true;

        //if (FilterColour.B == true && FilterColour.B == IncomingColour.B) If filter contains Blue and Filter and Incoming colour are the same, then it incoming colour does contain Blue
        //    return true;
    }

    public static bool operator ==(TileColor A, TileColor B)
    {
        return ExactSameColour(A, B);
    }

    public static bool operator !=(TileColor A, TileColor B)
    {
        return !ExactSameColour(A, B);
    }

    public static TileColor CombineColors(TileColor[] colors)
    {
        bool red = false, green = false, blue = false;

        for(int i = 0; i < colors.Length; i++)
        {
            if (colors[i].R)
                red = true;

            if (colors[i].G)
                green = true;

            if (colors[i].B)
                blue = true;

            if(red && green && blue)
            {
                break;
            }
        }
        return new TileColor(red, green, blue);
    }
}