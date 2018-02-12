using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Color : MonoBehaviour {
    public TileColor tempColour;
    public Color outputColor;
    public string outputString;
    private void Start()
    {
        Debug.Log(tempColour);
    }

    private void Update()
    {
        outputColor = tempColour.ToColour();
        outputString = tempColour.ToString();
    }
}