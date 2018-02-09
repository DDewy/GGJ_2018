using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTile : BaseTile, ITileHit
{
    public Color TargetColour;
    public bool hasLight = false;

    private void Start()
    {
        creator.PrePathUpdate += delegate { hasLight = false; };
    }

    public void TileHit(Color hitColor)
    {
        hasLight = true;

        //Update any visuals or anything to the gamemode to say that it has been hit
    }

    public override void AssignNewTile(Vector2Int arrayPosition, SquareGridCreator creator)
    {
        base.AssignNewTile(arrayPosition, creator);

        SpriteRenderer sprite = GetComponent<SpriteRenderer>();

        if (sprite != null)
        {
            sprite.color = Color.green;
        }

        tileType = TileTypes.LightTarget;

        GameObject refPrefab = (GameObject)Resources.Load("TargetPrefab");
        Instantiate(refPrefab, transform);
    }
}
