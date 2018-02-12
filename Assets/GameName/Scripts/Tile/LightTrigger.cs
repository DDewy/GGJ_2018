using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTrigger : BaseTile, ITileHit
{
    public TileColor TriggerColour;
    public bool m_NeedExactColour;

    public override void AssignNewTile(Vector2Int arrayPosition, SquareGridCreator creator, Color tileColour)
    {
        base.AssignNewTile(arrayPosition, creator, new Color(0f, 1f, 1f));

        tileType = TileTypes.LightTrigger;
    }

    public override void RemoveTile()
    {
        base.RemoveTile();
    }

    public void TileHit(Vector2Int HitDirection, TileColor hitColour)
    {
        if(CanPass(hitColour))
        {
            Debug.Log("Trigger Been Hit", gameObject);
        }
    }

    public bool CanPass(TileColor inputColor)
    {
        if (m_NeedExactColour)
        {
            return (inputColor == TriggerColour);
        }
        else
        {
            return TileColor.ContainColour(inputColor, TriggerColour);
        }
    }
}
