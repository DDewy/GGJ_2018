using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; //testing

public class ColourMixing : MonoBehaviour {

    public Color wubColour;
    public Color addColour;

    public void mixColoursPerfectly(ref Color inColor)
    {        
        Vector4 colour1 = new Vector4(Mathf.Pow(wubColour.r, 2), Mathf.Pow(wubColour.g, 2), Mathf.Pow(wubColour.b, 2), 1.0f);
        Vector4 colour2 = new Vector4(Mathf.Pow(inColor.r, 2), Mathf.Pow(inColor.g, 2), Mathf.Pow(inColor.b, 2), 1.0f);
        wubColour = (colour1 + colour2) / 2;
        wubColour = new Vector4(Mathf.Sqrt(wubColour.r), Mathf.Sqrt(wubColour.g), Mathf.Sqrt(wubColour.b), 1.0f);
    }

    public void mixColoursSimple(ref Color inColor) //let's use this one. Red/Blue/Green/Magenta/Cyan/Yellow/White
    {
        wubColour += inColor;
    }
}

//testing
[CustomEditor(typeof(ColourMixing))]
public class ColourMixingEditor : Editor
{   
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        ColourMixing colourMixer = (ColourMixing)target;
        if (GUILayout.Button("Mix Colours Simple"))
        {
            colourMixer.mixColoursSimple(ref colourMixer.addColour);
        }
        if (GUILayout.Button("Mix Colours Complex"))
        {
            colourMixer.mixColoursPerfectly(ref colourMixer.addColour);
        }
    }
}
