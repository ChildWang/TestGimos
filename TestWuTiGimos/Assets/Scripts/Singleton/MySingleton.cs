

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MySingleton : MonoBehaviour
{

    private static GameObject m_Container = null;
    private static string name = "Singleton";
    private static Dictionary<string, MonoBehaviour> map = new Dictionary<string, MonoBehaviour>();
    private static bool m_isDestroying = false;

    public static MonoBehaviour getMono(string name)
    {
        if (map.ContainsKey(name))
        {
            return map[name];
        }
        else
            return null;
    }


    public static bool IsDestroying
    {
        get { return m_isDestroying; }
    }

    public static bool IsCreatedInstance<T>() where T : MonoBehaviour
    {
        if (m_Container == null)
        {
            return false;
        }
        if (map != null && map.ContainsKey(typeof(T).ToString()))
        {
            return true;
        }
        return false;
    }

    public static T Instance<T>() where T : MonoBehaviour
    {
        GameObject obj = GameObject.Find("MyMode");
        if (obj)
        {
            if (obj.GetComponent<T>())
            {
                return obj.GetComponent<T>();
            }
            else
            {
                return obj.AddComponent<T>();
            }
        }
        else
        {
            obj = new GameObject();
            obj.name = "MyMode";
            return obj.AddComponent<T>();
        }
    }


    public static T getInstance<T>() where T : MonoBehaviour
    {
        if (m_Container==null)
        {
            m_Container=new GameObject();
            m_Container.name=name;
            m_Container.AddComponent(typeof(MySingleton));
        }
        if (!map.ContainsKey(typeof(T).ToString()))
        {
            if (System.Type.GetType(typeof(T).ToString()) != null)
            {
                map.Add(typeof(T).ToString(), m_Container.AddComponent<T>());
            }
            else
            {
                Debug.LogWarning("Singleton Type ERROR! (" + typeof(T).ToString() + ")");
            }
        }
        return (T)map[typeof(T).ToString()];
    }

    public static void  RemoveInstance<T>() where T : MonoBehaviour
    {
        if (m_Container != null && map.ContainsKey(typeof(T).ToString()))
        {
            UnityEngine.Object.Destroy((UnityEngine.Object)(map[typeof(T).ToString()]));
            map.Remove(typeof(T).ToString());
            Debug.LogWarning("Singleton REMOVE! (" + typeof(T).ToString() + ")");
        }
    }

    void Awake()
    {        
        DontDestroyOnLoad(gameObject);
    }
    public static void OnApplicationQuit()
    {   
        if (m_Container != null)
        {
            GameObject.Destroy(m_Container);
            m_Container = null;
            m_isDestroying = true;
            map.Clear();
        }

    }
}
