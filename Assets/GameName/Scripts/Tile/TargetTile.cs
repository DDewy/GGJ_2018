﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTile : BaseTile
{
    public Color TargetColour;
    public bool hasLight = false;

    private void Start()
    {
        creator.PrePathUpdate += delegate { hasLight = false; };
    }

    public void HitByLight()
    {
        hasLight = true;
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
    }
}
