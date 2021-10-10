using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTEditor;
using UnityEngine.EventSystems;

public class BaseUIElement : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    private  bool inFloor=false;
    public GameObject equipmentHololens;

    private GameObject dragEquipment;
    public GameObject realEquipment;

    // Start is called before the first frame update
    void Start()
    {
        EditorObjectSelection.Instance.SelectionDeleted += SelectionDeletedHandler;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnBeginDrag(PointerEventData eventData)
    {       
        dragEquipment= Instantiate(equipmentHololens);      
    }

    public void OnDrag(PointerEventData eventData)
    {
        Ray ray = EditorCamera.Instance.Camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, 1 << LayerMask.NameToLayer("StaticMesh")))
        {
            if (hit.point != null)
            {

                Vector3 screenPos = EditorCamera.Instance.Camera.WorldToScreenPoint(hit.point);
                Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPos.z);
                Vector3 worldPos = EditorCamera.Instance.Camera.ScreenToWorldPoint(mousePos);

                Vector3 objWorldPos = new Vector3(worldPos.x, worldPos.y+ dragEquipment.GetComponent<BoxCollider>().bounds.size.y*0.5f, worldPos.z);

                dragEquipment.transform.position = objWorldPos;
                inFloor = true;
            }
            //else
            //{
            //    Vector3 worldPos = EditorCamera.Instance.Camera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, 5));
            //    dragEquipment.transform.position = worldPos;
            //}
        }
        else
        {
            Vector3 worldPos = EditorCamera.Instance.Camera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, 5));
            dragEquipment.transform.position = worldPos;
            inFloor = false;
        }
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (inFloor)
        {
            GameObject createdObj=Instantiate(realEquipment, dragEquipment.transform.position, dragEquipment.transform.rotation);
            var createAction = new CreateObjectAction(createdObj);
            createAction.Execute();

           
            Destroy(dragEquipment);
        }
        else
        {
            Destroy(dragEquipment);
        } 

    }




    void SelectionDeletedHandler(List<GameObject> deletedObjects) {
        //Debug.Log("删除了几个物体"+deletedObjects.Count);
    }
}
