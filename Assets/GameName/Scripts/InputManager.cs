using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
    [Header("Components")]
    [SerializeField] private Camera playerCamera;


	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Left Click Rotate Anti Clock Wise, Right Click Rotate Clock Wise
        if (Input.GetMouseButtonDown(0))
        {
            Ray clickRay = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit tempHit;
            Physics.Raycast(clickRay, out tempHit);

            if(tempHit.collider != null)
            {
                tempHit.collider.GetComponent<Interactable>().Clicked(true);
            }
        }
        else if(Input.GetMouseButtonDown(1))
        {
            Ray clickRay = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit tempHit;
            Physics.Raycast(clickRay, out tempHit);

            if (tempHit.collider != null)
            {
                tempHit.collider.GetComponent<Interactable>().Clicked(false);
            }
        }
	}
}
