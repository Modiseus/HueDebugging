using UnityEngine;
using UnityModManagerNet;

namespace HueDebugging
{
    public enum LayerPreset
    {
        Default, None, All, Custom
    }

    [DrawFields(DrawFieldMask.Public)]
    public class LayerList
    {
        public bool Triggers = false;
        public bool Slices = false;
        public bool Scenery = false;
        public bool Audio = false;
        public bool PlayerCollider = false;
        public bool Overlay = false;
        public bool ColouredObjects = false;
        public bool Background = false;
        public bool Ladders = false;
        public bool PlayerTopCollider = false;
        public bool CollideWithHiddenColours = false;
        public bool FabricPlayerEvents = false;
        public bool ColouredObjectsHidden = false;
        public bool Lasers = false;
        public bool PlayerRagdoll = false;
        public bool Trinkets = false;
        public bool TrinketColliders = false;
        public bool TrinketNoSelfCollide = false;
        public bool Locked = false;
        public bool SceneryBehindColours = false;
        public bool InfrontColours = false;
        public bool ParticleColliders = false;
        public bool InteractiveNonColour = false;

    }
    public class Settings : UnityModManager.ModSettings, IDrawable
    {
        [Draw("Noclip")] public KeyBinding NoclipKey = new KeyBinding() { keyCode=KeyCode.E};
        [Draw("Noclip Speed", Type = DrawType.Slider, Min = 1, Max = 20)] public int NoclipSpeed = 5;

        [Draw("Display Player Position And Velocity")] public bool DisplayPlayerPositionAndVelocity = true;

        [Draw("Drawing Distance", Min = 0.1, Max = 20, Precision = 2)] public float DrawRadius = 4;
        [Draw("Draw Player Collision")] public bool PlayerGroundCheck = true;
        [Draw("Draw Triggers")] public bool DrawTriggers = false;

        [Header("Layer Drawing")]
        [Draw("Preset", DrawType.ToggleGroup)] public LayerPreset Preset = LayerPreset.Default;

        [Draw("", VisibleOn = "Preset|Custom")] public LayerList LayerSettings = GetDefaultLayerSettings();


        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }

        public void OnChange()
        {
            switch (Preset)
            {
                case LayerPreset.None:
                    LayerSettings = new LayerList();
                    break;
                case LayerPreset.Default:
                    LayerSettings = GetDefaultLayerSettings();
                    break;
                case LayerPreset.All:
                    LayerSettings = new LayerList()
                    {
                        Triggers = true,
                        Slices = true,
                        Scenery = true,
                        Audio = true,
                        PlayerCollider = true,
                        Overlay = true,
                        ColouredObjects = true,
                        Background = true,
                        Ladders = true,
                        PlayerTopCollider = true,
                        CollideWithHiddenColours = true,
                        FabricPlayerEvents = true,
                        ColouredObjectsHidden = true,
                        Lasers = true,
                        PlayerRagdoll = true,
                        Trinkets = true,
                        TrinketColliders = true,
                        TrinketNoSelfCollide = true,
                        Locked = true,
                        SceneryBehindColours = true,
                        InfrontColours = true,
                        ParticleColliders = true,
                        InteractiveNonColour = true
                    };
                    break;

            }
        }

        private static LayerList GetDefaultLayerSettings()
        {
            return new LayerList() { Scenery = true, ColouredObjects = true, Ladders = true, InteractiveNonColour = true};
        }

        public int GetMask()
        {
            int mask = 0;

            mask = LayerSettings.Triggers ? SetLayer(9, ref mask) : mask;
            mask = LayerSettings.Slices ? SetLayer(10, ref mask) : mask;
            mask = LayerSettings.Scenery ? SetLayer(11, ref mask) : mask;
            mask = LayerSettings.Audio ? SetLayer(12, ref mask) : mask;
            mask = LayerSettings.PlayerCollider ? SetLayer(13, ref mask) : mask;
            mask = LayerSettings.Overlay ? SetLayer(14, ref mask) : mask;
            mask = LayerSettings.ColouredObjects ? SetLayer(15, ref mask) : mask;
            mask = LayerSettings.Background ? SetLayer(16, ref mask) : mask;
            mask = LayerSettings.Ladders ? SetLayer(17, ref mask) : mask;
            mask = LayerSettings.PlayerTopCollider ? SetLayer(18, ref mask) : mask;
            mask = LayerSettings.CollideWithHiddenColours ? SetLayer(19, ref mask) : mask;
            mask = LayerSettings.FabricPlayerEvents ? SetLayer(20, ref mask) : mask;
            mask = LayerSettings.ColouredObjectsHidden ? SetLayer(21, ref mask) : mask;
            mask = LayerSettings.Lasers ? SetLayer(22, ref mask) : mask;
            mask = LayerSettings.PlayerRagdoll ? SetLayer(23, ref mask) : mask;
            mask = LayerSettings.Trinkets ? SetLayer(24, ref mask) : mask;
            mask = LayerSettings.TrinketColliders ? SetLayer(25, ref mask) : mask;
            mask = LayerSettings.TrinketNoSelfCollide ? SetLayer(26, ref mask) : mask;
            mask = LayerSettings.Locked ? SetLayer(27, ref mask) : mask;
            mask = LayerSettings.SceneryBehindColours ? SetLayer(28, ref mask) : mask;
            mask = LayerSettings.InfrontColours ? SetLayer(29, ref mask) : mask;
            mask = LayerSettings.ParticleColliders ? SetLayer(30, ref mask) : mask;
            mask = LayerSettings.InteractiveNonColour ? SetLayer(31, ref mask) : mask;


            return mask;
        }

        private int SetLayer(int layer, ref int mask)
        {
            return mask | 1 << layer;
        }

    }


}
