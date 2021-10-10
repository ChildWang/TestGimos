using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestory : MonoBehaviour
{
    private float fadeTime = 1;
    // Start is called before the first frame update
    void Start()
    {
        //Destroy(this.gameObject, fadeTime);
        StartCoroutine(AutoFade());
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator AutoFade()
    {
        yield return new WaitForSeconds(fadeTime);
        this.gameObject.SetActive(false);
    }
}
