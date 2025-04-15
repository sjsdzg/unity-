using UnityEngine;
using System.Collections;
using ChartAndGraph;
public class PieChartFeed : MonoBehaviour
{
    private Material material;

	void Start ()
    {
        PieChart pie = GetComponent<PieChart>();
        if (pie != null)
        {
            pie.DataSource.Clear();

            pie.DataSource.AddCategory("0-99(人数90)", material);
            pie.DataSource.SetValue("0-99(人数90)", 50);
            pie.DataSource.AddCategory("Player 2", material);
            pie.DataSource.SetValue("Player 2", 50);
            //pie.DataSource.SlideValue("Player 1", 50, 10f);
            //pie.DataSource.AddCategory("Player 4", material);
        }
	}
}
