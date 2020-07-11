﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace HueDebugging
{

    public static class CollisionDrawer
    {

        public static bool drawBounds = false;
        public static int circleEdgeCount = 20;


        public static void DrawAllColliders()
        {
            GameManager gm = GameManager.instance;
            if (gm == null)
            {
                return;
            }

            PlayerNew player = gm.Player;
            if (player == null)
            {
                return;
            }

            Vector3 pos = player.circleCollider.bounds.center;
            var colls = Physics2D.OverlapCircleAll(pos, Main.settings.DrawRadius, Main.settings.LayerCollisionMask);

            foreach (var collider in colls)
            {

                if (collider.isTrigger)
                {
                    if (Main.settings.DrawTriggers)
                    {
                        DrawCollider(collider, Color.yellow);
                    }
                }
                else
                {
                    DrawCollider(collider, Color.white);
                }


            }

        }

        private static void DrawCollider(Collider2D col, Color color)
        {

            if (col.GetType() == typeof(BoxCollider2D))
            {
                DrawBox((BoxCollider2D)col, color);
            }
            else if (col.GetType() == typeof(PolygonCollider2D))
            {
                DrawPolygon((PolygonCollider2D)col, color);
            }
            else if (col.GetType() == typeof(EdgeCollider2D))
            {
                DrawEdge((EdgeCollider2D)col, color);
            }
            else if (col.GetType() == typeof(CircleCollider2D))
            {
                DrawCircle((CircleCollider2D)col, color);
            }


            if (drawBounds)
            {
                DrawBounds(col.bounds, color);
            }


        }

        public static void DrawEdge(EdgeCollider2D col, Color color)
        {
            List<Vector2> list = new List<Vector2>();
            foreach (var point in col.points)
            {
                list.Add(col.transform.TransformPoint(point + col.offset));

            }

            Vector2 last = list[0];
            Vector2 current;
            for (int i = 1; i < list.Count; i++)
            {
                current = list[i];
                DrawUtil.DrawLine(last, current, color);
                last = current;
            }
        }

        public static void DrawPolygon(PolygonCollider2D col, Color color)
        {

            List<Vector2> list = new List<Vector2>();
            foreach (var point in col.points)
            {
                list.Add(col.transform.TransformPoint(point + col.offset));

            }

            Vector2 last = list[0];
            Vector2 current;
            for (int i = 1; i < list.Count; i++)
            {
                current = list[i];
                DrawUtil.DrawLine(last, current, color);
                last = current;
            }
            DrawUtil.DrawLine(last, list[0], color);
        }

        public static void DrawCircle(CircleCollider2D col, Color color)
        {
            Vector2 last = col.transform.TransformPoint(col.radius * new Vector2(1, 0) + col.offset);
            Vector2 current;
            for (int i = 1; i <= circleEdgeCount; i++)
            {

                double rad = i * 2.0 * Math.PI / circleEdgeCount;

                float x = Convert.ToSingle(Math.Cos(rad));
                float y = Convert.ToSingle(Math.Sin(rad));

                Vector2 worldPos = col.radius * new Vector2(x, y) + col.offset;

                current = col.transform.TransformPoint(worldPos);

                DrawUtil.DrawLine(last, current, color);

                last = current;

            }

        }

        public static void DrawBox(BoxCollider2D col, Color color)
        {
            float x = col.size.x / 2.0f;
            float y = col.size.y / 2.0f;

            x += col.offset.x;
            y += col.offset.y;


            Vector2 v0 = col.transform.TransformPoint(-x, -y, 0f);
            Vector2 v1 = col.transform.TransformPoint(x, -y, 0f);
            Vector2 v2 = col.transform.TransformPoint(x, y, 0f);
            Vector2 v3 = col.transform.TransformPoint(-x, y, 0f);

            DrawUtil.DrawLine(v0, v1, color);
            DrawUtil.DrawLine(v1, v2, color);
            DrawUtil.DrawLine(v2, v3, color);
            DrawUtil.DrawLine(v3, v0, color);

        }

        public static void DrawBounds(Bounds bounds, Color color)
        {
            Vector2 v0 = bounds.min;
            Vector2 v1 = new Vector2(bounds.min.x, bounds.max.y);
            Vector2 v2 = bounds.max;
            Vector2 v3 = new Vector2(bounds.max.x, bounds.min.y);

            DrawUtil.DrawLine(v0, v1, color);
            DrawUtil.DrawLine(v1, v2, color);
            DrawUtil.DrawLine(v2, v3, color);
            DrawUtil.DrawLine(v3, v0, color);

        }



    }

}
