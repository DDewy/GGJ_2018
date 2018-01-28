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
        yield return new WaitForSeconds(2.0f);

        LightPositions = SquareGridCreator.instance.LightBouncePositions(arrayPosition, LightDirection);

        if(lineRenderer == null)
        {
            lineRenderer = transform.GetChild(0).GetComponent<LineRenderer>();
        }

        if(lineRenderer != null)
        {
            Vector3[] tempArray = new Vector3[LightPositions.Length];
            lineRenderer.positionCount = tempArray.Length;


            for (int i = 0; i < tempArray.Length; i++)
            {
                lineRenderer.SetPosition(i, (Vector2)LightPositions[i]);
                //tempArray[i] = (Vector2)LightPositions[i];
            }

            
            
        }
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

        GameObject tempLine = Instantiate((GameObject)Resources.Load("Light"), transform);

        lineRenderer = tempLine.GetComponent<LineRenderer>();
    }
    
    public override void RemoveTile()
    {
        if(lineRenderer != null)
        {
            DestroyImmediate(lineRenderer.gameObject);
            lineRenderer = null;
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 tempVec = ((Vector2)LightDirection);
        Gizmos.DrawLine(transform.position, transform.position + tempVec);
    }
}
