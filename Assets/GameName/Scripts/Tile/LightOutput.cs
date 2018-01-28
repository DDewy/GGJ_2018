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

    private void Update()
    {
        for(int i = 0; i < LightPositions.Length - 1; i++)
        {
            Debug.DrawLine((Vector2)LightPositions[i], (Vector2)LightPositions[i + 1]);
        }
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
            Vector3[] tempArray = new Vector3[LightPositions.Length];
            lineRenderer.positionCount = tempArray.Length;
            
            for (int i = 0; i < tempArray.Length; i++)
            {
                lineRenderer.SetPosition(i, (Vector2)LightPositions[i]);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 tempVec = ((Vector2)LightDirection);
        Gizmos.DrawLine(transform.position, transform.position + tempVec);
    }
}
