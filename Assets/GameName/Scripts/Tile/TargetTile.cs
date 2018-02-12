using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTile : BaseTile, ITileHit
{
    public TileColor TargetColour;
    public bool hasLight = false;

    private void Start()
    {
        creator.PrePathUpdate += delegate { hasLight = false; };
    }

    public void TileHit(Vector2Int HitDirection, TileColor hitColor)
    {
        hasLight = TileColor.ExactSameColour(hitColor, TargetColour);
        //Update any visuals or anything to the gamemode to say that it has been hit
    }

    public override void AssignNewTile(Vector2Int arrayPosition, SquareGridCreator creator, Color tileColour)
    {
        base.AssignNewTile(arrayPosition, creator, Color.green);

        tileType = TileTypes.LightTarget;

        GameObject refPrefab = (GameObject)Resources.Load("TargetPrefab");
        Instantiate(refPrefab, transform);
    }
}
