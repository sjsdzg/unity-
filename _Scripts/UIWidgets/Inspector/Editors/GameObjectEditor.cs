using UnityEngine;
using System.Collections;

namespace XFramework.UIWidgets
{
    public class GameObjectEditor : EditorBase<GameObject>
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
            expandControl.Bind(() => "基本属性", null);

            FieldBase fieldBase = InspectorManager.Instance.CreateField<bool>();
            RectTransformUtils.SetParentAndAlign(fieldBase.gameObject, expandControl.Content.gameObject);
            fieldBase.Bind("active", () => Target.activeSelf, x => Target.SetActive((bool)x));

            FieldBase floatField = InspectorManager.Instance.CreateField<float>();
            RectTransformUtils.SetParentAndAlign(floatField.gameObject, expandControl.Content.gameObject);
            floatField.Bind("float", () => 1.0f, null);
        }

    }
}

