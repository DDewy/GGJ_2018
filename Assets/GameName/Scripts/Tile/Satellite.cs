using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satellite : BaseTile
{
    public bool CanRotate
    {
        get
        {
            return m_CanRotate;
        }

        set
        {
            if(value != m_CanRotate) //If it is a different value than current set
            {
                if (value)
                    SetUpRotatable();
                else
                    RemoveRotatable();
            }
        }
    }
    public Vector2Int ReflectDirection;

    private bool m_CanRotate = false;
    private BoxCollider collider;

    /// <summary>
    /// 
    /// </summary>
    /// <returns>Vector of where this satalite is aiming</returns>
	public Vector2Int ReflectPosition()
    {
        return ReflectDirection;
    }


    public override void AssignNewTile(Vector2Int arrayPosition, SquareGridCreator creator, Color tileColour)
    {
        base.AssignNewTile(arrayPosition, creator, Color.blue);

        tileType = TileTypes.Satalite;
    }

    #region Base Rotate Code
    protected virtual void SetUpRotatable()
    {
        m_CanRotate = true;

        collider = gameObject.AddComponent<BoxCollider>();

        collider.size = new Vector3(0.95f, 0.95f, 0.2f);
    }

    protected virtual void RemoveRotatable()
    {
        m_CanRotate = false;

        if(collider != null)
            Destroy(collider);

        collider = null;
    }

    //Left Click Anti-ClockWise, Right click Clock Wise
    public void Clicked(bool LeftClick)
    {
        if (!m_CanRotate)
            return;

        ReflectDirection = RotateVec(ReflectDirection, LeftClick ? -45f : 45f);

        creator.OnPathUpdated();
    }

    public static Vector2Int RotateVec(Vector2Int RotateVec, float rotateAngle)
    {
        Vector2 tempVec = RotateVec;
        tempVec = Quaternion.Euler(0f, 0f, rotateAngle) * tempVec;

        return Vector2Int.RoundToInt(tempVec);
    }
    #endregion

    private void OnDrawGizmos()
    {
        if(m_CanRotate)
        {
            Gizmos.color = Color.red;
            Vector2 reflectDir = (Vector2)ReflectDirection;
            Gizmos.DrawLine(transform.position, transform.position + (Vector3)(Vector2)ReflectDirection);
        }
        else
        {
            if (!Application.isPlaying)
            {
                Vector3 tempVec = ((Vector2)ReflectDirection);
                Gizmos.DrawLine(transform.position, transform.position + tempVec);
            }
        }
    }
}
