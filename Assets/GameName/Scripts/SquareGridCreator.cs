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


    public Vector2Int[] LightBouncePositions(Vector2Int startPosition, Vector2Int aimingDirection, Color lightColor)
    {
        if(aimingDirection == Vector2Int.zero)
        {
            //If there is no direction then it can go no where, so lets just stop it here otherwise it will be stuck in a infinite loop
            Vector2Int[] tempArray = { startPosition };
            return tempArray;
        }

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

            switch(NextPostion.tileType)
            {
                case BaseTile.TileTypes.Satalite:
                case BaseTile.TileTypes.RotateSatellite:
                    //This is a satalite, this will update our heading
                    LastKnownHeading = NextPostion.GetComponent<ReflectSatellite>().ReflectDirection;

                    if (ReflectPositions.Contains(NextPostion.arrayPosition + WorldOffset))
                    {
                        //Stop this here, since we are going to end up going in a loop
                        ReflectPositions.Add(NextPostion.arrayPosition + WorldOffset);
                        NextPostion = null;
                        break;
                    }
                    else
                    {
                        ReflectPositions.Add(NextPostion.arrayPosition + WorldOffset);
                    }
                    break;

                case BaseTile.TileTypes.LightTarget:
                    TargetTile tempTarget = NextPostion.GetComponent<TargetTile>();

                    if (lightColor == tempTarget.TargetColour)
                    {
                        tempTarget.HitByLight();
                        ReflectPositions.Add(NextPostion.arrayPosition + WorldOffset);
                        NextPostion = null;
                        break;
                    }
                    else
                    {
                        //Not Reached our Target
                    }
                    break;

                case BaseTile.TileTypes.Asteroid:
                    //End Here, Hit an Asteroid
                    ReflectPositions.Add(NextPostion.arrayPosition + WorldOffset);
                    NextPostion = null;
                    break;

                case BaseTile.TileTypes.CombineSatellite:
                    //Check if we are searching for our selves
                    if (NextPostion.arrayPosition == startPosition)
                        break;
                    
                    //Tell Combine sataellite it has been hit 
                    CombineSatellite tempCombine = NextPostion.GetComponent<CombineSatellite>();
                    tempCombine.HitByColour(lightColor);
                    //End the Light position for this colour
                    ReflectPositions.Add(NextPostion.arrayPosition + WorldOffset);
                    NextPostion = null;
                    break;
            }

            if (NextPostion == null)
                break;

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
