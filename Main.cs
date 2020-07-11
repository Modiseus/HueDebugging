
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityModManagerNet;

namespace HueDebugging
{

    public static class Main
    {

        public static Texture2D lineTex;

        private static Dictionary<String, Line> lineDict = new Dictionary<string, Line>();
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

                foreach (var pair in lineDict)
                {
                    Line line = pair.Value;
                    DrawLine(line.pointA, line.pointB, line.color, 1);
                }
            }

        }

        public struct Line
        {
            public Vector3 pointA;
            public Vector3 pointB;
            public Color color;
        }

        public static void AddLine(String key, Vector3 pointA, Vector3 pointB, Color color)
        {

            Line line = new Line();
            line.pointA = pointA;
            line.pointB = pointB;
            line.color = color;

            lineDict[key] = line;

        }

        private static void DrawLine(Vector3 pointA, Vector3 pointB, Color color, float width)
        {
            //transform from world to screen coordinates
            pointA = Camera.current.WorldToScreenPoint(pointA);
            pointA.y = Screen.height - pointA.y;

            pointA.x = (float)Math.Round(pointA.x);
            pointA.y = (float)Math.Round(pointA.y);

            pointB = Camera.current.WorldToScreenPoint(pointB);
            pointB.y = Screen.height - pointB.y;

            pointB.x = (float)Math.Round(pointB.x);
            pointB.y = (float)Math.Round(pointB.y);

            // Save the current GUI matrix, since we're going to make changes to it.
            Matrix4x4 matrix = GUI.matrix;

            // Generate a single pixel texture if it doesn't exist
            if (!lineTex) { lineTex = new Texture2D(1, 1); }

            // Store current GUI color, so we can switch it back later,
            // and set the GUI color to the color parameter
            Color savedColor = GUI.color;
            GUI.color = color;

            // Determine the angle of the 
            float angle = Vector3.Angle(pointB - pointA, Vector2.right);

            // Vector3.Angle always returns a positive number.
            // If pointB is above pointA, then angle needs to be negative.
            if (pointA.y > pointB.y) { angle = -angle; }

            // Use ScaleAroundPivot to adjust the size of the 
            // We could do this when we draw the texture, but by scaling it here we can use
            //  non-integer values for the width and length (such as sub 1 pixel widths).
            // Note that the pivot point is at +.5 from pointA.y, this is so that the width of the line
            //  is centered on the origin at pointA.
            GUIUtility.ScaleAroundPivot(new Vector2((pointB - pointA).magnitude, width), new Vector2(pointA.x, pointA.y + 0.5f));

            // Set the rotation for the 
            //  The angle was calculated with pointA as the origin.
            GUIUtility.RotateAroundPivot(angle, pointA);

            // Finally, draw the actual 
            // We're really only drawing a 1x1 texture from pointA.
            // The matrix operations done with ScaleAroundPivot and RotateAroundPivot will make this
            //  render with the proper width, length, and angle.
            GUI.DrawTexture(new Rect(pointA.x, pointA.y, 1, 1), lineTex);

            // We're done.  Restore the GUI matrix and GUI color to whatever they were before.
            GUI.matrix = matrix;
            GUI.color = savedColor;
        }


    }





    [HarmonyPatch(typeof(PlayerNew), "lineCastHighestHit")]
    public static class DebugDraw
    {

        public static void Prefix(PlayerNew __instance, int ___floorRays, CircleCollider2D ___circleCollider, float ___circleColliderWidth,
            float ___circleColliderHeight, LayerMask ___floorLayerMask, float ___maxSlopeAngle)
        {

            //Main.AddLine("TEST",start, end, color);

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
                    Main.AddLine("ground" + i, vector, vector2, Color.green);
                }
                else
                {
                    Main.AddLine("ground" + i, vector, vector2, Color.red);
                }



            }
        }

    }



}
