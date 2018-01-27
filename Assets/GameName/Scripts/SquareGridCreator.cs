using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareGridCreator : MonoBehaviour {

    public static SquareGridCreator instance;

    public int Width, Height;
    public Vector2Int WorldOffset;
    public GameObject SquareRef;
    public BaseTile[][] GridArray;

    private void Start()
    {
        if (instance == null || instance == this)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }

        if(GridArray == null)
        {
            GridArray = new BaseTile[Width][];

            for(int i = 0; i < Width; i++)
            {
                GridArray[i] = new BaseTile[Height];
            }

            for(int i = 0; i < transform.childCount; i++)
            {
                BaseTile tempTile = transform.GetChild(i).GetComponent<BaseTile>();

                GridArray[tempTile.arrayPosition.x][tempTile.arrayPosition.y] = tempTile;
            }
        }
    }


    public Vector2Int[] LightBouncePositions(Vector2Int startPosition, Vector2Int aimingDirection)
    {
        BaseTile NextPostion = GridArray[startPosition.x][startPosition.y];
        List<Vector2Int> ReflectPositions = new List<Vector2Int>();
        Vector2Int LastKnownHeading = aimingDirection;
        Vector2Int nullPosition = new Vector2Int(-1, -1);

        //Set First Position
        ReflectPositions.Add(startPosition + WorldOffset);

        while (NextPostion != null)
        {
            //Look into the next Position
            Vector2Int tempPosition = nullPosition;
            
            //Check if this Position is a Satalite
            ReflectSatellite satalite = NextPostion.GetComponent<ReflectSatellite>();
            if(satalite != null)
            {
                //This is a satalite, this will update our heading
                LastKnownHeading = satalite.ReflectDirection;
                ReflectPositions.Add(NextPostion.arrayPosition + WorldOffset);
            }
            else
            {
                //Check if it is a Target Goal
                if(NextPostion.GetComponent<TargetTile>() != null)
                {
                    //TODO Check if the Color is the right Colour for this target
                    ReflectPositions.Add(NextPostion.arrayPosition + WorldOffset);
                    NextPostion = null;
                    break;
                }
            }

            //Basic Tile, Keep Heading in Direction and Till we reach the end
            tempPosition = NextPostion.arrayPosition + LastKnownHeading;

            //Checking this is within the size of the array, and has been assigned
            if (tempPosition.x < Width && tempPosition.x >= 0 && tempPosition.y < Height && tempPosition.y >= 0)
            {
                NextPostion = GridArray[tempPosition.x][tempPosition.y];
            }
            else
            {
                ReflectPositions.Add(NextPostion.arrayPosition + WorldOffset);
                NextPostion = null;
                break;
            }

        }

        return ReflectPositions.ToArray();
    }
}
