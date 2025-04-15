using UnityEngine;
using System.Collections;
using XFramework.Module;

namespace XFramework.UIWidgets
{
    public class InspectorTest : MonoBehaviour
    {
        public InspectorView view;

        private void Awake()
        {
            InspectorManager.Instance.RegisterEditor<GameObjectEditor>();
        }

        private void Start()
        {
            view.Inspect(gameObject, gameObject.name);

            VariableCollection variables = new VariableCollection()
            {
                Variables = new System.Collections.Generic.List<Variable>()
                {
                    new Variable()
                    {
                        Name = "Boolean",
                        Type = VariableType.Boolean,
                        Value = new Vector2(2f,2f),
                    },
                    new ConstantVariable()
                    {
                        Name = "Float",
                        Type = VariableType.Float,
                        Value = 1.0f,
                        DefaultValue = 1.0f,
                    },
                    new RangeVariable()
                    {
                        Name = "Integer",
                        Type = VariableType.Integer,
                        Value = 1,
                        MinValue = 0,
                        MaxValue = 2,
                    },
                    new ConstantVariable()
                    {
                        Name = "String",
                        Type = VariableType.String,
                        Value = "hello world",
                        DefaultValue = "hello world!",
                    }
                }
            };

            Debug.Log(variables.ToJson());
        }
    }
}

