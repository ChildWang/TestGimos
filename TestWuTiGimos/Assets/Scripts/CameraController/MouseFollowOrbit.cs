using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
public enum ExerciseState : byte
{
    ComRota = 0,
    Move = 1,
    Scale = 2,
    Autobiography = 4
}
public class MouseFollowOrbit : MonoBehaviour
{
    public GameObject target;

    // public UIScrollBar scroll;
    public float speedRate = 1;
    private float xSpeed = 100, ySpeed = 100, mSpeed = 2;//x,y缩放m的速度
    public float yMinLimit = -90, yMaxLimit = 90;
    public float distance = 5f, maxDistance = 10f;
    public float minDistance = 0.01f;
    public bool needDamping = true;//平滑
    public float downMoveYDistance;//向下平移距离
    public float upMoveYDistance;//向上平移距离

    public float moveXSpeed = 0.5f;//平移x速度
    public float moveYSpeed = 0.5f;//平移y速度
    public float damping = 5.0f;//平滑值

    public float x = 0.0f;
    public float y = 0.0f;

    private GameObject targetTemp;
    public Vector3 cameraPositionTemp;
    public Vector3 cameraAngleTemp;
    private float xTemp = 0;
    private float yTemp = 0;

    private float xMove = 0;
    private float yMove = 0;
    private Vector3 currentCameraPosition = Vector3.zero;

    public float anglespeed = 50f;
    //    private Vector3 panOffset = Vector3.zero;
    public float IdealDistance;
    //private GUIStyle StyleToolTip;
    //public GUISkin skinToolTip;
    public bool change;
    public Texture2D texture;
    //Controlmobile cot;


    public enum MoveDirection : byte
    {
        up = 1,
        down = 2,
        left = 3,
        right = 4,
        foword = 5,
        back = 6
    }

    private void Awake()
    {
        MySingleton.getInstance<PhysicsRaycasterManager>();
    }
    public ExerciseState State;
    // Use this for initialization
    void Start()
    {
        CreatePivot();
        targetTemp.transform.position = target.transform.position;

        IdealDistance = distance;
        Vector3 angles = Camera.main.transform.eulerAngles;
        x = angles.y;
        y = angles.x;
        cameraPositionTemp = Camera.main.transform.position;
        cameraAngleTemp = Camera.main.transform.eulerAngles;
        xTemp = x;
        yTemp = y;
        State = ExerciseState.ComRota;
        //StyleToolTip = skinToolTip.label;
        InitCamera();

    }

