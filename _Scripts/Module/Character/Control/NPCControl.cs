using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityStandardAssets.Characters.ThirdPerson;
using XFramework.Core;
using XFramework.UI;
using System.Collections;
using UnityEngine.Events;

namespace XFramework.Module
{
    [RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class NPCControl : MonoBehaviour, IPointerClickHandler, IDropHandler
    {
        public UnityEngine.AI.NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
        public ThirdPersonCharacter character { get; private set; } // the character we are controlling
        public Transform target;                                    // target to aim for

        /// <summary>
        /// 自身投射器
        /// </summary>
        public Projector projector;

        /// <summary>
        /// NPC名称
        /// </summary>
        public TextMeshPro textName;

        /// <summary>
        /// 刚体
        /// </summary>
        private Rigidbody m_Rigidbody;

        /// <summary>
        /// 定义NPC对话数据
        /// </summary>
        private List<string> dialouges = new List<string>();

        /// <summary>
        /// 玩家
        /// </summary>
        public GameObject player;

        /// <summary>
        /// QA的有效距离
        /// </summary>
        public float ActiveDistance = 1f;


        private EntityNPC entity = null;
        /// <summary>
        /// 对应实体
        /// </summary>
        public EntityNPC Entity
        {
            get { return entity; }
            set { entity = value; }
        }

        /// <summary>
        /// 是否激活
        /// </summary>
        public bool Active { get;  set; }


        private BeforeEvent m_DropEvent = new BeforeEvent();

        /// <summary>
        /// 放置的事件
        /// </summary>
        public BeforeEvent OnDropEvent
        {
            get { return m_DropEvent; }
            set { m_DropEvent = value; }
        }

        private bool affectedByNPC = true;
        /// <summary>
        /// 按钮被QA影响
        /// </summary>
        public bool AffectedByNPC
        {
            get { return affectedByNPC; }
            set { affectedByNPC = value; }
        }

        private bool isJumping = false;
        /// <summary>
        /// 
        /// </summary>
        public bool IsJumping
        {
            get { return isJumping; }
            set { isJumping = value; }
        }
        private bool isMoving = false;
        private void Start()
        {
            // get the components on the object we need ( should not be null due to require component so no need to check )
            agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();

            projector = transform.GetComponentInChildren<Projector>();
            textName = transform.GetComponentInChildren<TextMeshPro>();
            m_Rigidbody = transform.GetComponent<Rigidbody>();

            agent.updateRotation = false;
            agent.updatePosition = true;
            projector.enabled = false;

            if (Entity != null)
            {
                //textName.text = Entity.Name;
                m_Rigidbody.isKinematic = true;
                dialouges = Entity.Greeting.Split('#').ToList();
            }
        }

        private void Update()
        {
            if (!agent.isOnNavMesh)
                return;

            if (target != null)
                agent.SetDestination(target.position);

            if (agent.remainingDistance > agent.stoppingDistance)
                character.Move(agent.desiredVelocity, false, false);
            else
                character.Move(Vector3.zero, false, false);
        }

        void LateUpdate()
        {
            //textName.transform.eulerAngles = Camera.main.transform.rotation.eulerAngles;

            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player");
            }

            if (player != null)
            {
                if (Vector3.Distance(transform.position, player.transform.position) < ActiveDistance)
                {
                    projector.enabled = true;
                    Active = true;
                }
                else
                {
                    projector.enabled = false;
                    Active = false;
                }
            }
        }
        public void SetTarget(Transform target)
        {
            this.target = target;
        }

        /// <summary>
        /// NPC说的话
        /// </summary>
        public void Speak(string text)
        {
            EventDispatcher.ExecuteEvent<GameObject, string>(Core.Events.Entity.Speak, gameObject, text);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (player == null)
                {
                    player = GameObject.FindGameObjectWithTag("Player");
                }

                if (player != null)
                {
                    transform.DOLookAt(player.transform.position, 0.4f, AxisConstraint.Y);
                }

                if (!Active)
                {
                    string text = dialouges[UnityEngine.Random.Range(0, dialouges.Count)];
                    Speak(text);
                }
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            //ProductionItemElement component = eventData.pointerDrag.GetComponent<ProductionItemElement>();
            //if(component != null)
            //{
            //    Goods item = component.Item as Goods;
            //    BeforeEventArgs arg = new BeforeEventArgs(item, DropCallBack);
            //    OnDropEvent.Invoke(gameObject, arg);
            //}

            ProductionItemElement component = eventData.pointerDrag.GetComponent<ProductionItemElement>();
            if (component != null)
            {
                if (AffectedByNPC)
                {
                    if (Active)
                    {
                        Goods item = component.Item as Goods;
                        EventDispatcher.ExecuteEvent<NPCControl, Goods>(Events.Entity.Drop, this, item);
                    }
                    else
                    {
                        Speak("请到我身边来。");
                    }
                }
                else
                {
                    Goods item = component.Item as Goods;

                    EventDispatcher.ExecuteEvent<NPCControl, Goods>(Events.Entity.Drop, this, item);
                }
            }
        }

        private void DropCallBack(BeforeEventArgs arg)
        {

        }
        public void StartRuning(Transform[] points,UnityAction _action, string result)
        {
          CoroutineManager.Instance.StartCoroutine(PatrolRuning(points, _action, result));
        }
        /// <summary>
        /// 巡逻过程
        /// </summary>
        /// <param name="points">路径点列表</param>
        /// <param name="itemType">检测结束后打开文件类型</param>
        /// <returns></returns>
        public IEnumerator PatrolRuning(Transform[] points, UnityAction _action, string result = "当前的操作结束")
        {
            int i = 0;
            int pointCount = points.Length;
            isMoving = true;
            IsJumping = false;
            agent.isStopped = false;
            yield return new WaitForSeconds(1f);
            player.GetComponent<MyselfControl>().disable = true;
            //NPC巡逻的路径
            while (i < pointCount && !IsJumping)
            {
                int j = i % pointCount;
                
                agent.SetDestination(points[j].position);
                yield return new WaitForSeconds(0.2f);
                while (agent.remainingDistance >= agent.stoppingDistance + 0.1f)
                {
                    yield return new WaitForSeconds(0.01f);
                }
                i++;
                
            }
            yield return new WaitForEndOfFrame();
            isMoving = false;
            yield return new WaitForSeconds(1f);
            transform.DOLookAt(player.transform.position, 0.4f);

            //显示结果
            //ShowDialog(result, Color.black, 2);
            Speak(result);
            yield return new WaitForSeconds(2f);
            player.GetComponent<MyselfControl>().disable = false;
            _action.Invoke();
        }
        /// <summary>
        /// 巡逻过程返回
        /// </summary>
        /// <param name="points">路径点列表</param>
        /// <param name="itemType">检测结束后打开文件类型</param>
        /// <returns></returns>
        public IEnumerator PatrolRuningBack(Transform[] points, UnityAction _action)
        {

            Array.Reverse(points);

            int i = 0;
            int pointCount = points.Length;
            isMoving = true;
            yield return new WaitForSeconds(1f);

            //QA去检验的路径
            while (i < pointCount)
            {
                agent.SetDestination(points[i % pointCount].position);
                yield return new WaitForSeconds(0.5f);

                while (agent.remainingDistance >= agent.stoppingDistance + 0.1f)
                {
                    yield return new WaitForSeconds(0.5f);
                }

                i++;
            }

            yield return new WaitForEndOfFrame();
            isMoving = false;
            transform.DOLookAt(player.transform.position, 0.4f);
            _action.Invoke();
            Array.Reverse(points);

        }
    }
}
