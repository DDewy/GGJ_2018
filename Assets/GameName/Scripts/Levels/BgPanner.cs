using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgPanner : MonoBehaviour
{
    float speed = 0.005f;
    Renderer render;

    public void Start()
    {
        render = gameObject.GetComponent<Renderer>();
        StartCoroutine(Pan());        
    }

    public void fasterPan()     //subscribe to end of level
    {
        gameObject.GetComponentInChildren<AudioSource>().Play();
        StartCoroutine(QuickPan());
    }

    IEnumerator QuickPan()
    {
        speed = 0.3f;
        yield return new WaitForSeconds(3);
        speed = 0.05f;
        StopCoroutine(QuickPan());
    }

    IEnumerator Pan()
    {
        float offset = 0f;        
        while (true)
        {
            offset += Time.deltaTime * speed;
            render.material.SetTextureOffset("_MainTex", new Vector2(0, offset));
            yield return null;
        }        
    }
}
