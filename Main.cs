
using HarmonyLib;
using InControl;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityModManagerNet;

namespace HueDebugging
{

    public static class Main
    {
        public static bool Load(UnityModManager.ModEntry modEntry)
        {

            modEntry.OnToggle = OnToggle;
            modEntry.OnFixedGUI = OnFixedGUI;
            modEntry.OnFixedUpdate = OnFixedUpdate;


            return true;
        }

        private static void OnFixedUpdate(UnityModManager.ModEntry modEntry, float dt)
        {
            if (modEntry.Active)
            {

            }
        }

        private static bool OnToggle(UnityModManager.ModEntry modEntry, bool toggle)
        {
            Harmony harmony = new Harmony(modEntry.Info.Id);
            if (toggle)
            {
                harmony.PatchAll();
            }
            else
            {
                harmony.UnpatchAll(modEntry.Info.Id);
            }



            return true;
        }


        private static void OnFixedGUI(UnityModManager.ModEntry modEntry)
        {
            if (modEntry.Active)
            {
                CollisionDrawer.DrawAllColliders();

                DrawUtil.OnFixedGUI();
            }

        }

        public static int floorMask = 0;


    }


    [HarmonyPatch(typeof(PlayerNew), "lineCastHighestHit")]
    public static class GroundCollision
    {

        public static void Prefix(PlayerNew __instance, int ___floorRays, CircleCollider2D ___circleCollider, float ___circleColliderWidth,
            float ___circleColliderHeight, LayerMask ___floorLayerMask, float ___maxSlopeAngle)
        {

            Main.floorMask = ___floorLayerMask;

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
