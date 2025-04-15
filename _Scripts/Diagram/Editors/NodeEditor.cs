using UnityEngine;
using System.Collections;
using XFramework.UIWidgets;

namespace XFramework.Diagram
{
    public class NodeEditor : EditorBase<Node>
    {
        private RectTransform rectTransform;

        protected override void Awake()
        {
            base.Awake();
            rectTransform = GetComponent<RectTransform>();
        }

        public override void BuildEditor()
        {
            base.BuildEditor();

            ExpandControl expandControl = InspectorManager.Instance.CreateControl<ExpandControl>();
            RectTransformUtils.SetParentAndAlign(expandControl.gameObject, rectTransform.gameObject);
            expandControl.Bind(() => "参数设置", null);
            // add
            Components.Add(expandControl);

            if (Target.Variables != null)
            {
                foreach (var variable in Target.Variables)
                {
                    switch (variable.Type)
                    {
                        case Module.VariableType.Float:
                            FieldBase floatField = InspectorManager.Instance.CreateField<float>();
                            RectTransformUtils.SetParentAndAlign(floatField.gameObject, expandControl.Content.gameObject);
                            floatField.Bind(variable.Name, () => variable.Value, x => variable.Value = (float)x);
                            // add
                            Components.Add(floatField);
                            break;
                        case Module.VariableType.Integer:
                            FieldBase intField = InspectorManager.Instance.CreateField<int>();
                            RectTransformUtils.SetParentAndAlign(intField.gameObject, expandControl.Content.gameObject);
                            intField.Bind(variable.Name, () => variable.Value, x => variable.Value = (int)x);
                            // add
                            Components.Add(intField);
                            break;
                        case Module.VariableType.Boolean:
                            FieldBase boolField = InspectorManager.Instance.CreateField<bool>();
                            RectTransformUtils.SetParentAndAlign(boolField.gameObject, expandControl.Content.gameObject);
                            boolField.Bind(variable.Name, () => variable.Value, x => variable.Value = (bool)x);
                            // add
                            Components.Add(boolField);
                            break;
                        case Module.VariableType.String:
                            break;
                        default:
                            break;
                    }
                }
            }

        }
    }

}