    private void Update()
    {
        FunMouseCtrol();
    }
    void CreatePivot()
    {
        targetTemp = new GameObject("Pivot");
        MeshFilter mf = targetTemp.AddComponent<MeshFilter>();
        MeshRenderer mr = targetTemp.AddComponent<MeshRenderer>();

        Vector3[] vertices = new Vector3[12];
        int[] triangles = new int[12];
        float a = 0.1f;
        Quaternion qt = Quaternion.Euler(90, 0, 0);
        //组织顶点,z轴（0,0,a）,顶点轴心
        Vector3 p0 = new Vector3(-a * 2 * Mathf.Sqrt(2) / 3, 0, -a / 3);

        Vector3 p1 = new Vector3(a * Mathf.Sqrt(2) / 3, a * Mathf.Sqrt(6) / 3, -a / 3);

        Vector3 p2 = new Vector3(a * Mathf.Sqrt(2) / 3, -a * Mathf.Sqrt(6) / 3, -a / 3);
        Vector3 p3 = new Vector3(0, 0, a);

        //绕x轴转-90度
        float angle = -90;
        Matrix4x4 matrix4X4 = Matrix4x4.identity;
        matrix4X4.m11 = Mathf.Cos(angle * Mathf.Deg2Rad);
        matrix4X4.m12 = -Mathf.Sin(angle * Mathf.Deg2Rad);
        matrix4X4.m21 = Mathf.Sin(angle * Mathf.Deg2Rad);
        matrix4X4.m22 = Mathf.Cos(angle * Mathf.Deg2Rad);

        p0 = matrix4X4 * p0;
        p1 = matrix4X4 * p1;
        p2 = matrix4X4 * p2;
        p3 = matrix4X4 * p3;

        Mesh mesh = mf.sharedMesh;
        if (mesh == null)
        {
            mf.mesh = new Mesh();
            mesh = mf.sharedMesh;
        }
        mesh.Clear();
        mesh.vertices = new Vector3[] {
                p0,p1,p2,
                p0,p2,p3,
                p2,p1,p3,
                p0,p3,p1
            };
        mesh.triangles = new int[]{
                0,1,2,
                3,4,5,
                6,7,8,
                9,10,11
                };
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.RecalculateTangents();
        Material mt = new Material(Shader.Find("Standard"));
        mt.color = new Color(Color.green.r, Color.green.g, Color.green.b, 0.3f);
        mt.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        mt.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mt.SetInt("_ZWrite", 0);
        mt.DisableKeyword("_ALPHATEST_ON");
        mt.DisableKeyword("_ALPHABLEND_ON");
        mt.EnableKeyword("_ALPHAPREMULTIPLY_ON");
        mt.renderQueue = 3000;
        mr.material = mt;
    }
    void FunMouseCtrol()
    {
        if (target)
        {
            if (Input.GetMouseButtonDown(1))
            {
                State = ExerciseState.ComRota;
                //if (Singleton.getInstance<ComTool>().autobiographyGameObject == null)
                //{
                //    State = ExerciseState.ComRota;                       
                //}
                //else
                //{
                //    State = ExerciseState.Autobiography;                     
                //}

                // 3d功能区的操作
            }
            if (Input.GetMouseButtonDown(0))
            {

                //State=state.Rota;

                // 3d功能区的操作

            }
            //缩放

            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                State = ExerciseState.Scale;
            }
            if (Input.GetKeyDown(KeyCode.KeypadMinus) || Input.GetKeyDown(KeyCode.KeypadPlus) || Input.GetKeyDown(KeyCode.Equals) || Input.GetKeyDown(KeyCode.Minus))
            {
                State = ExerciseState.Scale;
            }


            //旋转
            if (State == ExerciseState.ComRota || State == ExerciseState.Autobiography)
            {
                if (Input.GetMouseButton(1))
                {
                    x += Input.GetAxis("Mouse X") * xSpeed * speedRate;
                    y -= Input.GetAxis("Mouse Y") * ySpeed * speedRate;
                }
                y = ClampAngle(y, yMinLimit, yMaxLimit);
                Quaternion rotation = Quaternion.Euler(y, x, 0.0f);
                Vector3 disVector = new Vector3(0.0f, 0.0f, -distance);
                Vector3 position = rotation * disVector + targetTemp.transform.position;
                if (needDamping)
                {
                    //panOffset = Vector3.Lerp(panOffset, distance, Time.deltaTime * damping);
                    Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, rotation, damping * Time.deltaTime);
                    //transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * damping);
                    Camera.main.transform.position = targetTemp.transform.position - distance * Camera.main.transform.forward;
                    //transform.LookAt(target);
                }
                else
                {
                    Camera.main.transform.rotation = rotation;
                    Camera.main.transform.position = position;
                }
                FireEvent(Camera.main.transform.localEulerAngles.y);
            }
            else if (State == ExerciseState.Scale)
            {
                ChangeScale();
            }
            else if (State == ExerciseState.Autobiography)
            {
                if (Input.GetMouseButton(1))
                {
                    //Singleton.getInstance<ComTool>().autobiographyGameObject.transform.Rotate(Camera.main.transform.right, Input.GetAxis("Mouse Y") * xSpeed * Time.deltaTime, Space.World);
                    //Singleton.getInstance<ComTool>().autobiographyGameObject.transform.Rotate(-Camera.main.transform.up, Input.GetAxis("Mouse X") * xSpeed * Time.deltaTime, Space.World);
                    //  Camera.main.transform.position =  Camera.main.transform.position

                }

            }
        }

        if (Input.GetMouseButtonDown(2))
        {
            State = ExerciseState.Move;
            // 3d功能区的操作
            // 获取当前摄像机位置
            GetCurrentCameraPosition();

        }
        //平移
        if (Input.GetMouseButton(2))
        {
            targetTemp.transform.Translate(-Camera.main.transform.right * Input.GetAxis("Mouse X") * moveXSpeed * distance * Time.deltaTime - Camera.main.transform.up * Input.GetAxis("Mouse Y") * moveYSpeed * distance * Time.deltaTime, Space.World);
            Camera.main.transform.Translate(-Camera.main.transform.right * Input.GetAxis("Mouse X") * moveXSpeed * distance * Time.deltaTime - Camera.main.transform.up * Input.GetAxis("Mouse Y") * moveYSpeed * distance * Time.deltaTime, Space.World);

        }
    }
    public void SetTarget(GameObject go)
    {
        Vector3 targetCameraPos = go.transform.position - Camera.main.transform.forward * distance;

        Camera.main.transform.DOMove(targetCameraPos, 0.3f).OnComplete(() =>
        {
            target = go;
            targetTemp.transform.position = target.transform.position;
        });

    }
    public void KeyDownRotate(string rot)
    {
        State = ExerciseState.ComRota;
        y = y % 360;
        x = x % 360;
        switch (rot)
        {
            case "upRotation":
                y = y - anglespeed;
                break;
            case "leftRotation":
                x = x - (anglespeed);
                break;
            case "rightRotation":
                x = x + (anglespeed);
                break;
            case "downRotation":
                y = y + (anglespeed);
                break;
        }
    }


    static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }

    public void valeChange()
    {
        //scroll.value = IdealDistance / (maxDistance - minDistance);
        //IdealDistance = (maxDistance - minDistance) * scroll.value;
    }
    /// <summary>
    /// 摄像机复位
    /// </summary>
    public void InitCamera()
    {
        Camera.main.transform.position = cameraPositionTemp;
        Camera.main.transform.eulerAngles = cameraAngleTemp;
        x = xTemp;
        y = yTemp;
        target = targetTemp;
        isAlreadyFire = new bool[yMinAngles.Length];
    }
    /// <summary>
    /// 获取当前摄像机位置
    /// </summary>
    public void GetCurrentCameraPosition()
    {
        currentCameraPosition = Camera.main.transform.position;
        xMove = 0;
        yMove = 0;
    }
    public void ChangeScale()
    {
        distance = Mathf.Lerp(distance, IdealDistance, damping * Time.deltaTime);//平滑距离变化
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
            IdealDistance -= Input.GetAxis("Mouse ScrollWheel") * distance * mSpeed;//滚动时距离变化
        else if (Input.GetKey(KeyCode.KeypadMinus) || Input.GetKey(KeyCode.Minus)) IdealDistance += 0.2f;
        else if (Input.GetKey(KeyCode.KeypadPlus) || Input.GetKey(KeyCode.Equals)) IdealDistance -= 0.2f;

        distance = Mathf.Clamp(distance, minDistance, maxDistance);//限制距离
        IdealDistance = Mathf.Clamp(IdealDistance, minDistance, maxDistance);//限制距离

        Quaternion rotation = Quaternion.Euler(y, x, 0.0f);
        Vector3 disVector = new Vector3(0.0f, 0.0f, -distance);
        Vector3 position = rotation * disVector + targetTemp.transform.position;
        if (needDamping)
        {
            Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, rotation, damping * Time.deltaTime);
            Camera.main.transform.position = targetTemp.transform.position - distance * Camera.main.transform.forward;
        }
        else
        {
            Camera.main.transform.rotation = rotation;
            Camera.main.transform.position = position;
        }
    }
    public float[] yMinAngles;
    public float[] yMaxAngles;
    public bool[] isAlreadyFire;
    private void FireEvent(float yAngle)
    {
        for (int i = 0; i < yMinAngles.Length; i++)
        {
            if (yAngle > yMinAngles[i] && yAngle < yMaxAngles[i])
            {
                if (!isAlreadyFire[i])
                {
                    //EventCenter.CameraEvent.RaiseCameraReachAngle(i, true);
                    isAlreadyFire[i] = true;
                }
            }
            else
            {
                if (isAlreadyFire[i])
                {
                    //EventCenter.CameraEvent.RaiseCameraReachAngle(i, false);
                    isAlreadyFire[i] = false;
                }
            }
        }
    }

}
