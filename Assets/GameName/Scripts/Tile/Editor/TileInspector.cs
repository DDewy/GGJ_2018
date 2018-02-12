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

        if(GUILayout.Button("Comb Satellite"))
        {
            ChangeTo(BaseTile.TileTypes.CombineSatellite);
        }

        if (GUILayout.Button("Light Output"))
        {
            ChangeTo(Tile.TileTypes.LightOutput);
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Target"))
        {
            ChangeTo(Tile.TileTypes.LightTarget);
        }

        if (GUILayout.Button("Asteroid"))
        {
            ChangeTo(Tile.TileTypes.Asteroid);
        }

        if(GUILayout.Button("Trigger"))
        {
            ChangeTo(BaseTile.TileTypes.LightTrigger);
        }

        if(GUILayout.Button("Gate"))
        {
            ChangeTo(BaseTile.TileTypes.LightGate);
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();

        if(GUILayout.Button("Splitter"))
        {
            ChangeTo(BaseTile.TileTypes.SatelliteSplitter);
        }

        EditorGUILayout.EndHorizontal();
    }

    void ChangeTo(Tile.TileTypes newType)
    {
        BaseTile.ChangeTo(newType, (BaseTile)target);
    }
}

[CustomEditor(typeof(Satellite))]
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

[CustomEditor(typeof(CombineSatellite))]
public class CombineSatelliteInspector : TileInspector
{

}

[CustomEditor(typeof(LightTrigger))]
public class LightTriggerInspector : TileInspector
{

}

[CustomEditor(typeof(LightGate))]
public class LightGateInspector : TileInspector
{

}

[CustomEditor(typeof(SplitterSatellite))]
public class SplitterSatelliteInspector : TileInspector
{

}