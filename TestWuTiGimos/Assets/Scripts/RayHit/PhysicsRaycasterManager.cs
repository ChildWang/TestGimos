using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PhysicsRaycasterManager : MonoBehaviour
{


    PhysicsRaycaster raycaster;

    RaycastHit[] hitsInfo;


    private GameObject selectTarget;
    public GameObject dragTarget;
    float operateDistance = 10f;


    //public List<GameObject> current 

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        hitsInfo = Physics.RaycastAll(ray);
        {
            if (hitsInfo.Length > 0)
            {
                Vector3 hitPos = hitsInfo[0].point;
                Vector3 screenPos = Camera.main.WorldToScreenPoint(hitPos);
                Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPos.z);
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
                if (dragTarget)
                {
                   // Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                    dragTarget.transform.position = worldPos;
                    dragTarget.transform.rotation = Camera.main.transform.rotation;
                }
                else
                {
                    // Cursor.SetCursor(handTexture, Vector2.zero, CursorMode.Auto);
                }
            }
            else
            {
                Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, operateDistance);
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
                if (dragTarget)
                {
                    dragTarget.transform.position = worldPos;
                    dragTarget.transform.rotation = Camera.main.transform.rotation;
                }
                else
                {
                    // Cursor.SetCursor(handTexture, Vector2.zero, CursorMode.Auto);
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            //if (CheckOperateResult(hitsInfo,)&& !EventSystem.current.IsPointerOverGameObject())
            //{

            //}
        }
    }


    bool CheckOperateResult(RaycastHit[] hits, List<GameObject> idleTargets)
    {
        bool result = false;


        for (int i = 0; i < hits.Length; i++)
        {
            if (idleTargets.Contains(hits[i].collider.gameObject))
            {
                result = true;
                break;
            }
        }
        return result;
    }


    public void SetTarget(GameObject go)
    {
        if (selectTarget != null)
        {
            TargetElement te= selectTarget.GetComponent<TargetElement>();
            te.LightOff();
            //te.CloseBillBoard();
        }
        selectTarget = go;
        //Camera.main.gameObject.GetComponent<MouseFollowOrbit>().SetTarget(selectTarget);
        TargetElement newte = selectTarget.GetComponent<TargetElement>();
        print("11");
        newte.LightOn();
        //newte.OpenBillBoard();
    }
}
