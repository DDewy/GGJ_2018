using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Tile))]
public class TileInspector : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.LabelField("Change Tile Type");

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Tile"))
        {
            ChangeTo(Tile.TileTypes.Tile);
        }

        if (GUILayout.Button("Satalite"))
        {
            ChangeTo(Tile.TileTypes.Satalite);
        }

        if(GUILayout.Button("RotSatellite"))
        {
            ChangeTo(Tile.TileTypes.RotateSatellite);
        }

        if (GUILayout.Button("Light Output"))
        {
            ChangeTo(Tile.TileTypes.LightOutput);
        }

        if (GUILayout.Button("Target"))
        {
            ChangeTo(Tile.TileTypes.LightTarget);
        }

        if (GUILayout.Button("Asteroid"))
        {
            ChangeTo(Tile.TileTypes.Asteroid);
        }

        EditorGUILayout.EndHorizontal();
    }

    void ChangeTo(Tile.TileTypes newType)
    {
        BaseTile.ChangeTo(newType, (BaseTile)target);
        //    BaseTile oldTile = (BaseTile)target;

        //    BaseTile newTile = null;

        //    switch(newType)
        //    {
        //        case Tile.TileTypes.Tile:
        //            newTile = oldTile.gameObject.AddComponent<Tile>();
        //            break;

        //        case Tile.TileTypes.Satalite:
        //            newTile = oldTile.gameObject.AddComponent<ReflectSatellite>();
        //            break;

        //        case Tile.TileTypes.LightOutput:
        //            newTile = oldTile.gameObject.AddComponent<LightOutput>();
        //            break;

        //        case Tile.TileTypes.LightTarget:
        //            newTile = oldTile.gameObject.AddComponent<TargetTile>();
        //            break;

        //        case Tile.TileTypes.Asteroid:
        //            newTile = oldTile.gameObject.AddComponent<AsteroidTile>();
        //            break;
        //    }

        //    if(newTile == null)
        //    {
        //        //report an Error and Back out
        //        Debug.LogError("Could not Change Tile target");
        //        return;
        //    }
        //    oldTile.RemoveTile();
        //    newTile.AssignNewTile(oldTile.arrayPosition);

        //    DestroyImmediate(oldTile);
        //}
    }
}

[CustomEditor(typeof(ReflectSatellite))]
public class ReflectSatelliteInspector : TileInspector
{

}

[CustomEditor(typeof(TargetTile))]
public class TargetTileInspector : TileInspector
{

}

[CustomEditor(typeof(LightOutput))]
public class LightOutputInspector : TileInspector
{

}

[CustomEditor(typeof(AsteroidTile))]
public class AsteroidTileInspector : TileInspector
{

}

[CustomEditor(typeof(RotateableSatellite))]
public class RotateableSatelliteInspector : TileInspector
{

}