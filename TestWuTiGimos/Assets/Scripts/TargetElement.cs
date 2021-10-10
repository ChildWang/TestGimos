using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class TargetElement : MonoBehaviour {

    public HighlightableObject highLight;
    // Use this for initialization
    void Start () {
        EventTriggerListener.Get(this.gameObject).onClick += OnClick;
        highLight = this.gameObject.GetComponent<HighlightableObject>();
        if (highLight == null)
        {
            highLight = this.gameObject.AddComponent<HighlightableObject>();
        }
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public virtual void OnClick(PointerEventData eventData)
    {        
        MySingleton.getInstance<PhysicsRaycasterManager>().SetTarget(this.gameObject);       
    }

    public virtual void LightOn()
    {
        highLight.ConstantOn(Color.green);
    }
    public virtual void LightOff()
    {
        highLight.ConstantOff();
    }

    public virtual void OpenBillBoard()
    {
        BillBoard.instance.SetBillBoardTarget(new GameObject[] { this.gameObject }, new string[] { this.gameObject.name });
        StartCoroutine(WaiteOneFrame(() => {
            BillBoard.instance.ShowBillBoard(true);
        }));
    }
    public virtual void CloseBillBoard()
    {
        BillBoard.instance.ShowBillBoard(false);
    }
    IEnumerator WaiteOneFrame(Action action)
    {
        yield return 1;
        if (action != null)
            action();
    }
}
