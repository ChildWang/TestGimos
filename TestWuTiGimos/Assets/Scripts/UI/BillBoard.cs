using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BillBoard : MonoBehaviour
{

    private Transform cacheTransform;
    private GameObject[] targetTfs;
    public BillBoardItem[] billBoardItems;
    public GameObject itemObject;
    private int count;
    private bool showBillBoard;
    private Vector3 tempV3;
    public BillBoardItem[] usedTargetTfs;

    public static BillBoard instance;
    void Awake()
    {
        instance = this;
        cacheTransform = this.transform;
       
    }

    public void SetBillBoardTarget(GameObject[] targettfs,string[] names)
    {
        if (usedTargetTfs != null)
        {
            usedTargetTfs.ToList().ForEach(o => {
                Destroy(o.gameObject);
            });
            usedTargetTfs = null;
        }
        this.targetTfs = targettfs;
        count = targettfs.Length;
        billBoardItems=new BillBoardItem[count];
        GameObject tempGo;
        for (int i = 0; i < count; i++)
        {
            tempGo = Instantiate(itemObject);
            tempGo.transform.SetParent(cacheTransform);
            billBoardItems[i] = tempGo.GetComponent<BillBoardItem>();
            billBoardItems[i].uiText.text = names[i];           
            billBoardItems[i].transform.localScale=Vector3.one;
            
        }
        usedTargetTfs = billBoardItems;
       
    }
    

    public void ShowBillBoard(bool show)
    {
        showBillBoard = show;
        for (int i = 0; i < count; i++)
        {
           // print(billBoardItems[i]+"动画"+show);
            billBoardItems[i].TweenAnim(show);
        }
    }

    //private void SetPosition()
    //{
    //    for (int i = 0; i < count; i++)
    //    {
    //        tempV3 = Camera.main.WorldToViewportPoint(targetTfs[i].transform.position);
    //        tempV3.x *= ScreenResize.Instance.activeWidth;
    //        tempV3.y *= ScreenResize.Instance.activeHeight;
    //        tempV3.z = 0f;
    //        billBoardItems[i].SetPosition(tempV3);
    //    }

    //}
    private void SetPosition()
    {
        for (int i = 0; i < count; i++)
        {
            tempV3 = Camera.main.WorldToScreenPoint(targetTfs[i].transform.position);
            //tempV3.x *= ScreenResize.Instance.activeWidth;
            //tempV3.y *= ScreenResize.Instance.activeHeight;
            //tempV3.z = 0f;
            billBoardItems[i].SetPosition(new Vector3(tempV3.x,tempV3.y,0));
        }

    }

    void Update()
    {
        if (showBillBoard)
        {
            SetPosition();
        }
    }

    void OnDestroy()
    {
        
    }
}
