using System.Collections.Generic;
using UnityEngine;


public class PLC_CurveParameter : MonoBehaviour
{

    private WMG_Axis_Graph m_Graph;
    /// <summary>
    /// Y轴最小值
    /// </summary>
    public int AxisMinValue = 0;
    /// <summary>
    /// Y轴最大值
    /// </summary>
    public int AxisMaxValue = 0;
    /// <summary>
    /// Y轴分几份（平均分）
    /// </summary>
    public int AxisNumTicks;
    /// <summary>
    /// 每次增加的数
    /// </summary>
    private int AddNum = 0;
    private int NowNum;
    List<string> m_List = new List<string>();
    private void Awake()
    {
        m_Graph = transform.GetComponent<WMG_Axis_Graph>();
        m_Graph.yAxis.AxisMinValue = AxisMinValue;
        m_Graph.yAxis.AxisMaxValue = AxisMaxValue;
        m_Graph.yAxis.AxisNumTicks = AxisNumTicks+1;
        m_Graph.yAxis.AxisLabelSize = AxisNumTicks+1;
        
        NowNum = AxisMinValue;
        CurveParameter();
    }
    /// <summary>
    /// 改变曲线参数
    /// </summary>
    public void CurveParameter()
    {
        
        
        if ((AxisMaxValue-AxisMinValue) % AxisNumTicks == 0)
        {
            AddNum = (AxisMaxValue - AxisMinValue) / AxisNumTicks;
            m_List.Add(NowNum.ToString());
            while(NowNum < AxisMaxValue)
            {
                NowNum += AddNum;
                m_List.Add(NowNum.ToString());
               // Debug.Log("list表中的值：" + m_List);
            }
           // Debug.Log("list表中的值??????："+m_List.Count+"shangbianshi shuliang " + m_List[0]+ m_List[1]+ m_List[2]+ m_List[3] + m_List[4] + m_List[5]);
           
           // Debug.Log("每次增加多少：" + AddNum);
            
            
        }
        else
        {
            Debug.LogError("对不起，参数输入不正确！");
        }
        //for (int i = 0; i < m_List.Count+1; i++)
        //{
        //    m_Graph.yAxis.axisLabels.list[i] = m_List[i];
        //}
        
        //for (int i = 0; i < m_List.Count; i++)
        //{
        //    m_Graph.yAxis.axisLabels[i] = m_List[i];
        //    Debug.Log("meigelistzhong de yangzi shi :" + m_Graph.yAxis.axisLabels[i]);
        //}
        //Debug.Log("???????????????:" + m_Graph.yAxis.axisLabels[0]+"?????"+ m_Graph.yAxis.axisLabels[1]+"?????"+ m_Graph.yAxis.axisLabels.Count);
    }
}
