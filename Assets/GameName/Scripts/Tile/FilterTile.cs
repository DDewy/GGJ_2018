using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilterTile : BaseTile, ITileHit {

    [SerializeField] private TileColor FilterColour;

    private LineRenderer lineRenderer;
    private LightHitInfo[] LightPositions;

    void Start()
    {
        //Update Line Renderer with Output Colour
        if (lineRenderer == null)
        {
            if (transform.childCount > 0)
            {
                lineRenderer = transform.GetChild(0).GetComponent<LineRenderer>();
            }
        }

        if (lineRenderer != null)
        {
            lineRenderer.material.color = FilterColour.ToColour();
        }
    }

    public override void AssignNewTile(Vector2Int arrayPosition, SquareGridCreator creator, Color tileColour)
    {
        base.AssignNewTile(arrayPosition, creator, new Color(0f, 1.0f, 0.5f));

        tileType = TileTypes.LightFilter;

        //Spawn Light Renderer
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
    }

    public void TileHit(Vector2Int HitDirection, TileColor tileColour)
    {
        //Check if the light can pass through 
        if(TileColor.ContainColour(tileColour, FilterColour))
        {
            LightPositions = creator.LightBouncePositions(arrayPosition, HitDirection, tileColour);

            Vector3[] tempArray = new Vector3[LightPositions.Length];

            if(bInstantBeam)
            {
                lineRenderer.positionCount = tempArray.Length;
            }

            for(int i = 0; i < tempArray.Length; i++)
            {
                tempArray[i] = (Vector2)LightPositions[i].lightPosition;

                if (bInstantBeam)
                {
                    lineRenderer.SetPosition(i, tempArray[i]);

                    if(LightPositions[i].hitTile != null)
                    {
                        LightPositions[i].hitTile.TileHit(Utility.NormalizeVec2Int(LightPositions[i].lightPosition - LightPositions[i - 1].lightPosition), tileColour);
                    }
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
            while (true)
            {
                endPoint += vecToCurrentIndex.normalized * Time.deltaTime * moveRate;
                //Update calculation for a overshoot
                vecToEndPoint = endPoint - endPositionArray[index - 1];

                //Check if have over shot the target
                if (vecToEndPoint.magnitude > vecToCurrentIndex.magnitude)
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
            if (LightPositions[index].hitTile != null)
            {
                LightPositions[index].hitTile.TileHit(Utility.NormalizeVec2Int(LightPositions[index].lightPosition - LightPositions[index - 1].lightPosition), FilterColour);
            }
        }

        lineRenderer.SetPosition(endPositionArray.Length - 1, endPositionArray[endPositionArray.Length - 1]);
    }
}
