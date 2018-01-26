using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGridCreator : MonoBehaviour
{
    public int Width, Height;

    public GameObject[][][] gridArray;

    public GameObject HexRef;

    IEnumerator Start()
    {
        yield return Poop(5);

        Debug.Log("End of Poop");
    }

    IEnumerator Poop(int poop)
    {
        for(int i = 0; i < poop; i++)
        {
            Debug.Log("Poop");
            yield return new WaitForSeconds(.5f);
        }
    }
}
