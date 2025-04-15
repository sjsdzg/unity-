


using UnityEngine;


/// <summary>
/// 鼠标控制摄像机围绕模型转动以实现Game视图中的模型旋转(右键)和缩放(滚轮)
/// 右键：旋转(围绕视口中心点或者指定的中心)
/// </summary>
[AddComponentMenu("Camera-Control/3dsMax Camera Style")]
public class CameraHepler : MonoBehaviour
{
    /// <summary>
    /// 旋转轴心
    /// </summary>
    public Transform RotateAxis;

    #region public

    public Vector3 targetOffset;
    public float distance = 5.0f;
    public float maxDistance = 20f;
    public float minDistance = 0f;
    public float xSpeed = 400.0f;
    public float ySpeed = 400.0f;
    public int yMinLimit = -360;
    public int yMaxLimit = 360;
    public int zoomRate = 40;
    public float panSpeed = 0.3f;
    public float zoomDampening = 5.0f;
    #endregion

    #region private
    private float xDeg = 0.0f;
    private float yDeg = 0.0f;
    private float currentDistance;
    private float desiredDistance;
    private Quaternion currentRotation;
    private Quaternion desiredRotation;
    private Quaternion rotation;
    private Vector3 position;
    #endregion

    bool doubleClick = false;
    Vector3 positionOrgin;
    /// <summary>
    /// 视角 移动速度( 上、下、左、右、拉近、拉远)
    /// </summary>
    private float m_fMoveSpeed;
    void Start()
    {
        positionOrgin = RotateAxis.position;

        Init();
    }
    void OnEnable() { Init(); }

    public void Init()
    {
        //If there is no RotateAxis, create a temporary RotateAxis at 'distance' from the cameras current viewpoint
        if (!RotateAxis)
        {
            GameObject go = new GameObject("Cam Target");
            go.transform.position = transform.position + (transform.forward * distance);
            RotateAxis = go.transform;
        }

        distance = Vector3.Distance(transform.position, RotateAxis.position);
        currentDistance = distance;
        desiredDistance = distance;

        //be sure to grab the current rotations as starting points.
        position = transform.position;
        rotation = transform.rotation;
        currentRotation = transform.rotation;
        desiredRotation = transform.rotation;

        xDeg = Vector3.Angle(Vector3.right, transform.right);
        yDeg = Vector3.Angle(Vector3.up, transform.up);
    }

    /*
     * Camera logic on LateUpdate to only update after all character movement logic has been handled. 
     */
    void LateUpdate()
    {
        m_fMoveSpeed = Time.deltaTime;
        if (doubleClick)
        {
            RotateAxis.position = new Vector3(Mathf.MoveTowards(RotateAxis.position.x, positionOrgin.x, 100 * m_fMoveSpeed), Mathf.MoveTowards(RotateAxis.position.y, positionOrgin.y, 100 * m_fMoveSpeed), Mathf.MoveTowards(RotateAxis.position.z, positionOrgin.z, 100 * m_fMoveSpeed));
            desiredDistance =7.5f;
            if (RotateAxis.position == positionOrgin)
            {
                doubleClick = false;
            }
        }
        // If Control and Alt and Middle button? ZOOM!
        if (Input.GetMouseButton(2))
        {
            desiredDistance -= Input.GetAxis("Mouse Y") * Time.deltaTime * zoomRate * 0.125f * Mathf.Abs(desiredDistance);
        }
        else if (Input.GetMouseButton(1) ) // If middle mouse and left alt are selected? ORBIT
        {
            xDeg += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            yDeg -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

            ////////OrbitAngle

            //Clamp the vertical axis for the orbit
            yDeg = ClampAngle(yDeg, yMinLimit, yMaxLimit);
            // set camera rotation 
            desiredRotation = Quaternion.Euler(yDeg, xDeg, 0);
            currentRotation = transform.rotation;

            rotation = Quaternion.Lerp(currentRotation, desiredRotation, Time.deltaTime * zoomDampening);
            transform.rotation = rotation;
        }
        // otherwise if middle mouse is selected, we pan by way of transforming the RotateAxis in screenspace
        //上下左右 begin
        //else if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        //{
        //    RotateAxis.Translate(Vector3.up * m_fMoveSpeed, Space.Self);
        //}
        //else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        //{
        //    RotateAxis.Translate(Vector3.down * m_fMoveSpeed, Space.Self);
        //}
        //else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        //{
        //    RotateAxis.rotation = transform.rotation;
        //    RotateAxis.Translate(Vector3.left * m_fMoveSpeed, Space.Self);
        //}
        //else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        //{
        //    RotateAxis.rotation = transform.rotation;
        //    RotateAxis.Translate(Vector3.right * m_fMoveSpeed, Space.Self);
        //}//上下左右 end


        ////////Orbit Position

        // affect the desired Zoom distance if we roll the scrollwheel
        desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomRate * Mathf.Abs(desiredDistance);
        //clamp the zoom min/max
        desiredDistance = Mathf.Clamp(desiredDistance, minDistance, maxDistance);
        // For smoothing of the zoom, lerp distance
        currentDistance = Mathf.Lerp(currentDistance, desiredDistance, Time.deltaTime * zoomDampening);

        // calculate position based on the new currentDistance 
        position = RotateAxis.position - (rotation * Vector3.forward * currentDistance + targetOffset);
        transform.position = position;
    }

    void OnGUI()
    {
        if (Event.current.isMouse && Event.current.type == EventType.MouseDown && Event.current.clickCount == 2)
        {
            if (Input.GetMouseButton(1))
            {
                doubleClick = true;
            }
        }
    }

    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}