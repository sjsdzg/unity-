using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using XFramework.Core;

namespace XFramework.Common
{
    public delegate void UsableDelegate(UsableComponent usable);

    /// <summary>
    /// 选择器
    /// </summary>
    public class Selector : MonoBehaviour
    {
        public class UsalbeEvent : UnityEvent<UsableComponent> { }

        /// <summary>
        /// Specifies how to target: center of screen or under the mouse cursor.
        /// </summary>
        public enum SelectAt
        {
            CenterOfScreen,
            MouserPosition,
            CustomPosition,
        }

        /// <summary>
        /// Specifies whether to compute range from the targeted object (distance to the camera
        /// or distance to the selector's game object). 
        /// </summary>
        public enum DistanceFrom
        {
            Gamera,
            Player,
        }

        /// <summary>
        /// Player
        /// </summary>
        public Transform Player { get; set; }

        /// <summary>
        /// The layer mask to use when targeting objects. Objects on others layers are ignored.
        /// </summary>
        public LayerMask layerMask = 1;
        
        /// <summary>
        /// 选择模式
        /// </summary>
        public SelectAt selectAt = SelectAt.CenterOfScreen;

        /// <summary>
        /// How to compute range to targeted object. Default is from the camera.
        /// </summary>
        public DistanceFrom distanceFrom = DistanceFrom.Gamera;

        /// <summary>
        /// The max selection distance. The selector won't target objects farther than this.
        /// </summary>
        public float maxSelectionDistance = 5f;

        /// <summary>
        /// Set <c>true</c> to check all objects within the raycast range for usables.
        /// If <c>false</c>, the check stops on the first hit, even if it's not a usable.
        /// This prevents selection through walls.
        /// </summary>
        public bool raycastAll = false;

        /// <summary>
        /// The key that sends an OnUse message.
        /// </summary>
        public KeyCode useKey = KeyCode.F;

        /// <summary>
        /// Gets or sets the custom position used when the selectAt is set to SelectAt.CustomPosition.
        /// You can use, for example, to slide around a targeting icon onscreen using a gamepad.
        /// </summary>
        public Vector3 CustomPosition { get; set; }

        /// <summary>
        /// Gets the current selection.
        /// </summary>
        /// <value>The selection.</value>
        public UsableComponent CurrentUsable { get { return usable; } }

        /// <summary>
        /// Gets the distance from the current usable.
        /// </summary>
        /// <value>The current distance.</value>
        public float CurrentDistance { get { return distance; } }

        private UsalbeEvent m_OnSelected = new UsalbeEvent();
        /// <summary>
        /// 选中Usable物体
        /// </summary>
        public UsalbeEvent OnSelected
        {
            get { return m_OnSelected ; }
            set { m_OnSelected  = value; }
        }

        private UsalbeEvent m_OnDeselected = new UsalbeEvent();
        /// <summary>
        /// 未选中Usable物体
        /// </summary>
        public UsalbeEvent OnDeselected
        {
            get { return m_OnDeselected; }
            set { m_OnDeselected = value; }
        }

        #region selection
        protected GameObject selection = null; // Currently under the selection point.
        protected UsableComponent usable = null; // Usable component of the current selection.
        protected float distance = 0;
        #endregion

        #region last
        protected Ray lastRay = new Ray();
        protected RaycastHit lastHit = new RaycastHit();
        protected RaycastHit[] lastHits = new RaycastHit[0];
        #endregion

        void Start()
        {
            if (Camera.main == null)
            {
                Debug.LogError("The scene is missing a camera tagged 'MainCamera'. ");
            }
        }

        void Update()
        {
            // Exit if disabled or paused:
            if (!enabled || (Time.timeScale <= 0)) return;
            // Exit if there's no main camera:
            if (Camera.main == null) return;
            // Exit if using mouse selection and is over a UI element:
            if ((selectAt == SelectAt.MouserPosition) && Utils.IsPointerOverUI()) return;
            // Raycast 3D
            RunRaycast();
            //if the player presses the use key/button on a target
            if (OnKeyDown() && (usable != null) && !usable.Disable)
            {
                if (distance <= usable.maxUseDistance)
                {
                    usable.OnUse();
                }
                else
                {
                    Debug.Log("distance > usable.maxUseDistance");
                }
            }
        }


