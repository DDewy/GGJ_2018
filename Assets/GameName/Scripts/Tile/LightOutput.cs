using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOutput : BaseTile
{
    public Vector2Int[] LightPositions;
    public Vector2Int LightDirection;
    public Color OutputColour;

    private LineRenderer lineRenderer;

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
            lineRenderer.material.color = OutputColour;
        }

        yield return new WaitForSeconds(0.1f);
        creator.PathUpdated += UpdatePath;
        UpdatePath();
    }

    public override void AssignNewTile(Vector2Int arrayPosition, SquareGridCreator creator)
    {
        base.AssignNewTile(arrayPosition, creator);

        SpriteRenderer sprite = GetComponent<SpriteRenderer>();

        if (sprite != null)
        {
            sprite.color = Color.red;
        }

        tileType = TileTypes.LightOutput;

        GameObject referenceObj = (GameObject)Resources.Load("Light");
        GameObject tempLine = Instantiate(referenceObj, transform);
        LineRenderer tempRender = referenceObj.GetComponent<LineRenderer>();

        GameObject refPrefab = (GameObject)Resources.Load("OutputPrefab");
        Instantiate(refPrefab, transform);

        lineRenderer = tempLine.GetComponent<LineRenderer>();
    }
    
    public override void RemoveTile()
    {
        if(lineRenderer != null)
        {
            DestroyImmediate(lineRenderer.gameObject);
            lineRenderer = null;
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
            bool bInstantBeam = false;

            Vector3[] tempArray = new Vector3[LightPositions.Length];

            if (bInstantBeam)
            {
                lineRenderer.positionCount = tempArray.Length;
            }
            
            for (int i = 0; i < tempArray.Length; i++)
            {
                if(bInstantBeam)
                {
                    lineRenderer.SetPosition(i, (Vector2)LightPositions[i]);
                }
                else
                {
                    tempArray[i] = (Vector2)LightPositions[i];
                }
                
            }

            if(!bInstantBeam)
            {
                StartCoroutine(MoveLightBeam(tempArray));
            }
        }
    }
    //1 Unit Per second. 1 unit = 1 square
    const float moveRate = 20f;

    IEnumerator MoveLightBeam(Vector3[] endPositionArray)
    {
        //Set up inital light beam
        //Vector3[] currentPoints = new Vector3();
        

        for (int index = 1; index < endPositionArray.Length; index++)
        {
            //Set up the Line Renderer
            lineRenderer.positionCount = index + 1;
            lineRenderer.SetPosition(index - 1, endPositionArray[index - 1]);

            //for(int i = 0; i < index; i++)
            //{
            //    lineRenderer.SetPosition(i, endPositionArray[i]);
            //}

            //Setting up end point for its starting position
            Vector3 endPoint = endPositionArray[index - 1];
            
            Vector3 vecToEndPoint = endPoint - endPositionArray[index - 1],
                vecToCurrentIndex = endPositionArray[index] - endPositionArray[index - 1];

            //While the end of the light is closer than the next point lets keep pushing it forward
            while(vecToEndPoint.magnitude < vecToCurrentIndex.magnitude)
            {
                endPoint += vecToCurrentIndex.normalized * Time.deltaTime * moveRate;
                vecToEndPoint = endPoint - endPositionArray[index - 1];
                //Update the LineRenderer
                lineRenderer.SetPosition(index, endPoint);
                yield return null;
            }

            
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 tempVec = ((Vector2)LightDirection);
        Gizmos.DrawLine(transform.position, transform.position + tempVec);
    }
}
