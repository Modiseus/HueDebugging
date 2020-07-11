using DoorKey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace HueDebugging
{
    class DrawUtil
    {
        public static void OnFixedGUI()
        {

            foreach (var pair in lineDict)
            {
                Line line = pair.Value;
                DrawLine(line.pointA, line.pointB, line.color, 1);
            }

            GUI.backgroundColor = Color.yellow;

            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            foreach(var pair in textDict)
            {
                GUILayout.Label(pair.Key + ": "+ pair.Value);
            }

            GUILayout.EndScrollView();

        }

        private static Vector2 scrollPosition = Vector2.zero;

        public static Texture2D lineTex;
        private static Dictionary<String, Line> lineDict = new Dictionary<string, Line>();

        private static Dictionary<String, String> textDict = new Dictionary<string, string>();

        public struct Line
        {
            public Vector3 pointA;
            public Vector3 pointB;
            public Color color;
        }

        public static void AddText(String key, String text)
        {
            textDict[key] = text;
        }

        public static void AddLine(String key, Vector3 pointA, Vector3 pointB, Color color)
        {

            Line line = new Line();
            line.pointA = pointA;
            line.pointB = pointB;
            line.color = color;

            lineDict[key] = line;

        }
        public static void DrawLine(Vector3 pointA, Vector3 pointB, Color color)
        {
            DrawLine(pointA, pointB, color, 1);
        }

        public static void DrawLine(Vector3 pointA, Vector3 pointB, Color color, float width)
        {
            //transform from world to screen coordinates
            pointA = Camera.main.WorldToScreenPoint(pointA);
            pointA.y = Screen.height - pointA.y;

            pointA.x = (float)Math.Round(pointA.x);
            pointA.y = (float)Math.Round(pointA.y);

            pointB = Camera.main.WorldToScreenPoint(pointB);
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
}
