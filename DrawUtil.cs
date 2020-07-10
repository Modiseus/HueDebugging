using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace HueDebugging
{
    public class DrawUtil : MonoBehaviour
    {
        private static Dictionary<String, ColoredText> dictText = new Dictionary<string, ColoredText>();
        private static Dictionary<String, LineRenderer> dictLine = new Dictionary<string, LineRenderer>();
        private static Dictionary<String, Line> dictLine2 = new Dictionary<string, Line>();

        public static void DrawText(String key, String text, Color color)
        {
            
            dictText[key] = new ColoredText(text, color);

        }

        public static void DrawText(String key, String text)
        {
            DrawText(key, text, Color.white);
        }

        public static void DrawLine(String key, Vector3 start, Vector3 end, Color color)
        {

            dictLine2[key] = new Line(start, end, color);

            LineRenderer lr;
            if (dictLine.ContainsKey(key))
            {
                lr = dictLine[key];

                if (lr == null)
                {
                    dictLine.Remove(key);
                    lr = createLineRenderer();
                }

            }
            else
            {
                lr = createLineRenderer();
            }


            lr.SetColors(color, color);

            //TODO

            //lr.gameObject.layer = LayerMask.NameToLayer("Background");
            //lr.gameObject.layer = -1;
            //lr.gameObject.layer = LayerMask.NameToLayer("PlayerCollider");
            //lr.gameObject.layer = LayerMask.NameToLayer("ColouredObjects");
            //lr.gameObject.layer = LayerMask.NameToLayer("ColouredObjectsHidden");
            //lr.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            //lr.gameObject.layer = LayerMask.NameToLayer("InteractiveNonColour");

            start.z = 0;
            end.z = 0;
            if (lr == null)
            {
                Debug.LogError("lr==null");
                return;
            }
            lr.SetPositions(new Vector3[] { start, end });
            dictLine[key] = lr;
        }

        private static LineRenderer createLineRenderer()
        {
            GameObject gameObj = new GameObject();
            LineRenderer lr = gameObj.AddComponent<LineRenderer>();
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            lr.material = new Material(shader);

            //"Hidden/Internal-Colored"
            //"Particles/Additive"
            //"Particles/Alpha Blended Premultiply"

            lr.SetWidth(0.01f, 0.01f);

            return lr;
        }

        public void Start()
        {
            
        }

        int cameraCount = 0;
        public void Update()
        {

            //DrawText("camera count", Camera.allCamerasCount.ToString());


            //List<Camera> list = new List<Camera>();


            //foreach (Camera cam in Camera.allCameras)
            //{



            //    list.Add(cam);
            //}

            //var ordered = list.OrderBy(c => c.depth);

            //int i = 0;
            //foreach (Camera cam in ordered)
            //{
            //    DrawText("camera " + i + " depth", cam.depth.ToString());
            //    DrawText("camera " + i + " occlusion", cam.useOcclusionCulling.ToString());
            //    DrawText("camera " + i + " mask", Convert.ToString(cam.cullingMask,2));

            //    if (cam.depth != 0)
            //    {
            //        cam.cullingMask = 0;
            //    }

            //    i++;
            //}

            //var camera = ordered.Last();
            //int mask = camera.cullingMask;

            //DrawText("camera " + i + " mask", Convert.ToString(mask, 2));

            //for (int j = 0; j <= 31; j++) //user defined layers start with layer 8 and unity supports 31 layers
            //{
            //    var layerN = LayerMask.LayerToName(j); //get the name of the layer
            //    if ((mask & (1 << j)) != 0) //only add the layer if it has been named (comment this line out if you want every layer)
            //    {

            //        DrawText("camera " + i + " mask " + j, layerN);
            //    }
            //}


            //for (int j = 0; j <= 31; j++) //user defined layers start with layer 8 and unity supports 31 layers
            //{
            //    var layerN = LayerMask.LayerToName(j); //get the name of the layer
            //    if (layerN.Length > 0) //only add the layer if it has been named (comment this line out if you want every layer)
            //    {
            //        DrawText("layer " + j, layerN);

            //    }
            //}


            DrawLine("Test line",new Vector2(0, 0), new Vector2(50, 50), Color.red);


        }


        bool[] disabledCamera = new bool[] { false,false,false,false,false,false};
        int[] masks = new int[6];

        public void OnGUI()
        {



            //GUI.Label(new Rect(50, 50, 100, 50), "Hello World");

            GUI.color = Color.white;

            //draw text

            int width = 450;
            int lineHeight = 20;
            int left = 50;
            int top = 50;

            int posY = top;

            String text = "Hue Debugging";

            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, Color.black);
            GUI.DrawTexture(new Rect(left, top, width, (dictText.Count + 1) * lineHeight), texture);
            GUI.Box(new Rect(left, top, width, (dictText.Count + 1) * lineHeight), text);
            posY += lineHeight;


            foreach (var pair in dictText)
            {
                ColoredText coloredText = pair.Value;
                Color colorBefore = GUI.color;
                GUI.color = coloredText.color;
                GUI.Label(new Rect(left, posY, width, lineHeight), pair.Key + ": " + coloredText.text);
                GUI.color = colorBefore;

                posY += lineHeight;

            }








            //List<Camera> list = new List<Camera>();


            //foreach (Camera cam in Camera.allCameras)
            //{



            //    list.Add(cam);
            //}

            //var ordered = list.OrderBy(c => c.depth);

            //int i = 0;
            //foreach (Camera cam in ordered)
            //{
            //    //int layer = 11;
            //    //if(cam.depth != 3) 
            //    //{ 
            //    //    cam.cullingMask &= ~(1 << layer);
            //    //}


            //    if (!disabledCamera[i])
            //    {
            //        masks[i] = cam.cullingMask;
            //    }

            //    disabledCamera[i] = GUI.Toggle(new Rect(left, posY, width, lineHeight), disabledCamera[i], "camera " + i + " depth " + cam.depth + " mask " + Convert.ToString(cam.cullingMask, 2));

            //    if (disabledCamera[i])
            //    {
            //        cam.cullingMask = 0;
            //    }
            //    else if(masks[i] != 0) 
            //    {
            //        cam.cullingMask = masks[i];
            //    }


            //    posY += lineHeight;
            //    i++;
            //}





        }
        public struct Line
        {
            public Vector3 start { get; set; }
            public Vector3 end { get; set; }
            public Color color { get; set; }

            public Line(Vector3 start, Vector3 end, Color color)
            {
                this.start = start;
                this.end = end;
                this.color = color;
            }

        }

        public struct ColoredText
        {
            public String text { get; set; }
            public Color color { get; set; }

            public ColoredText(String text, Color color)
            {
                this.text = text;
                this.color = color;
            }
        }

    }
}
