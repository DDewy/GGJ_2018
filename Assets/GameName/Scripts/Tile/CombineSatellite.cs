using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineSatellite : RotateableSatellite
{
    private List<Color> inputColours;
    private LineRenderer lineRenderer;
    public Color outputColour;

    private void Start()
    {
        creator.PrePathUpdate += ClearColours;
        creator.PathUpdated += RefreshingPaths;

        ClearColours();
    }

    void ClearColours()
    {
        inputColours = new List<Color>();
    }

    void RefreshingPaths()
    {
        Vector2Int[] LightPositions = creator.LightBouncePositions(arrayPosition, ReflectDirection, outputColour);

        if (lineRenderer == null)
        {
            lineRenderer = transform.GetChild(0).GetComponent<LineRenderer>();
        }

        if (lineRenderer != null)
        {
            Vector3[] tempArray = new Vector3[LightPositions.Length];
            lineRenderer.positionCount = tempArray.Length;

            for (int i = 0; i < tempArray.Length; i++)
            {
                lineRenderer.SetPosition(i, (Vector2)LightPositions[i]);
            }
        }
    }

    public void HitByColour(Color newColour)
    {
        inputColours.Add(newColour);

        //Update Output Colour
        outputColour = Color.black;
        for(int i = 0; i < inputColours.Count; i++)
        {
            outputColour += inputColours[i];
        }

        if(lineRenderer != null)
        {
            lineRenderer.material.color = outputColour;
        }

        RefreshingPaths();
    }

    public override void AssignNewTile(Vector2Int arrayPosition, SquareGridCreator creator)
    {
        base.AssignNewTile(arrayPosition, creator);

        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        
        if(sprite != null)
        {
            sprite.color = new Color(1.0f, 0.5f, 0.0f); //Should be orange
        }

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
