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
                GameObject childRef = transform.GetChild(i).gameObject;
                BaseTile tempTile = childRef.GetComponent<BaseTile>();

                if(tempTile != null)
                {
                    GridArray[tempTile.arrayPosition.x][tempTile.arrayPosition.y] = tempTile;
                }
                else
                {
                    Debug.LogError("NULL Grid found at child: " + i, childRef);

                    #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
                    #else
                    //TODO, BACK TO MAIN MENU- Reload Scene
                    Application.Quit();
                    #endif
                }
                
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
            
            if(NextPostion.tileType == BaseTile.TileTypes.Satalite)
            {
                //This is a satalite, this will update our heading
                LastKnownHeading = NextPostion.GetComponent<ReflectSatellite>().ReflectDirection;
                ReflectPositions.Add(NextPostion.arrayPosition + WorldOffset);
            }
            else if(NextPostion.tileType == BaseTile.TileTypes.LightTarget)
            {
                //TODO Check if the Color is the right Colour for this target
                ReflectPositions.Add(NextPostion.arrayPosition + WorldOffset);
                NextPostion = null;
                break;
            }
            else if(NextPostion.tileType == BaseTile.TileTypes.Asteroid)
            {
                //End Here, Hit an Asteroid
                ReflectPositions.Add(NextPostion.arrayPosition + WorldOffset);
                NextPostion = null;
                break;
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

    public void ClearGrid()
    {
        BaseTile[][] TempArray = GridArray;

        if (TempArray == null)
        {
            while (transform.childCount > 0)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }
            return;
        }


        for (int i = 0; i < TempArray.Length; i++)
        {
            for (int p = 0; p < TempArray[i].Length; p++)
            {
                DestroyImmediate(TempArray[i][p].gameObject);
                TempArray[i][p] = null;
            }
        }

        GridArray = null;
    }
}
