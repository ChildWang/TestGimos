using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PengZhuangJianCe : MonoBehaviour
{
    public HighlightableObject highLight;
    // Start is called before the first frame update
    void Start()
    {
        highLight = this.gameObject.GetComponent<HighlightableObject>();
        if (highLight == null)
        {
            highLight = this.gameObject.AddComponent<HighlightableObject>();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponentInParent<BaseEquipment>())
        {
            //Debug.Log(other.gameObject.GetComponentInParent<BaseEquipment>().gameObject.name);
            LightOnError();
        }
    }


    public virtual void LightOnRight()
    {
        highLight.On(Color.green);
    }
    public virtual void LightOnError()
    {
        highLight.On(Color.red);
    }
    public virtual void LightOff()
    {
        highLight.ConstantOff();
    }

}
