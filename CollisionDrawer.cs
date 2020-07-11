using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace HueDebugging
{

    public static class CollisionDrawer
    {
        private static void DrawCollider(Collider2D col, Color color)
        {
            
            DrawBounds(col.bounds, color);


        }

        private static void DrawBounds(Bounds bounds, Color color)
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
            var colls = Physics2D.OverlapCircleAll(pos, 2);

            foreach(var collider in colls)
            {
                if (!collider.isTrigger)
                {
                    DrawCollider(collider, Color.blue);
                }
            }

        }

    }

}
