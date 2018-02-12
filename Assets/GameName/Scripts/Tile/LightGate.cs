using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightGate : BaseTile
{
    public TileColor gateColour;
    [SerializeField] private bool m_NeedsExactColour;
    [SerializeField] private Vector2Int GateDirection;

    public override void AssignNewTile(Vector2Int arrayPosition, SquareGridCreator creator, Color tileColor)
    {
        base.AssignNewTile(arrayPosition, creator, new Color(1f, 0f, 1f));
        
        tileType = TileTypes.LightGate;
    }

    public override void RemoveTile()
    {
        base.RemoveTile();
    }

    public bool CanPass(Vector2Int HitDirection, TileColor inputColour)
    {
        if (m_NeedsExactColour)
        {
            if (inputColour == gateColour)
            {
                return HitDirection == GateDirection;
            }
        }
        else
        {
            if(TileColor.ContainColour(inputColour, gateColour))
                return HitDirection == GateDirection;
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)(Vector2)GateDirection);
    }
}
