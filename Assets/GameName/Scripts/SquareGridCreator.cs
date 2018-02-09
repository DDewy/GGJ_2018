using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareGridCreator : MonoBehaviour {

    public static SquareGridCreator instance;

    public int Width, Height;
    public Vector2Int WorldOffset;
    public GameObject SquareRef;
    public BaseTile[][] GridArray;
    public TargetTile[] TargetTiles;

    #region DelegateStuff
    public delegate void SquareGridCalls(SquareGridCreator self);
    public event System.Action PathUpdated;
    public event System.Action PrePathUpdate;
    public event SquareGridCalls LevelComplete;

    public void OnPathUpdated()
    {
        //Mainly meant for the light Combiners to clear their Pathing before it is reset
        if(PrePathUpdate != null)
        {
            PrePathUpdate();
        }
        
        if (PathUpdated != null)
        {
            PathUpdated();
        }
    }

    void OnLevelComplete()
    {
        if(LevelComplete != null)
        {
            LevelComplete(this);
        }
    }

    #endregion

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

        GetComponent<LevelEditor>().levelLoaded += FindTargets;
    }

    void FindTargets()
    {
        List<TargetTile> tempTargets = new List<TargetTile>();

        for (int i = 0; i < Width; i++)
        {
            for (int p = 0; p < Height; p++)
            {
                if (GridArray[i][p].tileType == BaseTile.TileTypes.LightTarget)
                {
                    tempTargets.Add(GridArray[i][p].GetComponent<TargetTile>());
                }
            }
        }
    }


    public LightHitInfo[] LightBouncePositions(Vector2Int startPosition, Vector2Int aimingDirection, Color lightColor)
    {
        if (aimingDirection == Vector2Int.zero)
        {
            //If there is no direction then it can go no where, so lets just stop it here otherwise it will be stuck in a infinite loop
            LightHitInfo[] tempArray = { new LightHitInfo(startPosition, Vector2Int.zero) };
            return tempArray;
        }

        BaseTile NextPosition = GridArray[startPosition.x][startPosition.y];
        List<LightHitInfo> ReflectPositions = new List<LightHitInfo>();
        //A Temporary List for checking the satelite Positions hit, so it doesn't get stuck in a loop
        List<Vector2Int> TilesHit = new List<Vector2Int>();
        Vector2Int LastKnownHeading = aimingDirection;
        Vector2Int nullPosition = new Vector2Int(-1, -1);

        //Set First Position
        ReflectPositions.Add(new LightHitInfo(NextPosition, WorldOffset));

        while (NextPosition != null)
        {
            //Look into the next Position
            Vector2Int tempPosition = nullPosition;
            
            //Check if this Position is a Satalite

            switch(NextPosition.tileType)
            {
                case BaseTile.TileTypes.Satalite:
                    //This is a satalite, this will update our heading
                    LastKnownHeading = NextPosition.GetComponent<Satellite>().ReflectDirection;

                    if (TilesHit.Contains(NextPosition.arrayPosition))
                    {
                        //Stop this here, since we are going to end up going in a loop
                        
                        ReflectPositions.Add(new LightHitInfo(NextPosition, WorldOffset));
                        TilesHit.Add(NextPosition.arrayPosition); //Is this needed? 
                        NextPosition = null;
                        break;
                    }
                    else
                    {
                        ReflectPositions.Add(new LightHitInfo(NextPosition, WorldOffset));
                        TilesHit.Add(NextPosition.arrayPosition);
                    }
                    break;

                case BaseTile.TileTypes.LightTarget:
                    TargetTile tempTarget = NextPosition.GetComponent<TargetTile>();

                    if (lightColor == tempTarget.TargetColour)
                    {
                        ReflectPositions.Add(new LightHitInfo(NextPosition, WorldOffset));
                        TilesHit.Add(NextPosition.arrayPosition); //Is this needed? 
                        NextPosition = null;
                        break;
                    }
                    else
                    {
                        //Not Reached our Target
                    }
                    break;

                case BaseTile.TileTypes.Asteroid:
                    //End Here, Hit an Asteroid
                    ReflectPositions.Add(new LightHitInfo(NextPosition, WorldOffset));
                    TilesHit.Add(NextPosition.arrayPosition); //Is this needed? 
                    NextPosition = null;
                    break;

                case BaseTile.TileTypes.CombineSatellite:
                    //Check if we are searching for our selves
                    if (NextPosition.arrayPosition == startPosition)
                        break;
                    
                    //End the Light position for this colour
                    ReflectPositions.Add(new LightHitInfo(NextPosition, WorldOffset));
                    TilesHit.Add(NextPosition.arrayPosition);
                    NextPosition = null;
                    break;

                case BaseTile.TileTypes.LightTrigger:
                    //Not matter what happens, the light beam should end here
                    ReflectPositions.Add(new LightHitInfo(NextPosition, WorldOffset));
                    TilesHit.Add(NextPosition.arrayPosition);
                    NextPosition = null;
                    break;
            }

            if (NextPosition == null)
                break;

            //Basic Tile, Keep Heading in Direction and Till we reach the end
            tempPosition = NextPosition.arrayPosition + LastKnownHeading;

            //Checking this is within the size of the array, and has been assigned
            if (tempPosition.x < Width && tempPosition.x >= 0 && tempPosition.y < Height && tempPosition.y >= 0)
            {
                NextPosition = GridArray[tempPosition.x][tempPosition.y];
            }
            else
            {
                ReflectPositions.Add(new LightHitInfo(NextPosition, WorldOffset)); //Reached End
                NextPosition = null;
                break;
            }
        }
        return ReflectPositions.ToArray();
    }

    void CheckIfComplete()
    {
        //Look Through all targets to see if they are being 
        bool allComplete = true;

        for(int i = 0; i < TargetTiles.Length; i++)
        {
            if(!TargetTiles[i].hasLight)
            {
                allComplete = false;
            }
        }

        OnLevelComplete();
    }

    public void CreateGrid()
    {
        //Initalize Grid
        BaseTile[][] TempGridRef = new BaseTile[Width][];

        for (int i = 0; i < TempGridRef.Length; i++)
        {
            TempGridRef[i] = new BaseTile[Height];
        }

        //Setup the Grid Size
        GridArray = TempGridRef;

        //const int SquareSpacing = 1;
        int xOffset = Width / 2, yOffset = Height / 2;
        WorldOffset = new Vector2Int(-xOffset, -yOffset);

        for (int i = 0; i < TempGridRef.Length; i++)
        {
            for (int p = 0; p < TempGridRef[i].Length; p++)
            {
                GameObject TempTile = Instantiate(SquareRef, transform);
                TempTile.transform.localPosition = new Vector3Int(i - xOffset, p - yOffset, 0);
                TempTile.GetComponent<BaseTile>().AssignNewTile(new Vector2Int(i, p), this);
            }
        }
        Debug.Log("Reached the end of the Creation Array");
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

    public void RenameGrid()
    {
        //Initalize Grid
        BaseTile[][] TempGridRef = new BaseTile[Width][];

        for (int i = 0; i < TempGridRef.Length; i++)
        {
            TempGridRef[i] = new BaseTile[Height];
        }

        for (int i = 0; i < TempGridRef.Length; i++)
        {
            for (int p = 0; p < TempGridRef[i].Length; p++)
            {
                int childNum = (i * Height) + p;

                Transform tileTrans = transform.GetChild(childNum);
                tileTrans.name = "GameSquare(" + i + "," + p + ")" + " num: " + childNum;
            }
        }

        GridArray = TempGridRef;

        Debug.Log("Refreshed Array");
    }

    public void RebuildGrid()
    {
        
    }

    public void SetTile(Vector2Int ArrayPos, BaseTile newTile)
    {
        GridArray[ArrayPos.x] [ArrayPos.y] = newTile;
    }
}
