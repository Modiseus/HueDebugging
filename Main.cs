
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityModManagerNet;

namespace HueDebugging
{

    [HarmonyPatch(typeof(PlayerNew))]
    [HarmonyPatch("FixedUpdate")]
    static class PlayerNew_FixedUpdate_Patch
    {
        static bool Prefix(PlayerNew __instance, ref float ___timeAfterFallPlayerCanJump, ref float ___isNotGroundedTimer) 
        {
            DrawUtil.DrawText("timeAfterFallPlayerCanJump", ___timeAfterFallPlayerCanJump.ToString());
            DrawUtil.DrawText("isNotGroundedTimer", ___isNotGroundedTimer.ToString());
            return true;
        }
    }

    static class Main
    {
        public static bool enabled;

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            modEntry.OnToggle = OnToggle;
            modEntry.OnFixedGUI = OnFixedGUI;
            modEntry.OnUpdate = Update;

            return true;
        }

        // Called when the mod is turned to on/off.
        // With this function you control an operation of the mod and inform users whether it is enabled or not.
        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value /* active or inactive */)
        {
            if (value)
            {
                Run(modEntry); // Perform all necessary steps to start mod.
            }
            else
            {
                Stop(); // Perform all necessary steps to stop mod.
            }
            
            enabled = value;
            return true; // If true, the mod will switch the state. If not, the state will not change.
        }

        static void OnFixedGUI(UnityModManager.ModEntry modEntry)
        {
            GUILayout.BeginHorizontal();
                        

            GUILayout.EndHorizontal();
        }

        static void Update(UnityModManager.ModEntry modEntry, float dt)
        {
            //camera.CopyFrom(Camera.main);
            //int layer = 3;
            //camera.depth = 3;
            //camera.cullingMask = 1 << layer;
            //camera.enabled = true;
            //camera.clearFlags = CameraClearFlags.Depth;
        }


        private static GameObject drawObject;
        private static Camera camera;

        static void Run(UnityModManager.ModEntry modEntry)
        {
            var harmony = new Harmony(modEntry.Info.Id);
            harmony.PatchAll();


            drawObject = new GameObject();

            drawObject.AddComponent<DrawUtil>();

            UnityEngine.Object.DontDestroyOnLoad(drawObject);

            // camera = drawObject.AddComponent<Camera>();


            //int layer = 14;
            //camera.depth = 3;
            //camera.cullingMask = 1<<layer;
            //camera.enabled = true;
            //camera.clearFlags = CameraClearFlags.Depth;
            
        }

        static void Stop()
        {
            UnityEngine.Object.Destroy(drawObject);
        }

    }


}
