using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOutput : BaseTile
{
    public Vector2Int LightDirection;
    public TileColor OutputColour;
   
    //Components
    private LineRenderer lineRenderer;
    private GameObject outputModel;

    //Private Variables
    private LightHitInfo[] LightPositions;

    IEnumerator Start()
    {
        //Update Line Renderer with Output Colour
        if(lineRenderer == null)
        {
            if(transform.childCount > 0)
            {
                lineRenderer = transform.GetChild(0).GetComponent<LineRenderer>();
            }
        }

        if(lineRenderer != null)
        {
            lineRenderer.material.color = OutputColour.ToColour();
        }

        yield return new WaitForSeconds(0.1f);
        creator.PathUpdated += UpdatePath;
        UpdatePath();
    }

    public override void AssignNewTile(Vector2Int arrayPosition, SquareGridCreator creator, Color tileColour)
    {
        base.AssignNewTile(arrayPosition, creator, Color.red);

        tileType = TileTypes.LightOutput;

        GameObject referenceObj = (GameObject)Resources.Load("Light");
        GameObject tempLine = Instantiate(referenceObj, transform);
        LineRenderer tempRender = referenceObj.GetComponent<LineRenderer>();

        GameObject refPrefab = (GameObject)Resources.Load("OutputPrefab");
        outputModel = Instantiate(refPrefab, transform);

        lineRenderer = tempLine.GetComponent<LineRenderer>();
    }
    
    public override void RemoveTile()
    {
        if(lineRenderer != null)
        {
            DestroyImmediate(lineRenderer.gameObject);
            lineRenderer = null;
        }

        if(outputModel != null)
        {
            DestroyImmediate(outputModel);
            outputModel = null;
        }

        creator.PathUpdated -= UpdatePath;
    }

    void UpdatePath()
    {
        LightPositions = creator.LightBouncePositions(arrayPosition, LightDirection, OutputColour);

        if (lineRenderer == null)
        {
            lineRenderer = transform.GetChild(0).GetComponent<LineRenderer>();
        }

        if (lineRenderer != null)
        {
            Vector3[] tempArray = new Vector3[LightPositions.Length];

            if (bInstantBeam)
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
                        LightPositions[i].hitTile.TileHit(Utility.NormalizeVec2Int(LightPositions[i].lightPosition - LightPositions[i - 1].lightPosition), OutputColour);
                    }
                }
                else
                {
                    tempArray[i] = (Vector2)LightPositions[i].lightPosition;
                }
                
            }

            if(!bInstantBeam)
            {
                StartCoroutine(MoveLightBeam(tempArray));
            }
        }
    }
    
    IEnumerator MoveLightBeam(Vector3[] endPositionArray)
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
            while(true)
            {
                endPoint += vecToCurrentIndex.normalized * Time.deltaTime * moveRate;
                //Update calculation for a overshoot
                vecToEndPoint = endPoint - endPositionArray[index - 1];

                //Check if have over shot the target
                if(vecToEndPoint.magnitude > vecToCurrentIndex.magnitude)
                {
                    //Have overshot our point lets start to work on the next point
                    lineRenderer.SetPosition(index, endPositionArray[index]);
                    break;
                }

                //Update the LineRenderer
                lineRenderer.SetPosition(index, endPoint);
                yield return null;
            }            
            //Say we have hit our end point
            if(LightPositions[index].hitTile != null)
            {
                LightPositions[index].hitTile.TileHit(Utility.NormalizeVec2Int(LightPositions[index].lightPosition - LightPositions[index - 1].lightPosition), OutputColour);
            }
        }

        lineRenderer.SetPosition(endPositionArray.Length - 1, endPositionArray[endPositionArray.Length - 1]);
    }

    private void OnDrawGizmos()
    {
        Vector3 tempVec = ((Vector2)LightDirection);
        Gizmos.DrawLine(transform.position, transform.position + tempVec);
    }
}