        protected void RunRaycast()
        {
            Ray ray = Camera.main.ScreenPointToRay(GetSelectionPoint());
            lastRay = ray;

            if (raycastAll)
            {
                RaycastHit[] hits = Physics.RaycastAll(ray, maxSelectionDistance, layerMask);
                lastRay = ray;
                lastHits = hits;
                bool foundUsable = false;
                foreach (var hit in hits)
                {
                    //碰撞距离
                    float hitDistance = 0;
                    if (distanceFrom == DistanceFrom.Gamera)
                    {
                        hitDistance = hit.distance;
                    }
                    else
                    {
                        if (Player != null)
                        {
                            hitDistance = Vector3.Distance(Player.position + new Vector3(0, 1, 0), hit.collider.transform.position);
                        }
                    }
                    //selection
                    if (selection == hit.collider.gameObject)
                    {
                        foundUsable = true;
                        distance = hitDistance;
                        break;
                    }
                    else
                    {
                        UsableComponent hitUsable = hit.collider.gameObject.GetComponent<UsableComponent>();
                        if (hitUsable != null && hitUsable.mode == UsableComponent.Mode.RayCastHit)
                        {
                            foundUsable = true;
                            distance = hitDistance;
                            usable = hitUsable;
                            selection = hit.collider.gameObject;
                            if (distance <= usable.maxUseDistance)
                            {
                                OnSelected.Invoke(usable);
                                //EventDispatcher
                                if (!usable.Disable)
                                {
                                    EventDispatcher.ExecuteEvent(Events.Selector.Select, this, usable);
                                }
                            }
                            else
                            {
                                Deselect();
                            }
                            break;
                        }
                    }
                }
                //没使用
                if (!foundUsable)
                {
                    Deselect();
                }
            }
            else
            {
                // Cast a ray and see what we hit:
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, maxSelectionDistance, layerMask))
                {
                    if (distanceFrom == DistanceFrom.Gamera)
                    {
                        distance = hit.distance;
                    }
                    else
                    {
                        //distance = Vector3.Distance(transform.position, hit.collider.transform.position);
                        if (Player != null)
                        {
                            distance = Vector3.Distance(Player.position + new Vector3(0, 1, 0), hit.collider.transform.position);
                        }
                    }
                    //selection
                    if (selection != hit.collider.gameObject)
                    {
                        UsableComponent hitUsable = hit.collider.gameObject.GetComponent<UsableComponent>();
                        if (hitUsable != null && hitUsable.mode == UsableComponent.Mode.RayCastHit)
                        {
                            usable = hitUsable;
                            selection = hit.collider.gameObject;
                            if (distance <= usable.maxUseDistance)
                            {
                                OnSelected.Invoke(usable);
                                //EventDispatcher
                                if (!usable.Disable)
                                {
                                    EventDispatcher.ExecuteEvent(Events.Selector.Select, usable.useName, usable.useMessage);
                                }
                            }
                            else
                            {
                                Deselect();
                            }
                        }
                        else
                        {
                            Deselect();
                        }
                    }
                }
                else
                {
                    Deselect();
                }
                //lastHit
                lastHit = hit;
            }
        }

        /// <summary>
        /// Deselect
        /// </summary>
        protected void Deselect()
        {
            if (usable != null)
            {
                OnDeselected.Invoke(usable);
                //EventDispatcher
                EventDispatcher.ExecuteEvent(Events.Selector.Deselect);
            }
            usable = null;
            selection = null;
        }

        protected bool OnKeyDown()
        {
            if ((useKey != KeyCode.None) && Input.GetKeyDown(useKey))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取射线起点
        /// </summary>
        /// <returns></returns>
        protected Vector3 GetSelectionPoint()
        {
            Vector3 pos = Vector3.zero;
            switch (selectAt)
            {
                case SelectAt.CenterOfScreen:
                    pos = new Vector3(Screen.width / 2, Screen.height / 2);
                    break;
                case SelectAt.MouserPosition:
                    pos = Input.mousePosition;
                    break;
                case SelectAt.CustomPosition:
                    pos = CustomPosition;
                    break;
                default:
                    break;
            }
            return pos;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(lastRay.origin, lastRay.origin + lastRay.direction * maxSelectionDistance);
            if (raycastAll)
            {
                foreach (var hit in lastHits)
                {
                    bool hasUsable = (hit.collider.GetComponent<UsableComponent>() != null);
                    Gizmos.color = hasUsable ? Color.green : Color.red;
                    Gizmos.DrawWireSphere(hit.point, 0.2f);
                }
            }
            else
            {
                if (lastHit.collider != null)
                {
                    bool hasUsable = (lastHit.collider.GetComponent<UsableComponent>() != null);
                    Gizmos.color = hasUsable ? Color.green : Color.red;
                    Gizmos.DrawWireSphere(lastHit.point, 0.2f);
                }
            }
        }

    }
}


