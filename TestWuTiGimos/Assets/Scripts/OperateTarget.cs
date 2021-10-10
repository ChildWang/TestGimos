using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;
using RTEditor;

public class OperateTarget : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public string sourcename;
    Text Name;
    public GameObject prefab;
    GameObject go;
    public void OnBeginDrag(PointerEventData eventData)
    {
        go =Instantiate(prefab);
       
        Debug.Log("11");
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (go != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5);
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            go.transform.position = worldPos;

            //EditorObjectSelection.Instance.OnInputDeviceFirstButtonDown();
            //Vector3 screenSpace;
            //if (Physics.Raycast(ray, out hit))
            //{
            //    go.transform.position = hit.point;
            //}
            //else
            //{
            //    Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1);
            //    Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);                
            //    go.transform.position = worldPos;
            //}
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {

    }
    // Use this for initialization
    void Start()
    {
        //Name = transform.Find("Text").GetComponent<Text>();
    }
}