using Battlehub.RTHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using XFramework.Common;
using XFramework.Core;
using XFramework.UI;

namespace XFramework.Architectural
{
    public class Group2D : ProceduralGraphic
    {
        private Group group;
        /// <summary>
        /// 组
        /// </summary>
        public Group Group
        {
            get { return group; }
            set
            {
                group = value;
                group.TransformChanged += Group_TransformChanged;
                group.VerticesChanged += Group_VerticesChanged;
                group.EntityAdded += Group_EntityAdded;
                group.EntityRemoved += Group_EntityRemoved;
            }
        }

        RectTooluse rectTooluse;
        /// <summary>
        /// 成员索引列表
        /// </summary>
        private readonly Dictionary<string, GraphicObject> graphicIndexSet = new Dictionary<string, GraphicObject>();

        private void Group_TransformChanged()
        {
            SetTranformDirty();
        }

        private void Group_VerticesChanged()
        {
            SetVerticesDirty();
        }

        private void Group_EntityAdded(Group sender, GroupChangedArgs e)
        {
            EntityObject entity = e.Item;
            GraphicManager.Instance.TryGetGraphic2D(entity, out GraphicObject graphic);
            AddGraphic(entity.Id, graphic);
        }

        private void Group_EntityRemoved(Group sender, GroupChangedArgs e)
        {
            EntityObject entity = e.Item;
            RemoveGraphic(entity.Id);
        }


        public void AddGraphic(string id, GraphicObject graphic)
        {
            graphic.Owner = this;
            graphicIndexSet.Add(id, graphic);
            // 重置选中状态
            SelectableGraphic selectable = graphic as SelectableGraphic;
            if (selectable != null)
            {
                if (!(selectable is Wall2D))
                {
                    selectable.DoStateTransition(currentSelectionState);
                }
            }
        }

        public bool RemoveGraphic(string id)
        {
            if (graphicIndexSet.TryGetValue(id, out GraphicObject graphic))
            {
                graphic.Owner = null;
                graphicIndexSet.Remove(id);
                // 重置选中状态
                SelectableGraphic selectable = graphic as SelectableGraphic;
                if (selectable != null)
                {
                    if (!(selectable is Wall2D))
                    {
                        selectable.DoStateTransition(SelectionState.Normal);
                    }
                }

                return true;
            }

            return false;
        }

