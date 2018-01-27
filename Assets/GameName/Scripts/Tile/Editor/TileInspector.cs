using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Tile))]
public class TileInspector : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.LabelField("Change Tile Type");

        EditorGUILayout.BeginHorizontal();

        if(GUILayout.Button("Tile"))
        {
            ChangeTo(Tile.TileTypes.Tile);
        }

        if (GUILayout.Button("Satalite"))
        {
            ChangeTo(Tile.TileTypes.Satalite);
        }

        if (GUILayout.Button("Light Output"))
        {
            ChangeTo(Tile.TileTypes.LightOutput);
        }

        if (GUILayout.Button("Target"))
        {
            ChangeTo(Tile.TileTypes.LightTarget);
        }

        EditorGUILayout.EndHorizontal();
    }

    void ChangeTo(Tile.TileTypes newType)
    {
        BaseTile oldTile = (BaseTile)target;

        BaseTile newTile = null;

        switch(newType)
        {
            case Tile.TileTypes.Tile:
                newTile = oldTile.gameObject.AddComponent<Tile>();
                break;

            case Tile.TileTypes.Satalite:
                newTile = oldTile.gameObject.AddComponent<ReflectSatellite>();
                break;

            case Tile.TileTypes.LightOutput:
                newTile = oldTile.gameObject.AddComponent<LightOutput>();
                break;

            case Tile.TileTypes.LightTarget:
                newTile = oldTile.gameObject.AddComponent<LightOutput>();
                break;
        }

        if(newTile == null)
        {
            //report an Error and Back out
            Debug.LogError("Could not Change Tile target");
            return;
        }

        newTile.AssignNewTile(oldTile.arrayPosition);

        DestroyImmediate(oldTile);
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