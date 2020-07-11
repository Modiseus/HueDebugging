using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityModManagerNet;

namespace HueDebugging
{
    public class Settings : UnityModManager.ModSettings, IDrawable
    {
        [Draw("Draw Triggers")] public bool DrawTriggers = false;
        [Draw("Draw Radius", Min = 0.5, Max = 10, Precision = 1)] public float DrawRadius = 4;

        [Draw("Selected Layers", Collapsible = true)] public MaskSettings maskSettings = new MaskSettings();

        public int LayerCollisionMask;

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }

        public void OnChange()
        {
            LayerCollisionMask = maskSettings.GetMask();
            Debug.Log(LayerCollisionMask);
        }

    }

    [DrawFields(DrawFieldMask.Public)]
    public class MaskSettings
    {
        public bool TransparentFX = false;
        public bool IgnoreRaycast = false;
        public bool Water = false;
        public bool UI = false;
        public bool Player = false;
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

        private bool[] layers;

        public int GetMask()
        {
            if(layers == null)
            {
                Setup();
            }

            int mask = 0;

            for (int i = 0; i < layers.Length; i++)
            {
                if (layers[i])
                {
                    mask |= 1 << (i + 1);
                }
            }

            return mask;
        }

        private void Setup()
        {
           layers = new bool[] {
                TransparentFX           ,
                IgnoreRaycast           ,
                Water                   ,
                UI                      ,
                Player                  ,
                Triggers                ,
                Slices                  ,
                Scenery                 ,
                Audio                   ,
                PlayerCollider          ,
                Overlay                 ,
                ColouredObjects         ,
                Background              ,
                Ladders                 ,
                PlayerTopCollider       ,
                CollideWithHiddenColours,
                FabricPlayerEvents      ,
                ColouredObjectsHidden   ,
                Lasers                  ,
                PlayerRagdoll           ,
                Trinkets                ,
                TrinketColliders        ,
                TrinketNoSelfCollide    ,
                Locked                  ,
                SceneryBehindColours    ,
                InfrontColours          ,
                ParticleColliders       ,
                InteractiveNonColour
           };
        }

        public void SetToDefaultMask()
        {
            for (int i = 0; i < layers.Length; i++)
            {
                layers[i] = false;
            }

            layers[LayerMask.NameToLayer("Scenery")] = true;
            layers[LayerMask.NameToLayer("ColouredObjects")] = true;
            layers[LayerMask.NameToLayer("Ladders")] = true;
            layers[LayerMask.NameToLayer("InteractiveNonColour")] = true;

        }
    }
}
