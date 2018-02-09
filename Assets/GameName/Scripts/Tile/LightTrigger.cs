using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTrigger : BaseTile, ITileHit
{
    public Color TriggerColour;
    public bool m_NeedExactColour;

    public override void AssignNewTile(Vector2Int arrayPosition, SquareGridCreator creator)
    {
        base.AssignNewTile(arrayPosition, creator);

        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.color = new Color(0.0f, 1.0f, 1.0f);

        tileType = TileTypes.LightTrigger;
    }

    public override void RemoveTile()
    {
        base.RemoveTile();
    }

    public void TileHit(Color hitColour)
    {
        if(CanPass(hitColour))
        {
            Debug.Log("Trigger Been Hit", gameObject);
        }
    }

    public bool CanPass(Color inputColor)
    {
        if (m_NeedExactColour)
        {
            if (inputColor == TriggerColour)
            {
                return true;
            }
        }
        else
        {
            return true;
        }
        return false;
    }
}
