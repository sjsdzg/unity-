using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XFramework.Math;

namespace XFramework.Architectural
{
    public static class DxfUtility
    {
        /// <summary>
        /// 根据图层，从 dxf 中获取线段列表
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="layers"></param>
        /// <returns></returns>
        public static List<Segment2> FindSegments(netDxf.DxfDocument doc, List<string> layers)
        {
            List<Segment2> segments = new List<Segment2>();
            if (layers == null || layers.Count == 0)
            {
                return segments;
            }

            foreach (var line in doc.Lines)
            {
                if (layers.Contains(line.Layer.Name))
                {
                    Vector2 p1 = new Vector2((float)(line.StartPoint.X / 1000), (float)(line.StartPoint.Y / 1000));
                    Vector2 p2 = new Vector2((float)(line.EndPoint.X / 1000), (float)(line.EndPoint.Y / 1000));
                    Segment2 segment = new Segment2(p1, p2);
                    segments.Add(segment);
                }
            }

            foreach (var polyline in doc.Polylines)
            {
                if (layers.Contains(polyline.Layer.Name))
                {
                    int count = polyline.Vertexes.Count;
                    for (int i = 0; i < count - 1; i++)
                    {
                        var StartPoint = polyline.Vertexes[i].Position;
                        var EndPoint = polyline.Vertexes[i + 1].Position;
                        Vector2 p1 = new Vector2((float)(StartPoint.X / 1000), (float)(StartPoint.Y / 1000));
                        Vector2 p2 = new Vector2((float)(EndPoint.X / 1000), (float)(EndPoint.Y / 1000));
                        Segment2 segment = new Segment2(p1, p2);
                        segments.Add(segment);
                    }

                    if (polyline.IsClosed)
                    {
                        var StartPoint = polyline.Vertexes[count - 1].Position;
                        var EndPoint = polyline.Vertexes[0].Position;
                        Vector2 p1 = new Vector2((float)(StartPoint.X / 1000), (float)(StartPoint.Y / 1000));
                        Vector2 p2 = new Vector2((float)(EndPoint.X / 1000), (float)(EndPoint.Y / 1000));
                        Segment2 segment = new Segment2(p1, p2);
                        segments.Add(segment);
                    }
                }
            }

            foreach (var lwPolyline in doc.LwPolylines)
            {
                if (layers.Contains(lwPolyline.Layer.Name))
                {
                    int count = lwPolyline.Vertexes.Count;
                    for (int i = 0; i < count - 1; i++)
                    {
                        var StartPoint = lwPolyline.Vertexes[i].Position;
                        var EndPoint = lwPolyline.Vertexes[i + 1].Position;
                        Vector2 p1 = new Vector2((float)(StartPoint.X / 1000), (float)(StartPoint.Y / 1000));
                        Vector2 p2 = new Vector2((float)(EndPoint.X / 1000), (float)(EndPoint.Y / 1000));
                        Segment2 segment = new Segment2(p1, p2);
                        segments.Add(segment);
                    }

                    if (lwPolyline.IsClosed)
                    {
                        var StartPoint = lwPolyline.Vertexes[count - 1].Position;
                        var EndPoint = lwPolyline.Vertexes[0].Position;
                        Vector2 p1 = new Vector2((float)(StartPoint.X / 1000), (float)(StartPoint.Y / 1000));
                        Vector2 p2 = new Vector2((float)(EndPoint.X / 1000), (float)(EndPoint.Y / 1000));
                        Segment2 segment = new Segment2(p1, p2);
                        segments.Add(segment);
                    }
                }
            }

            return segments;
        }

