
using CurveManager;
using HarmonyLib;
using System.Reflection;
using UnityEngine;
using UnityModManagerNet;


namespace HueDebugging
{
#if DEBUG
    [EnableReloading]
#endif
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

            modEntry.OnUpdate = OnUpdate;

            modEntry.OnUnload = Unload;
            
            return true;
        }


        public static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Draw(modEntry);
        }

        public static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Save(modEntry);
        }

        private static void OnFixedUpdate(UnityModManager.ModEntry modEntry, float dt)
        {
            if (modEntry.Active)
            {
                PlayerCollision.OnFixedUpdate(dt);
            }
        }

        private static void OnUpdate(UnityModManager.ModEntry modEntry, float dt)
        {
            if (modEntry.Active)
            {
                PlayerCollision.OnUpdate();
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

        public static bool Unload(UnityModManager.ModEntry modEntry)
        {
            Harmony harmony = new Harmony(modEntry.Info.Id);
            harmony.UnpatchAll(modEntry.Info.Id);

            return true;
        }

        private static void OnFixedGUI(UnityModManager.ModEntry modEntry)
        {
            if (modEntry.Active)
            {
                DrawUtil.DrawText("HueDebugging enabled");
#if DEBUG
                System.Version version = Assembly.GetExecutingAssembly().GetName().Version;
                DrawUtil.DrawText("Version " + version.ToString());
#endif

                CollisionDrawer.DrawAllColliders();

                if (settings.PlayerGroundCheck)
                {
                    PlayerCollision.OnFixedGUI();
                }

                DrawUtil.OnFixedGUI();

            }
        }
    }
}
