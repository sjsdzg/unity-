using UnityEngine;
using System.Collections;
using XFramework.Common;
using System;
using XFramework.Component;

public class UpdateFluidAction : ActionBase
{
    /// <summary>
    /// gameObject
    /// </summary>
    public GameObject gameObject;

    /// <summary>
    /// 速度
    /// </summary>
    public int velocity = 1;

    public UpdateFluidAction(GameObject _gameObject, int _velocity)
    {
        gameObject = _gameObject;
        velocity = _velocity;
    }

    public override void Execute()
    {
        FluidComponent m_Fluid = gameObject.GetComponent<FluidComponent>();
        m_Fluid.Velocity = velocity;
        Completed();
    }

}