        /// <summary>
        /// 根据图层，提取窗子的线段列表
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="layers"></param>
        /// <returns></returns>
        public static List<Segment2> FindWindowSegments(netDxf.DxfDocument doc, List<string> layers)
        {
            List<Segment2> segments = new List<Segment2>();
            if (layers == null || layers.Count == 0)
            {
                return segments;
            }

            // 从实体列表中提取线段
            segments.AddRange(FindSegments(doc, layers));
            // 从块里提取线段
            foreach (netDxf.Blocks.Block block in doc.Blocks)
            {
                if (layers.Contains(block.Layer.Name))
                {
                    foreach (var entity in block.Entities)
                    {
                        if (entity.Type == netDxf.Entities.EntityType.Line)
                        {
                            netDxf.Entities.Line line = (netDxf.Entities.Line)entity;
                            Vector2 p1 = new Vector2((float)line.StartPoint.X / 1000, (float)line.StartPoint.Y / 1000);
                            Vector2 p2 = new Vector2((float)line.EndPoint.X / 1000, (float)line.EndPoint.Y / 1000);
                            Segment2 segment = new Segment2(p1, p2);
                        }
                        else if (entity.Type == netDxf.Entities.EntityType.Polyline)
                        {
                            netDxf.Entities.Polyline polyline = (netDxf.Entities.Polyline)entity;
                            int count = polyline.Vertexes.Count;
                            for (int i = 0; i < count - 1; i++)
                            {
                                var StartPoint = polyline.Vertexes[i].Position;
                                var EndPoint = polyline.Vertexes[i + 1].Position;
                                Vector2 p1 = new Vector2((float)(StartPoint.X / 1000), (float)(StartPoint.Y / 1000));
                                Vector2 p2 = new Vector2((float)(EndPoint.X / 1000), (float)(EndPoint.Y / 1000));
                                Segment2 segment = new Segment2(p1, p2);
                                segments.Add(segment);
                            }

                            if (polyline.IsClosed)
                            {
                                var StartPoint = polyline.Vertexes[count - 1].Position;
                                var EndPoint = polyline.Vertexes[0].Position;
                                Vector2 p1 = new Vector2((float)(StartPoint.X / 1000), (float)(StartPoint.Y / 1000));
                                Vector2 p2 = new Vector2((float)(EndPoint.X / 1000), (float)(EndPoint.Y / 1000));
                                Segment2 segment = new Segment2(p1, p2);
                                segments.Add(segment);
                            }
                        }
                        else if (entity.Type == netDxf.Entities.EntityType.LwPolyline)
                        {
                            netDxf.Entities.LwPolyline lwPolyline = (netDxf.Entities.LwPolyline)entity;
                            int count = lwPolyline.Vertexes.Count;
                            for (int i = 0; i < count - 1; i++)
                            {
                                var StartPoint = lwPolyline.Vertexes[i].Position;
                                var EndPoint = lwPolyline.Vertexes[i + 1].Position;
                                Vector2 p1 = new Vector2((float)(StartPoint.X / 1000), (float)(StartPoint.Y / 1000));
                                Vector2 p2 = new Vector2((float)(EndPoint.X / 1000), (float)(EndPoint.Y / 1000));
                                Segment2 segment = new Segment2(p1, p2);
                                segments.Add(segment);
                            }

                            if (lwPolyline.IsClosed)
                            {
                                var StartPoint = lwPolyline.Vertexes[count - 1].Position;
                                var EndPoint = lwPolyline.Vertexes[0].Position;
                                Vector2 p1 = new Vector2((float)(StartPoint.X / 1000), (float)(StartPoint.Y / 1000));
                                Vector2 p2 = new Vector2((float)(EndPoint.X / 1000), (float)(EndPoint.Y / 1000));
                                Segment2 segment = new Segment2(p1, p2);
                                segments.Add(segment);
                            }
                        }
                    }
                }
            }
            // 从块参考里提取线段
            Matrix4x4 matrix = Matrix4x4.identity;
            foreach (netDxf.Entities.Insert insert in doc.Inserts)
            {
                if (layers.Contains(insert.Layer.Name))
                {
                    List<netDxf.Entities.EntityObject> entities = insert.Explode();
                    Vector3 position = new Vector3((float)insert.Position.X / 1000, (float)insert.Position.Y / 1000, (float)insert.Position.Z / 1000);
                    Quaternion rotation = Quaternion.AngleAxis((float)insert.Rotation, new Vector3((float)insert.Normal.X / 1000, (float)insert.Normal.Y / 1000, (float)insert.Normal.Z / 1000));
                    Vector3 scale = new Vector3((float)insert.Scale.X, (float)insert.Scale.Y, (float)insert.Scale.Z);
                    matrix.SetTRS(position, rotation, scale);

                    foreach (var entity in insert.Block.Entities)
                    {
                        if (entity.Type == netDxf.Entities.EntityType.Line)
                        {
                            netDxf.Entities.Line line = (netDxf.Entities.Line)entity;
                            Vector2 p1 = matrix.MultiplyPoint(new Vector2((float)line.StartPoint.X / 1000, (float)line.StartPoint.Y / 1000));
                            Vector2 p2 = matrix.MultiplyPoint(new Vector2((float)line.EndPoint.X / 1000, (float)line.EndPoint.Y / 1000));
                            Segment2 segment = new Segment2(p1, p2);
                        }
                        else if (entity.Type == netDxf.Entities.EntityType.Polyline)
                        {
                            netDxf.Entities.Polyline polyline = (netDxf.Entities.Polyline)entity;
                            int count = polyline.Vertexes.Count;
                            for (int i = 0; i < count - 1; i++)
                            {
                                var StartPoint = polyline.Vertexes[i].Position;
                                var EndPoint = polyline.Vertexes[i + 1].Position;
                                Vector2 p1 = matrix.MultiplyPoint(new Vector2((float)(StartPoint.X / 1000), (float)(StartPoint.Y / 1000)));
                                Vector2 p2 = matrix.MultiplyPoint(new Vector2((float)(EndPoint.X / 1000), (float)(EndPoint.Y / 1000)));
                                Segment2 segment = new Segment2(p1, p2);
                                segments.Add(segment);
                            }

                            if (polyline.IsClosed)
                            {
                                var StartPoint = polyline.Vertexes[count - 1].Position;
                                var EndPoint = polyline.Vertexes[0].Position;
                                Vector2 p1 = matrix.MultiplyPoint(new Vector2((float)(StartPoint.X / 1000), (float)(StartPoint.Y / 1000)));
                                Vector2 p2 = matrix.MultiplyPoint(new Vector2((float)(EndPoint.X / 1000), (float)(EndPoint.Y / 1000)));
                                Segment2 segment = new Segment2(p1, p2);
                                segments.Add(segment);
                            }
                        }
                        else if (entity.Type == netDxf.Entities.EntityType.LwPolyline)
                        {
                            netDxf.Entities.LwPolyline lwPolyline = (netDxf.Entities.LwPolyline)entity;
                            int count = lwPolyline.Vertexes.Count;
                            for (int i = 0; i < count - 1; i++)
                            {
                                var StartPoint = lwPolyline.Vertexes[i].Position;
                                var EndPoint = lwPolyline.Vertexes[i + 1].Position;
                                Vector2 p1 = matrix.MultiplyPoint(new Vector2((float)(StartPoint.X / 1000), (float)(StartPoint.Y / 1000)));
                                Vector2 p2 = matrix.MultiplyPoint(new Vector2((float)(EndPoint.X / 1000), (float)(EndPoint.Y / 1000)));
                                Segment2 segment = new Segment2(p1, p2);
                                segments.Add(segment);
                            }

                            if (lwPolyline.IsClosed)
                            {
                                var StartPoint = lwPolyline.Vertexes[count - 1].Position;
                                var EndPoint = lwPolyline.Vertexes[0].Position;
                                Vector2 p1 = matrix.MultiplyPoint(new Vector2((float)(StartPoint.X / 1000), (float)(StartPoint.Y / 1000)));
                                Vector2 p2 = matrix.MultiplyPoint(new Vector2((float)(EndPoint.X / 1000), (float)(EndPoint.Y / 1000)));
                                Segment2 segment = new Segment2(p1, p2);
                                segments.Add(segment);
                            }
                        }
                    }
                }
            }

            return segments;
        }

