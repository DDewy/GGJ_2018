using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangableGate : LightGate, ITriggerable, ITileHit
{
    public TileColor ActivatedColour, DeactivatedColour, incomingColour; 
    public BaseTile /*Something to Re*/

	// Use this for initialization
	void Start ()
    {
		
	}

    public override void AssignNewTile(Vector2Int arrayPosition, SquareGridCreator creator, Color tileColor)
    {
        base.AssignNewTile(arrayPosition, creator, tileColor);
    }

    public override void RemoveTile()
    {
        base.RemoveTile();
    }

    public void Activate()
    {

    }

    public void Deactivate()
    {

    }

    public void TileHit(Vector2Int HitDirection, TileColor inputColor)
    {

    }
}
