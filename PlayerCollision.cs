
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using HarmonyLib;
using UnityModManagerNet;

namespace HueDebugging
{

    [HarmonyPatch(typeof(PlayerNew), "lineCastHighestHit")]
    public static class PlayerCollision
    {

        private static List<DrawUtil.Line> lineList = new List<DrawUtil.Line>();

        private static Vector2 pos;
        private static Vector2 vel;

        private static bool noclip = false;
        private static bool noclipLast = false;

        private static float moveX;
        private static float moveY;

        public static void OnUpdate()
        {
            if (Main.settings.NoclipKey.Down())
            {
                noclipLast = noclip;
                noclip = !noclip;
            }
            DrawUtil.AddText("Noclip", noclip ? "On" : "Off");


            if (noclip)
            {
                moveX = HueActions.Instance.Move.X;
                if (Mathf.Abs(moveX) < 0.2f)
                {
                    moveX = 0f;
                }
                moveX = Mathf.Clamp(moveX * 1.5f, -1f, 1f);


                moveY = HueActions.Instance.Move.Y;
                if (Mathf.Abs(moveY) < 0.2f)
                {
                    moveY = 0f;
                }
                moveY = Mathf.Clamp(moveY * 1.5f, -1f, 1f);
            }

        }

        public static void OnFixedUpdate(float dt)
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

            Rigidbody2D rigidbody = player.GetComponent<Rigidbody2D>();
            if (rigidbody != null)
            {
                pos = rigidbody.position;
                vel = rigidbody.velocity;

                if (noclip)
                {

                    player.playerState = PlayerNew.PlayerState.isDead;

                    rigidbody.isKinematic = true;

                    rigidbody.position += new Vector2(moveX * dt * Main.settings.NoclipSpeed, moveY * dt * Main.settings.NoclipSpeed);

                }
                else if (noclipLast)
                {
                    player.playerState = PlayerNew.PlayerState.isIdle;
                    rigidbody.isKinematic = false;
                }

                noclipLast = noclip;
            }


        }

        public static void OnFixedGUI()
        {
            //Draw ground detection lines
            foreach (DrawUtil.Line line in lineList)
            {
                DrawUtil.DrawLine(line);
            }

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

            if (Main.settings.DisplayPlayerPositionAndVelocity)
            {
                DrawPlayerPosition(player);
            }

            if (player.door)
            {
                DrawUtil.DrawText("Door: " + player.door.doorID);
            }
            else
            {
                DrawUtil.DrawText("No door");
            }

            CollisionDrawer.DrawCircle(player.circleCollider, Color.white);
            CollisionDrawer.DrawCircle((CircleCollider2D)player.topCollider, Color.white);


        }

        private static string xString = "";
        private static string yString = "";
        private static void DrawPlayerPosition(PlayerNew player)
        {
            DrawUtil.DrawText("Player Position: " + pos.x + " , " + pos.y);

            GUILayout.BeginHorizontal();

            xString = GUILayout.TextField(xString);
            yString = GUILayout.TextField(yString);

            GUILayout.EndHorizontal();

            if (GUILayout.Button("Set Player Position"))
            {
                float x;
                float y;

                if (float.TryParse(xString, out x) && float.TryParse(yString, out y))
                {
                    Rigidbody2D rigidbody = player.GetComponent<Rigidbody2D>();
                    if (rigidbody != null)
                    {
                        rigidbody.position = new Vector2(x, y);
                    }
                }

            }

            DrawUtil.DrawText("Player Velocity: " + vel.x + " , " + vel.y);

        }


        public static void Prefix(PlayerNew __instance, int ___floorRays, CircleCollider2D ___circleCollider, float ___circleColliderWidth,
            float ___circleColliderHeight, LayerMask ___floorLayerMask, float ___maxSlopeAngle)
        {

            //Clear list here since this is only called on FixedUpdate
            lineList.Clear();

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
                    lineList.Add(new DrawUtil.Line(vector, vector2, Color.green));
                }
                else
                {
                    lineList.Add(new DrawUtil.Line(vector, vector2, Color.red));
                }

            }
        }

    }
}
