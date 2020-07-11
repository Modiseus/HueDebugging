
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using HarmonyLib;

namespace HueDebugging
{

    [HarmonyPatch(typeof(PlayerNew), "lineCastHighestHit")]
    public static class PlayerCollision
    {

        private static List<DrawUtil.Line> lineList = new List<DrawUtil.Line>();

        public static void OnFixedGUI()
        {
            foreach(DrawUtil.Line line in lineList)
            {
                DrawUtil.DrawLine(line);
            }

            lineList.Clear();

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

            CollisionDrawer.DrawCircle(player.circleCollider, Color.white);
            CollisionDrawer.DrawCircle((CircleCollider2D)player.topCollider, Color.white);
        }


        public static void Prefix(PlayerNew __instance, int ___floorRays, CircleCollider2D ___circleCollider, float ___circleColliderWidth,
            float ___circleColliderHeight, LayerMask ___floorLayerMask, float ___maxSlopeAngle)
        {
            //DrawUtil.AddText("floor mask", Convert.ToString(___floorLayerMask, 2));

            //for (int layer = 0; layer < 32; layer++)
            //{
            //    if ((___floorLayerMask & (1 << layer)) != 0)
            //    {
            //        DrawUtil.AddText("floor mask layer " + layer, LayerMask.LayerToName(layer));
            //    }
            //}


            for (int i = 0; i < ___floorRays; i++)
            {
                RaycastHit2D highestHit = default(RaycastHit2D);
                float x = ___circleCollider.bounds.min.x + ___circleColliderWidth / (float)(___floorRays - 1) * (float)i;
                Vector2 vector = new Vector2(x, __instance.transform.position.y);
                Vector2 vector2 = new Vector2(x, __instance.transform.position.y - ___circleColliderHeight);

                RaycastHit2D[] array = Physics2D.LinecastAll(vector, vector2, ___floorLayerMask);
                foreach (RaycastHit2D raycastHit in array)
                {
                    raycastHit.collider.GetComponent<BouncyBlock>();
                    if (raycastHit.collider && !raycastHit.collider.isTrigger)
                    {


                        float floorAngle = Vector2.Angle(Vector2.up, raycastHit.normal);
                        raycastHit.collider.GetComponent<Rigidbody2D>();
                        FloatUpwards component = raycastHit.collider.GetComponent<FloatUpwards>();
                        if (floorAngle < ___maxSlopeAngle && raycastHit.point.y < __instance.transform.position.y && !component)
                        {

                            if (!highestHit.collider)
                            {
                                highestHit = raycastHit;
                                //this.standingMaterial = raycastHit2D2.collider.sharedMaterial;
                            }
                            if (raycastHit.point.y > highestHit.point.y)
                            {
                                highestHit = raycastHit;
                                //this.standingMaterial = raycastHit2D2.collider.sharedMaterial;
                            }
                        }
                    }
                }


                if (highestHit.collider)
                {
                    DrawUtil.AddLine("ground" + i, vector, vector2, Color.green);
                }
                else
                {
                    DrawUtil.AddLine("ground" + i, vector, vector2, Color.red);
                }

            }
        }

    }
}