        public List<MData> mDatas = new List<MData>();
        public List<Vector3> m_Vector3s = new List<Vector3>();
        bool isFirst = false;
        GroupLibraryItem[] groupLibraryItems;
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                Group tmp = group;
                List<ContextMenuParameter> parameters = new List<ContextMenuParameter>
                {
                    new ContextMenuParameter("删除", x=>
                    {
                        if (Selection.Instance.currentSelectedEntityObject.Equals(group))
                        {
                            Selection.Instance.currentSelectedEntityObject = null;
                        }
                        ArchitectUtility.RemoveGroupHandler(Architect.Instance.CurrentFloor, group);
                        ArchitectUtility.RemoveGroup(Architect.Instance.CurrentFloor, group);
                        ArchitectUtility.CombineWall(Architect.Instance.CurrentFloor);

                        Core.EventDispatcher.ExecuteEvent<Group>(Architect.RemoveGroupEvent, group);
                    }),
                    new ContextMenuParameter("编辑位置",x=>{
                        //先删除
                        if (Selection.Instance.currentSelectedEntityObject.Equals(tmp))
                        {
                            Selection.Instance.currentSelectedEntityObject = null;
                        }
                        ArchitectUtility.RemoveGroupHandler(Architect.Instance.CurrentFloor, tmp);
                        ArchitectUtility.RemoveGroup(Architect.Instance.CurrentFloor, tmp);
                        ArchitectUtility.CombineWall(Architect.Instance.CurrentFloor);

                        Core.EventDispatcher.ExecuteEvent<Group>(Architect.RemoveGroupEvent, tmp);

                        //重新生成
                        if (groupLibraryItems == null)
                        {
                            var p =GameObject.FindObjectOfType<ArchitectUI>().transform.Find("Background/EditorBar/GroupLibraryPanel/Viewport/Content");
                            groupLibraryItems = p.GetComponentsInChildren<GroupLibraryItem>();
                        }
                        ArchitectUI architect = GameObject.FindObjectOfType<ArchitectUI>();

                        var _data = groupLibraryItems.ToList().Find(item =>item.Data.GroupInfo.Name.Equals(tmp.Name));

                        Architect.Instance.ActiveTool = Architect.Instance.GetTool<GroupCreateTool>();
                        GroupCreateToolArgs t = new GroupCreateToolArgs();
                        var groupId = _data.Data.GroupInfo.Id;
                        var group = architect.GroupsDoc.CurrentFloor.Groups.Find(x1 => x1.Id.Equals(groupId));
                        t.Group = (Group)group.Clone();
                        Architect.Instance.ActiveTool.Init(t);
                        Debug.Log("编辑位置");
                    }),
                    new ContextMenuParameter("编辑尺寸", x =>
                    {


                        mDatas.Clear();
                        if (rectTooluse == null)
                        {
                            rectTooluse = GameObject.FindObjectOfType<RectTooluse>();
                        }
                        rectTooluse.finish = null;
                        rectTooluse.finish += CompleteDrag;
                        if (rectTooluse.IsUsingTool)
                        {
                            rectTooluse.StopUsingTool();
                        }
                        if (isFirst)
                        {

                        }
                        else
                        {
                            //遍历group.Members 计算包围盒
                            List<Vector3> vector3s = new List<Vector3>();
                            foreach (var item in group.Members)
                            {
                                if (item.Entity.Type == EntityType.Corner)
                                {
                                    vector3s.Add(item.Position);
                                    m_Vector3s.Add(item.Position);
                                }
                            }
                            CalculateBoundingBox(vector3s);
                        }


                        List<Room> rooms = new List<Room>();
                        if( group.TryGetRooms(out rooms))
                        {
                            var pos = rooms[0].GetVisualPoint();
                            rectTooluse.SetPositionAndScale(bounds[0], bounds[1], bounds[2], bounds[3],pos);
                            rectTooluse.StartUsingTool();
                                foreach (var item in group.Members)
                                {
                                    if (item.Entity.Type == EntityType.Corner)
                                    {
                                        MData _mData = new MData();
                                        _mData.member = item;
                                        for (int i = 0; i < rectTooluse.startDragPoint.Count; i++)
                                        {
                                            if (Approximately(((Corner)item.Entity).Position,rectTooluse.startDragPoint[i]))
                                            {
                                                _mData.rectToolIndex = i;
                                                _mData.IsCorner = true;
                                                break;
                                            }
                                            else
                                            {
                                                _mData.IsCorner = false;
                                                _mData.rectToolIndex = -1;
                                            }
                                        }
                                        mDatas.Add(_mData);
                                    }
                                }
                        }

                    }),
                    new ContextMenuParameter("关闭", x => { ContextMenuEx.Instance.Hide(); }),
                };
                ContextMenuEx.Instance.Show(gameObject, parameters);
            }
        }
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            GUIStyle gUIStyle = new GUIStyle();
            gUIStyle.fontSize = 20;
            gUIStyle.fontStyle = FontStyle.Bold;
            gUIStyle.normal.textColor = Color.green;

            if (group != null)
            {
                for (int i = 0; i < group.Members.Count; i++)
                {
                    if (group.Members[i].Entity.Type == EntityType.Corner)
                    {
                        UnityEditor.Handles.Label(((Corner)group.Members[i].Entity).Position, i.ToString(), gUIStyle);
                    }
                }
            }
        }
