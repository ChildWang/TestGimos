using EventCenter;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBillBoard : MonoBehaviour
{
   
	// Use this for initialization
	void Start ()
    {
		 BillBoard.instance.SetBillBoardTarget(new GameObject []{ this.gameObject},new string[] {this.gameObject.name});
        StartCoroutine(WaiteOneFrame(() => {
            BillBoard.instance.ShowBillBoard(true);
        }));
    }
    IEnumerator WaiteOneFrame(Action action)
    {
        yield return 1;
        if (action != null)
            action();
    }
    // Update is called once per frame
    void Update () {
		
	}
}
