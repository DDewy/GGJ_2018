using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectSatellite : BaseTile
{
    public Vector2Int ReflectDirection;

    /// <summary>
    /// 
    /// </summary>
    /// <returns>Vector of where this satalite is aiming</returns>
	public Vector2Int ReflectPosition()
    {
        return ReflectDirection;
    }


    public override void AssignNewTile(Vector2Int arrayPosition)
    {
        base.AssignNewTile(arrayPosition);

        SpriteRenderer sprite = GetComponent<SpriteRenderer>();

        if (sprite != null)
        {
            sprite.color = Color.blue;
        }

        tileType = TileTypes.Satalite;
    }

    private void OnDrawGizmos()
    {
        Vector3 tempVec = ((Vector2)ReflectDirection);
        Gizmos.DrawLine(transform.position, transform.position + tempVec);
    }
}