#endif
        private void CompleteDrag(RectTooluse _tools)
        {

            //计算mDatas中的顶点
            for (int i = 0; i < mDatas.Count; i++)
            {
                if (mDatas[i].IsCorner)
                {
                    //墙角的顶点
                    ((Corner)mDatas[i].member.Entity).Position = _tools.EndVectors2[mDatas[i].rectToolIndex];
                }
                else
                {
                    //不是墙角的顶点
                    Vector3 pos = CalculatedPosition(
                       ((Corner)mDatas[i].member.Entity).Position,
                        new Vector3[] { _tools.startDragPoint[0], _tools.startDragPoint[1], _tools.startDragPoint[2], _tools.startDragPoint[3] },
                        new Vector3[] { _tools.EndVectors2[0], _tools.EndVectors2[1], _tools.EndVectors2[2], _tools.EndVectors2[3] }
                        );
                    ((Corner)mDatas[i].member.Entity).Position = pos;
                }
            }
            isFirst = true;
            bounds[0] = _tools.EndVectors2[0];
            bounds[1] = _tools.EndVectors2[1];
            bounds[2] = _tools.EndVectors2[2];
            bounds[3] = _tools.EndVectors2[3];
        }

        /// <summary>
        /// 不是墙角的顶点，计算新的位置
        /// </summary>
        Vector3 CalculatedPosition(Vector3 oldPos, Vector3[] startPoints, Vector3[] endPoints)
        {
            Vector3 _newPosition = Vector3.zero;
            // 是不是在0-1边上
            if (IsPointOnLine(startPoints[0], startPoints[1], oldPos))
            {
                float s = CalculateScale(startPoints[0], startPoints[1], oldPos);
                _newPosition = CalculateScalePoint(endPoints[0], endPoints[1], s);
                _newPosition.z = endPoints[0].z;
            }
            // 是不是在1-2边上
            if (IsPointOnLine(startPoints[1], startPoints[2], oldPos))
            {
                float s = CalculateScale(startPoints[1], startPoints[2], oldPos);
                _newPosition = CalculateScalePoint(endPoints[1], endPoints[2], s);
                _newPosition.x = endPoints[1].x;
            }
            // 是不是在2-3边上
            if (IsPointOnLine(startPoints[2], startPoints[3], oldPos))
            {
                float s = CalculateScale(startPoints[2], startPoints[3], oldPos);
                _newPosition = CalculateScalePoint(endPoints[2], endPoints[3], s);
                _newPosition.z = endPoints[2].z;
            }
            // 是不是在3-0边上
            if (IsPointOnLine(startPoints[3], startPoints[0], oldPos))
            {
                float s = CalculateScale(startPoints[3], startPoints[0], oldPos);
                _newPosition = CalculateScalePoint(endPoints[3], endPoints[0], s);
                _newPosition.x = endPoints[3].x;
            }
            return _newPosition;
        }

        /// <summary>
        /// 点C是不是在AB直线上
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        bool IsPointOnLine(Vector3 a, Vector3 b, Vector3 c, float error = 0.02f)
        {
            Vector3 ab = b - a;
            Vector3 ac = c - a;

            // 计算投影向量
            float projection = Vector3.Dot(ac, ab) / ab.sqrMagnitude;
            Vector3 projectedPoint = a + ab * projection;

            // 计算点C到投影点的距离
            float distance = Vector3.Distance(c, projectedPoint);

            return distance <= error;
        }

        /// <summary>
        /// 计算point在startPoint和endPoint之间的比例
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        float CalculateScale(Vector3 startPoint, Vector3 endPoint, Vector3 point)
        {
            // AB向量
            Vector3 AB = endPoint - startPoint;

            // AC向量
            Vector3 AC = point - startPoint;

            // 判断AB向量是否为零向量，防止除以零
            if (AB.sqrMagnitude == 0f)
            {
                Debug.LogError("AB向量长度为0，无法计算比例");
                return 0f;
            }

            // 计算点C在AB上的投影长度与AB长度的比值
            float scale = Vector3.Dot(AC, AB) / AB.sqrMagnitude;

            // 限制比例在0到1之间，防止超出线段范围
            return Mathf.Clamp(scale, 0f, 1f);
        }

        /// <summary>
        /// 计算给定点在变化后的新的位置
        /// </summary>
        Vector3 CalculateScalePoint(Vector3 _a, Vector3 _b, float s)
        {
            // AB向量
            Vector3 AB = _b - _a;

            // 计算点C的位置
            Vector3 AC = AB * s;
            Vector3 C = _a + AC;

            return C;
        }

        public Vector3[] bounds;
        public void CalculateBoundingBox(List<Vector3> points)
        {
            float minX = float.MaxValue;
            float maxX = float.MinValue;
            float minZ = float.MaxValue;
            float maxZ = float.MinValue;

            // 遍历所有点，更新最小和最大值
            foreach (Vector3 point in points)
            {
                minX = Mathf.Min(minX, point.x);
                maxX = Mathf.Max(maxX, point.x);
                minZ = Mathf.Min(minZ, point.z);
                maxZ = Mathf.Max(maxZ, point.z);
            }

            // 将最小和最大值赋值给bounds数组
            bounds = new Vector3[4];
            bounds[0] = new Vector3(minX, 0, minZ); // 左下角
            bounds[1] = new Vector3(maxX, 0, minZ); // 右下角
            bounds[2] = new Vector3(maxX, 0, maxZ); // 右上角
            bounds[3] = new Vector3(minX, 0, maxZ); // 左上角

        }

        public override void Press()
        {
            base.Press();
            Selection.Instance.SetSelectedEntityObject(group);
        }

        public override void DoStateTransition(SelectionState state)
        {
            base.DoStateTransition(state);
            switch (state)
            {
                case SelectionState.Normal:
                    break;
                case SelectionState.Highlighted:
                    break;
                case SelectionState.Selected:
                    break;
                default:
                    break;
            }

            foreach (var graphic in graphicIndexSet.Values)
            {
                SelectableGraphic selectable = graphic as SelectableGraphic;
                if (selectable is Wall2D)
                    continue;

                if (selectable != null)
                {
                    selectable.DoStateTransition(state);
                }
            }
        }

        /// <summary>
        /// 计算两个Vector3的差向量。
        /// 计算差向量的模长（即长度）。
        /// 将模长与设定的误差范围（0.02m）进行比较。
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public static bool Approximately(Vector3 a, Vector3 b, float tolerance = 0.02f)
        {
            return Vector3.SqrMagnitude(a - b) < tolerance * tolerance;
        }
    }

    [Serializable]
    public class MData
    {
        public Group.Member member;
        //是不是墙角的顶点
        public bool IsCorner;
        public int rectToolIndex;

        //不是墙角的顶点，记录位置
        public Vector3 position;
    }
}
