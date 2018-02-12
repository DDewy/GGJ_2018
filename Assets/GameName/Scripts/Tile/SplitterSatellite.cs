using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitterSatellite : BaseTile, ITileHit
{
    private LineRenderer[] lineRenderers;
    private LightHitInfo[][] lightHitPaths;

    void Start()
    {
        if(lineRenderers == null)
        {
            lineRenderers = new LineRenderer[2];
            lightHitPaths = new LightHitInfo[2][];
            for(int i = 0; i < lineRenderers.Length; i++)
            {
                lineRenderers[i] = transform.GetChild(i).GetComponent<LineRenderer>();
            }
        }

        creator.PrePathUpdate += ClearLights;
    }

    public override void AssignNewTile(Vector2Int arrayPosition, SquareGridCreator creator, Color tileColour)
    {
        base.AssignNewTile(arrayPosition, creator, new Color(1f, 1f, 1f));

        tileType = TileTypes.SatelliteSplitter;

        //Instanciate two Light Emiiters
        lineRenderers = new LineRenderer[2];

        for (int i = 0; i < lineRenderers.Length; i++)
        {
            GameObject referenceObj = (GameObject)Resources.Load("Light");
            GameObject tempLine = Instantiate(referenceObj, transform);
            LineRenderer tempRender = referenceObj.GetComponent<LineRenderer>();

            lineRenderers[i] = tempLine.GetComponent<LineRenderer>();
        }
    }

    public override void RemoveTile()
    {
        base.RemoveTile();
    }

    public void TileHit(Vector2Int HitDirection, TileColor inputColour)
    {
        //why doesn't this get hit and debug, please tell me. It is your mission to find out
        
        //Tile Hit, Split that nigger
        Vector2Int[] outputDirection = new Vector2Int[2];

        //Calculate 135 Degrees and 225 Degrees
        outputDirection[0] = Satellite.RotateVec(HitDirection, -45f);
        outputDirection[1] = Satellite.RotateVec(HitDirection,  45f);

        //Get the Calculated Path
        lightHitPaths[0] = creator.LightBouncePositions(arrayPosition, outputDirection[0], inputColour);
        lightHitPaths[1] = creator.LightBouncePositions(arrayPosition, outputDirection[1], inputColour);

        //Set Line Render Color
        Color newColor = inputColour.ToColour();
        lineRenderers[0].material.color = newColor;
        lineRenderers[1].material.color = newColor;


        Vector3[][] lightPaths = new Vector3[lightHitPaths.Length][];

        for(int p = 0; p < lightHitPaths.Length; p++)
        {
            lineRenderers[p].positionCount = bInstantBeam ? 0 : lightHitPaths[p].Length;

            lightPaths[p] = new Vector3[lightHitPaths[p].Length];

            for (int i = 0; i < lightHitPaths[p].Length; i++)
            {
                lightPaths[p][i] = (Vector2)lightHitPaths[p][i].lightPosition;

                if(!bInstantBeam)
                {
                    continue;
                }

                lineRenderers[p].SetPosition(i, lightPaths[p][i]);

                if (lightHitPaths[p][i].hitTile != null)
                {
                    lightHitPaths[p][i].hitTile.TileHit(Utility.NormalizeVec2Int(lightHitPaths[p][i].lightPosition - lightHitPaths[p][i - 1].lightPosition), inputColour);
                }
            }
        }

        if (!bInstantBeam)
        {
            //Pass Path to a coroutine which travels each path 
            for(int i = 0; i < lightHitPaths.Length; i++)
            {
                StartCoroutine(MoveLightBeam(i, lightPaths[i], inputColour));
            }
        }
    }

    IEnumerator MoveLightBeam(int pathIndex, Vector3[] endPositionArray, TileColor lightColour)
    {
        for (int index = 1; index < endPositionArray.Length; index++)
        {
            //Set up the Line Renderer
            lineRenderers[pathIndex].positionCount = index + 1;
            lineRenderers[pathIndex].SetPosition(index - 1, endPositionArray[index - 1]);

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
                    lineRenderers[pathIndex].SetPosition(index, endPositionArray[index]);
                    break;
                }

                //Update the LineRenderer
                lineRenderers[pathIndex].SetPosition(index, endPoint);
                yield return null;
            }
            //Say we have hit our end point
            if (lightHitPaths[pathIndex][index].hitTile != null)
            {
                lightHitPaths[pathIndex][index].hitTile.TileHit(Utility.NormalizeVec2Int(lightHitPaths[pathIndex][index].lightPosition - lightHitPaths[pathIndex][index-1].lightPosition), lightColour);
            }
        }

        lineRenderers[pathIndex].SetPosition(endPositionArray.Length - 1, endPositionArray[endPositionArray.Length - 1]);
    }

    void ClearLights()
    {
        StopAllCoroutines();

        for (int i = 0; i < lineRenderers.Length; i++)
        {
            //Reset Renderer
            lineRenderers[i].positionCount = 0;
            //Clear Info Path
            lightHitPaths[i] = null;

            //Set Line renderers to black 
            lineRenderers[i].material.color = Color.black;
        }
    }
}
