using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineSatellite : Satellite, ITileHit
{
    private List<TileColor> inputColours;
    private LightHitInfo[] LightPositions;
    private LineRenderer lineRenderer;
    public TileColor outputColour;

    private void Start()
    {
        creator.PrePathUpdate += ClearColours;
        creator.PathUpdated += RefreshingPaths;

        ClearColours();
    }

    void ClearColours()
    {
        inputColours = new List<TileColor>();
    }

    void RefreshingPaths()
    {
        LightPositions = creator.LightBouncePositions(arrayPosition, ReflectDirection, outputColour);

        if (lineRenderer == null)
        {
            lineRenderer = transform.GetChild(0).GetComponent<LineRenderer>();
        }

        if (lineRenderer != null)
        {
            Vector3[] tempArray = new Vector3[LightPositions.Length];

            if(bInstantBeam)
            {
                lineRenderer.positionCount = tempArray.Length;
            }
            
            for (int i = 0; i < tempArray.Length; i++)
            {
                if(bInstantBeam)
                {
                    lineRenderer.SetPosition(i, (Vector2)LightPositions[i].lightPosition);


                    if(LightPositions[i].hitTile != null)
                    {
                        LightPositions[i].hitTile.TileHit(LightPositions[i].lightPosition - LightPositions[i - 1].lightPosition, outputColour);
                    }
                }
                else
                {
                    tempArray[i] = (Vector2)LightPositions[i].lightPosition;
                }
            }

            if(!bInstantBeam)
            {
                StartCoroutine(EmitLight(tempArray));
            }
        }
    }

    IEnumerator EmitLight(Vector3[] endPositionArray)
    {
        for (int index = 1; index < endPositionArray.Length; index++)
        {
            //Set up the Line Renderer
            lineRenderer.positionCount = index + 1;
            lineRenderer.SetPosition(index - 1, endPositionArray[index - 1]);

            //Setting up end point for its starting position
            Vector3 endPoint = endPositionArray[index - 1];

            Vector3 vecToEndPoint = endPoint - endPositionArray[index - 1],
                vecToCurrentIndex = endPositionArray[index] - endPoint;

            //While the end of the light is closer than the next point lets keep pushing it forward
            while (vecToEndPoint.magnitude < vecToCurrentIndex.magnitude)
            {
                endPoint += vecToCurrentIndex.normalized * Time.deltaTime * moveRate;
                vecToEndPoint = endPoint - endPositionArray[index - 1];
                //Update the LineRenderer
                lineRenderer.SetPosition(index, endPoint);
                yield return null;
            }

            //Say we have hit our end point
            if(LightPositions[index].hitTile != null)
            {
                LightPositions[index].hitTile.TileHit(Utility.NormalizeVec2Int(LightPositions[index].lightPosition - LightPositions[index - 1].lightPosition), outputColour);
            }
        }
    }

    public void TileHit(Vector2Int HitDirection, TileColor newColour)
    {
        inputColours.Add(newColour);

        //Update Output Colour
        outputColour = TileColor.CombineColors(inputColours.ToArray());
        

        if(lineRenderer != null)
        {
            lineRenderer.material.color = outputColour.ToColour();
        }

        RefreshingPaths();
    }

    public override void AssignNewTile(Vector2Int arrayPosition, SquareGridCreator creator, Color tileColour)
    {
        base.AssignNewTile(arrayPosition, creator, new Color(1.0f, 0.5f, 0f));

        tileType = TileTypes.CombineSatellite;

        GameObject referenceObj = (GameObject)Resources.Load("Light");
        GameObject tempLine = Instantiate(referenceObj, transform);
        LineRenderer tempRender = referenceObj.GetComponent<LineRenderer>();

        lineRenderer = tempLine.GetComponent<LineRenderer>();
    }

    public override void RemoveTile()
    {
        base.RemoveTile();

        if(lineRenderer != null)
        {
            DestroyImmediate(lineRenderer.gameObject);
            lineRenderer = null;
        }

        creator.PathUpdated -= RefreshingPaths;
        creator.PrePathUpdate -= ClearColours;
    }
    //Combines the Colours that are inputted into it


    //Gets Told a Colour is hitting it


    //Hook onto PrePathUpdate to clear its path    
}
