using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.EventSystems;
using XFramework.Core;

namespace XFramework.Architectural
{
    public class Group3D : ProceduralGraphic
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

        /// <summary>
        /// 成员索引列表
        /// </summary>
        private readonly Dictionary<string, GraphicObject> m_GraphicSet = new Dictionary<string, GraphicObject>();

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
            m_GraphicSet.Add(id, graphic);
            // 重置选中状态
            SelectableGraphic selectable = graphic as SelectableGraphic;
            if (selectable != null)
            {
                selectable.DoStateTransition(currentSelectionState);
            }
        }

        public bool RemoveGraphic(string id)
        {
            if (m_GraphicSet.TryGetValue(id, out GraphicObject graphic))
            {
                graphic.Owner = null;
                m_GraphicSet.Remove(id);
                // 重置选中状态
                SelectableGraphic selectable = graphic as SelectableGraphic;
                if (selectable != null)
                {
                    selectable.DoStateTransition(SelectionState.Normal);
                }

                return true;
            }

            return false;
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

            foreach (var graphic in m_GraphicSet.Values)
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
    }
}
