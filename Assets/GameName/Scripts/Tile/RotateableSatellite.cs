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

    public void Clicked(bool LeftClick)
    {
        Debug.Log("Been tocuhed with a " + (LeftClick ? "Left" : "Right") + " click");
    }
}