        /// <summary>
        ///  根据图层，从 dxf 中获取 arc 列表
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="layers"></param>
        /// <returns></returns>
        public static List<Arc2> FindArcs(netDxf.DxfDocument doc, List<string> layers)
        {
            List<Arc2> arcs = new List<Arc2>();
            if (layers == null || layers.Count == 0)
            {
                return arcs;
            }

            foreach (netDxf.Entities.Arc dxfArc in doc.Arcs)
            {
                if (layers.Contains(dxfArc.Layer.Name))
                {
                    Arc2 arc = new Arc2();
                    arc.center = new Vector2((float)dxfArc.Center.X / 1000, (float)dxfArc.Center.Y / 1000);
                    arc.radius = (float)dxfArc.Radius / 1000;
                    arc.startAngle = (float)dxfArc.StartAngle * Mathf.Deg2Rad;
                    arc.sweepAngle = (float)(dxfArc.EndAngle - dxfArc.StartAngle) * Mathf.Deg2Rad;
                    arcs.Add(arc);
                }
            }

            foreach (netDxf.Blocks.Block block in doc.Blocks)
            {
                if (layers.Contains(block.Layer.Name))
                {
                    foreach (var entity in block.Entities)
                    {
                        if (entity.Type == netDxf.Entities.EntityType.Arc)
                        {
                            netDxf.Entities.Arc dxfArc = (netDxf.Entities.Arc)entity;
                            Arc2 arc = new Arc2();
                            arc.center = new Vector2((float)dxfArc.Center.X / 1000, (float)dxfArc.Center.Y / 1000);
                            arc.radius = (float)dxfArc.Radius / 1000;
                            arc.startAngle = (float)dxfArc.StartAngle * Mathf.Deg2Rad;
                            arc.sweepAngle = (float)(dxfArc.EndAngle - dxfArc.StartAngle) * Mathf.Deg2Rad;
                            arcs.Add(arc);
                        }
                    }
                }
            }

            Matrix4x4 matrix = Matrix4x4.identity;
            foreach (netDxf.Entities.Insert insert in doc.Inserts)
            {
                if (layers.Contains(insert.Layer.Name))
                {
                    List<netDxf.Entities.EntityObject> entities = insert.Explode();
                    Vector3 position = new Vector3((float)insert.Position.X / 1000, (float)insert.Position.Y / 1000, (float)insert.Position.Z / 1000);
                    Quaternion rotation = Quaternion.AngleAxis((float)insert.Rotation, new Vector3((float)insert.Normal.X / 1000, (float)insert.Normal.Y / 1000, (float)insert.Normal.Z / 1000));
                    Vector3 scale = new Vector3((float)insert.Scale.X, (float)insert.Scale.Y, (float)insert.Scale.Z);
                    matrix.SetTRS(position, rotation, scale);

                    foreach (var entity in insert.Block.Entities)
                    {
                        if (entity.Type == netDxf.Entities.EntityType.Arc)
                        {
                            netDxf.Entities.Arc dxfArc = (netDxf.Entities.Arc)entity;
                            Arc2 arc = new Arc2();
                            arc.center = matrix.MultiplyPoint(new Vector2((float)dxfArc.Center.X / 1000, (float)dxfArc.Center.Y / 1000));
                            arc.radius = (float)dxfArc.Radius * Mathf.Abs(scale.x) / 1000;

                            float startAngle = (float)dxfArc.StartAngle * Mathf.Deg2Rad;
                            float endAngle = (float)dxfArc.EndAngle * Mathf.Deg2Rad;

                            Vector2 startVector = new Vector2(Mathf.Cos(startAngle), Mathf.Sin(startAngle));
                            Vector2 endVector = new Vector2(Mathf.Cos(endAngle), Mathf.Sin(endAngle));

                            startVector = matrix.MultiplyVector(startVector);
                            endVector = matrix.MultiplyVector(endVector);
                            startAngle = Mathf.Atan2(startVector.y, startVector.x);
                            endAngle = Mathf.Atan2(endVector.y, endVector.x);

                            float cross = MathUtility.Cross(startVector, endVector);
                            if (cross < 0)
                            {
                                float temp = startAngle;
                                startAngle = endAngle;
                                endAngle = temp;
                            }

                            if (endAngle < startAngle)
                            {
                                endAngle += 2 * Mathf.PI;
                            }

                            //Debug.LogFormat("arc : {0}  {1}", startAngle, endAngle);
                            arc.startAngle = startAngle;
                            arc.sweepAngle = endAngle - startAngle;
                            arcs.Add(arc);
                        }
                    }
                }
            }

            return arcs;
        }

        /// <summary>
        /// 根据图层，从 dxf 中获取文本列表
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="layers"></param>
        /// <returns></returns>
        public static List<netDxf.Entities.Text> FindTexts(netDxf.DxfDocument doc, List<string> layers)
        {
            List<netDxf.Entities.Text> texts = new List<netDxf.Entities.Text>();
            if (layers == null || layers.Count == 0)
            {
                return texts;
            }

            foreach (netDxf.Entities.Text text in doc.Texts)
            {
                if (layers.Contains(text.Layer.Name))
                {
                    texts.Add(text);
                }
            }

            return texts;
        }
    }
}
