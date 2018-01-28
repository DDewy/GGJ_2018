using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateableSatellite : ReflectSatellite, Interactable {

    private BoxCollider collider;

    public override void AssignNewTile(Vector2Int arrayPosition, SquareGridCreator creator)
    {
        base.AssignNewTile(arrayPosition, creator);

        SpriteRenderer sprite = GetComponent<SpriteRenderer>();

        if(sprite != null)
        {
            sprite.color = Color.yellow;
        }

        tileType = TileTypes.RotateSatellite;

        collider = gameObject.AddComponent<BoxCollider>();

        collider.size = new Vector3(0.95f, 0.95f, 0.2f);
    }

    public override void RemoveTile()
    {
        base.RemoveTile();

        Destroy(collider);
    }

    //Left Click Anti-ClockWise, Right click Clock Wise
    public void Clicked(bool LeftClick)
    {
        ReflectDirection = RotateVec(ReflectDirection, LeftClick ? -45f : 45f);

        creator.OnPathUpdated();
    }

    Vector2Int RotateVec(Vector2Int RotateVec, float rotateAngle)
    {
        Vector2 tempVec = RotateVec;
        tempVec = Quaternion.Euler(0f, 0f, rotateAngle) * tempVec;

        return Vector2Int.RoundToInt(tempVec);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector2 reflectDir = (Vector2)ReflectDirection;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)(Vector2)ReflectDirection);
    }
}
