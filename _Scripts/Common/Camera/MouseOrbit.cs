
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace XFramework
{
    /// <summary>
    /// 相机观察
    /// </summary>
    public class MouseOrbit : MonoBehaviour
    {
        public class DistanceChangeEvent : UnityEvent<float> { }
        public class AngleChangedEvent : UnityEvent<Quaternion> { }

        private DistanceChangeEvent m_OnDistanceChange = new DistanceChangeEvent();
        /// <summary>
        /// 距离更改时，触发
        /// </summary>
        public DistanceChangeEvent OnDistanceChange
        {
            get { return m_OnDistanceChange; }
            set { m_OnDistanceChange = value; }
        }

        private AngleChangedEvent m_OnAngleChanged = new AngleChangedEvent();
        /// <summary>
        /// 角度更改事件
        /// </summary>
        public AngleChangedEvent OnAngleChanged
        {
            get { return m_OnAngleChanged; }
            set { m_OnAngleChanged = value; }
        }

        private UnityEvent m_OnFocusCompleted = new UnityEvent();
        /// <summary>
        /// 聚焦完成
        /// </summary>
        public UnityEvent OnFocusCompleted
        {
            get { return m_OnFocusCompleted; }
            set { m_OnFocusCompleted = value; }
        }

        /// <summary>
        /// 观察目标
        /// </summary>
        public Transform target;
        /// <summary>
        /// 最开始的Target
        /// </summary>
        Transform originTarget;

        public float distance = 70;
        /// <summary>
        /// 相机到目标距离
        /// </summary>
        public float Distance
        {
            get { return distance; }
            set
            {
                distance = value;
                OnDistanceChange.Invoke(distance);
            }
        }

        /// <summary>
        /// 相机离目标最小距离
        /// </summary>
        public float minDistance = 3f;

        /// <summary>
        /// 相机离目标最大距离
        /// </summary>
        public float maxDistance = 10;

        /// <summary>
        /// 鼠标中间滚动控制相机前后移动速度
        /// </summary>
        public int scrollSpeed = 20;

        /// <summary>
        /// 相机旋转角度在X轴上限制
        /// </summary>
        public int yMaxLimit = 90;

        /// <summary>
        /// 相机旋转角度在Y轴上限制
        /// </summary>
        public int yMinLimit = -90;

        /// <summary>
        /// 按下鼠标右键旋转视角时X轴旋转速度
        /// </summary>
        private float xSpeed = 250.0f;

        /// <summary>
        /// 按下鼠标右键旋转视角时Y轴旋转速度
        /// </summary>
        private float ySpeed = 150.0f;

        /// <summary>
        /// 按下鼠标中建目标移动速度
        /// </summary>
        public float panSpeed = 1;

        /// <summary>
        /// 沿水平面移动
        /// </summary>
        public bool panHorizontal = false;

        /// <summary>
        /// 平滑速率
        /// </summary>
        public float lerpSpeed = 10f;

        /// <summary>
        /// 相机在X轴上的角度
        /// </summary>
        private float x = 0.0f;

        /// <summary>
        /// 相机在Y轴上的角度
        /// </summary>
        private float y = 0.0f;

        /// <summary>
        /// 相机位置
        /// </summary>
        private Vector3 position;

        /// <summary>
        /// 相机旋转角度
        /// </summary>
        private Quaternion rotation;

        private Quaternion cacheRotation;
        /// <summary>
        /// 缓存相机旋转角度
        /// </summary>
        public Quaternion CacheRotation
        {
            get { return cacheRotation; }
            set
            {
                cacheRotation = value;
                OnAngleChanged.Invoke(cacheRotation);
            }
        }

        /// <summary>
        /// 相机到目标距离占最大距离比例，用来限制相机离目标很近的时候的移动速度
        /// </summary>
        private float panRate;

        /// <summary>
        /// 是否可以交互
        /// </summary>
        public bool Interactable = true;

        /// <summary>
        /// 相机插值移动时限制移动和旋转
        /// </summary>
        //private bool IsObserve = true;

        /// <summary>
        /// 相机原始位置
        /// </summary>
        private Vector3 rawCameraPos;

        /// <summary>
        /// 
        /// </summary>
        private Quaternion rawCameraQuate;

        /// <summary>
        /// 目标原始位置
        /// </summary>
        private Vector3 rawTargetPos;

        /// <summary>
        /// 受影响层，在该层不触发
        /// </summary>
        public LayerMask blockedLayer;

        /// <summary>
        /// 是否平滑插值
        /// </summary>
        public bool smooth = false;

        void Awake()
        {
            originTarget = target;
            //目标与摄像机之间的距离
            Distance = Vector3.Distance(target.position, transform.position);
            //相机欧拉角
            Vector3 angle = transform.eulerAngles;
            y = angle.x;
            x = angle.y;
            //相机位置和角度
            rotation = Quaternion.Euler(y, x, 0);
            position = rotation * new Vector3(0.0f, 0.0f, -Distance) + target.position;
            transform.rotation = rotation;
            transform.position = position;
            //transform.LookAt(target, Vector3.up);//摄像机对着目标

            rawCameraPos = transform.position;
            rawCameraQuate = transform.rotation;
            rawTargetPos = target.position;
        }
        private void OnEnable()
        {
            //transform.LookAt(target);//摄像机对着目标
            //IsObserve = true;
        }
        private void OnDisable()
        {

            StopAllCoroutines();
        }
        void LateUpdate()
        {
            if (!Interactable)
                return;

            if (InCoroutine)
                return;

            //rate
            panRate = Distance / maxDistance;

            //旋转
            if (Input.GetMouseButton(1) && !IsBlocked())
            {
                x += Input.GetAxis("Mouse X") * (xSpeed) * 0.01f;
                y -= Input.GetAxis("Mouse Y") * (ySpeed) * 0.01f;
                y = ClampAngle(this.y, (float)yMinLimit, (float)yMaxLimit);//从最小大最大变化
                if (isRotatableFocus)
                {
                    hasRotated = true;
                }
            }
            //平移
            else if (Input.GetMouseButton(2) && !IsBlocked())//按住中键，改变相机位置
            {
                position += transform.right * -Input.GetAxis("Mouse X") * panSpeed * panRate;
                if (panHorizontal)
                {
                    Vector3 direction = Vector3.Cross(transform.right, Vector3.down);
                    position += direction * Input.GetAxis("Mouse Y") * panSpeed * panRate;
                }
                else
                {
                    position += transform.up * -Input.GetAxis("Mouse Y") * panSpeed * panRate;
                }
                rotation = Quaternion.Euler(y, x, 0);
                target.position = rotation * new Vector3(0.0f, 0.0f, Distance) + position;
            }
            //缩放
            if (Input.GetAxis("Mouse ScrollWheel") != 0 && !IsBlocked())//滚动中键，视角缩放
            {
                rotation = Quaternion.Euler(y, x, 0);
                Distance = Mathf.Clamp(Distance - Input.GetAxis("Mouse ScrollWheel") * scrollSpeed, minDistance, maxDistance);
            }

            rotation = Quaternion.Euler(y, x, 0);
            position = rotation * new Vector3(0.0f, 0.0f, -Distance) + target.position;

            //平滑
            if (smooth)
            {
                transform.position = Vector3.Lerp(transform.position, position, lerpSpeed * Time.deltaTime);
                CacheRotation = Quaternion.Lerp(transform.rotation, rotation, lerpSpeed * Time.deltaTime);
                transform.rotation = cacheRotation;
            }
            else
            {
                transform.position = position;
                CacheRotation = rotation;
                transform.rotation = CacheRotation;
            }

            if (IsRotatableFocus&&!hasRotated)
            {
                transform.position = target.position;
                transform.rotation = target.rotation;
            }
        }

        /// <summary>
        /// 相机角度限制
        /// </summary>
        private static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360f)
            {
                angle += 360f;
            }
            if (angle > 360f)
            {
                angle -= 360f;
            }
            return Mathf.Clamp(angle, min, max);
        }


        /// <summary>
        /// 焦距
        /// </summary>
        /// <param name="cameraPos"></param>
        /// <param name="targetPos"></param>
        public void Focus(Vector3 cameraPos, Vector3 targetPos, UnityAction callback = null)
        {
            StopAllCoroutines();//停止所有协同程序
            InCoroutine = false;
            if (!InCoroutine)
            {
                InCoroutine = true;
                StartCoroutine(WaitAndPrint(cameraPos, targetPos, callback));//开始这个名叫WaitAndPrint的协同程序
            }
        }

        /// <summary>
        /// 焦距
        /// </summary>
        /// <param name="cameraPos"></param>
        /// <param name="targetPos"></param>
        public void Focus(Transform camera, Transform target, UnityAction callback = null, bool immediate = false)
        {
            StopAllCoroutines();//停止所有协同程序
            InCoroutine = false;
            if (!InCoroutine)
            {
                InCoroutine = true;
                StartCoroutine(WaitAndPrint(camera.position, camera.rotation, target.position,callback));//开始这个名叫WaitAndPrint的协同程序
            }
        }

        public void Focus(Transform camera, UnityAction callback= null, bool immediate = false)
        {
            
        }

        /// <summary>
        /// 焦距
        /// </summary>
        /// <param name="targetPos"></param>
        /// <param name="distance"></param>
        public void Focus(Vector3 targetPos, float distance)
        {
            StopAllCoroutines();
            var offset = transform.position - targetPos;
            Vector3 cameraPos = targetPos + Vector3.ClampMagnitude(offset, distance);
            InCoroutine = false;
            if (!InCoroutine)
            {
                InCoroutine = true;
                StartCoroutine(WaitAndPrint(cameraPos, targetPos));//开始这个名叫WaitAndPrint的协同程序
            }
        }

        private bool InCoroutine = false;//是否在执行携程
        /// <summary>
        /// 移动相机到最佳位置zuijia是相机位置,模型是需要观察模型位置用来赋值给观察目标CameraTarget
        /// </summary>
        /// <param name="position_camera"></param>
        /// <param name="position_target"></param>
        /// <returns></returns>
        IEnumerator WaitAndPrint(Vector3 position_camera, Vector3 position_target,UnityAction callback=null)
        {
            InCoroutine = true;
            float progressValue = 0;//进度值
            while (progressValue <= 1)
            {
                target.position = Vector3.Lerp(target.position, position_target, progressValue);
                transform.position = Vector3.Lerp(transform.position, position_camera, progressValue);
                transform.LookAt(position_target);
                CacheRotation = transform.rotation;
                progressValue += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            Distance = Vector3.Distance(transform.position, target.position);
            Vector3 angles = transform.eulerAngles;
            x = angles.y;
            y = angles.x;
            if (callback != null)
            {
                callback.Invoke();
            }
            //聚焦完成触发
            OnFocusCompleted.Invoke();
            InCoroutine = false;
        }

        /// <summary>
        /// 移动相机到最佳位置zuijia是相机位置,模型是需要观察模型位置用来赋值给观察目标CameraTarget
        /// </summary>
        /// <param name="position_camera"></param>
        /// <param name="position_target"></param>
        /// <returns></returns>
        IEnumerator WaitAndPrint(Vector3 position_camera, Quaternion rotation_camera, Vector3 position_target,UnityAction callback=null)
        {
            InCoroutine = true;
            float progressValue = 0;//进度值
            while (progressValue <= 1)
            {
                target.position = Vector3.Lerp(target.position, position_target, progressValue);
                transform.position = Vector3.Lerp(transform.position, position_camera, progressValue);
                CacheRotation = Quaternion.Slerp(transform.rotation, rotation_camera, progressValue);
                transform.rotation = CacheRotation;
                progressValue += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            Distance = Vector3.Distance(transform.position, target.position);
            Vector3 angles = transform.eulerAngles;
            x = angles.y;
            y = angles.x;
            if (callback!=null)
            {
                callback.Invoke();
            }
            
            //聚焦完成触发
            OnFocusCompleted.Invoke();
            InCoroutine = false;
        }

        /// <summary>
        /// 是否堵塞
        /// </summary>
        /// <returns></returns>
        public bool IsBlocked()
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.pressPosition = Input.mousePosition;
            eventData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            if (results.Count > 0)
            {
                int value = (1 << results[0].gameObject.layer) & blockedLayer;
                if (value == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            StopAllCoroutines();//停止所有协同程序
            InCoroutine = false;
            if (!InCoroutine)
            {
                InCoroutine = true;
                StartCoroutine(WaitAndPrint(rawCameraPos, rawCameraQuate, rawTargetPos));//开始这个名叫WaitAndPrint的协同程序
            }
        }

        /// <summary>
        /// 设置Distance
        /// </summary>
        /// <param name="dis"></param>
        public void SetDistance(float dis)
        {
            if (InCoroutine)
                return;
            rotation = Quaternion.Euler(y, x, 0);
            distance = Mathf.Clamp(dis, minDistance, maxDistance);
            transform.position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_x"></param>
        /// <param name="_y"></param>
        public void SetY(float _y)
        {
            if (InCoroutine)
                return;
            y = _y;
        }

        /// <summary>
        /// 放大或缩小
        /// </summary>
        /// <param name="value"></param>
        public void Zoom(float value)
        {
            Distance += value;
            Distance = Mathf.Clamp(distance, minDistance, maxDistance);
        }

        /// <summary>
        /// 绕轴心点旋转
        /// </summary>
        /// <param name="_x"></param>
        /// <param name="_y"></param>
        public void Rotate(float _x, float _y = 0)
        {
            if (InCoroutine)
                return;
            x += _x;
            y -= _y;
        }


        //[HideInInspector]
        //public Transform fixedPoint;

        //public void SetFixedPointMode()
        //{
        //    isFixedPointMode = true;
        //    Distance = 0;
        //    minDistance = 0;
        //    maxDistance = 0;
        //}

        private bool hasRotated;

        private bool isRotatableFocus;

        public bool IsRotatableFocus
        {
            get
            {
                return isRotatableFocus;
            }
            set
            {
                isRotatableFocus = value;
                if (!isRotatableFocus)
                {
                    target = originTarget;
                }
            }
        }
        
        /// <summary>
        /// 相机移动至一个点, 只能在该点旋转，相机初始Transform等于该点
        /// </summary>
        /// <param name="point"></param>
        public void RotatableFocus(Transform point)
        {
            InCoroutine = false;
            IsRotatableFocus = true;
            hasRotated = false;
            target = point;
            lastRotatableFocus = point;
            Distance = 0;
            minDistance = 0;
            maxDistance = 0;
            position = point.position;
            rotation = point.rotation;
        }

        Transform lastRotatableFocus;

        public void RotatableFocus()
        {
            if (lastRotatableFocus!=null)
            {
                RotatableFocus(lastRotatableFocus);
            }
        }
    }
}