
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
        public static Settings settings;

        public static bool Load(UnityModManager.ModEntry modEntry)
        {

            settings = Settings.Load<Settings>(modEntry);

            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;


            modEntry.OnToggle = OnToggle;
            modEntry.OnFixedGUI = OnFixedGUI;
            modEntry.OnFixedUpdate = OnFixedUpdate;


            return true;
        }



        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Draw(modEntry);
            if(GUILayout.Button("Reset Layers"))
            {
                settings.maskSettings.SetToDefaultMask();
            }
        }

        static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Save(modEntry);
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

                PlayerCollision.OnFixedGUI();

                DrawUtil.OnFixedGUI();
            }

        }



    }

}
