using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Prediction.Skillshots;
using Aimtec.SDK.Prediction.Skillshots.AoE;
using Aimtec.SDK.TargetSelector;
using Aimtec.SDK.Util;

namespace Xerath {

    public class Modes {

        private static readonly float[] RRanges = {0, 3500, 4820, 6140};
        private static readonly int[] UltiShots = {0, 3, 4, 5};

        public static void OnMixed() {
            if (MenuManager.Menu["harass"]["q"].Enabled) {
                SpellManager.Get(SpellSlot.Q).CastMob();
            }

            if (MenuManager.Menu["harass"]["w"].Enabled) {
                SpellManager.Get(SpellSlot.W).CastMob();
            }

            if (MenuManager.Menu["harass"]["e"].Enabled) {
                SpellManager.Get(SpellSlot.E).CastMob();
            }
        }

        public static void OnCombo() {
            if (MenuManager.Menu["combo"]["q"].Enabled) {
                SpellManager.Get(SpellSlot.Q).CastMob();
            }

            if (MenuManager.Menu["combo"]["w"].Enabled) {
                SpellManager.Get(SpellSlot.W).CastMob();
            }

            if (MenuManager.Menu["combo"]["e"].Enabled) {
                SpellManager.Get(SpellSlot.E).CastMob();
            }
        }

        public static void OnCastingR() {
            switch (MenuManager.GetRMode()) {
                case RMode.Auto:
                    Obj_AI_Hero t1 = TargetSelector.GetTarget(GetRRange());
                    SpellManager.Get(SpellSlot.R).CastMob(t1);
                    break;
                case RMode.NearMouse:
                    Obj_AI_Hero t2 = TargetSelector
                        .GetOrderedTargets(
                            GetRRange()
                        ).FirstOrDefault(h => h != null && h.Distance(Game.CursorPos) <= Program.RNearMouseRange);

                    SpellManager.Spells[SpellSlot.R].CastMob(t2);
                    break;
                case RMode.Tap:
                    Obj_AI_Hero t3 = TargetSelector.GetTarget(GetRRange());
                    if (Program.TapKeyPressed && SpellManager.Get(SpellSlot.R).CastMob(t3)) {
                        Program.TapKeyPressed = false;
                    }
                    break;
            }
        }

        public static void OnLaneClear() {

            if (!MenuManager.Menu["laneClear"]["enabled"].Enabled || ObjectManager.GetLocalPlayer().ManaPercent() <
                MenuManager.Menu["laneClear"]["minMana"].Value) {
                return;
            }

            if (MenuManager.Menu["laneClear"]["q"].Enabled) {
                Vector3? castPosition = Farming.GetLineClearLocation(SpellManager.Get(SpellSlot.Q));
                if (castPosition != null) {
                    SpellManager.Get(SpellSlot.Q).HitChance = HitChance.Medium;
                    SpellManager.Get(SpellSlot.Q).Cast((Vector3) castPosition);
                    SpellManager.Get(SpellSlot.Q).HitChance = HitChance.High;
                }
                
            }

            if (MenuManager.Menu["laneClear"]["w"].Enabled) {
                Vector3? castPosition = Farming.GetCircularClearLocation(SpellManager.Get(SpellSlot.W));
                if (castPosition != null) {
                    SpellManager.Get(SpellSlot.Q).HitChance = HitChance.Medium;
                    SpellManager.Get(SpellSlot.W).Cast((Vector3) castPosition);
                    SpellManager.Get(SpellSlot.Q).HitChance = HitChance.VeryHigh;
                }
            }
        }

        public static float GetRRange() {
            return RRanges[ObjectManager.GetLocalPlayer().SpellBook.GetSpell(SpellSlot.R).Level];
        }

        public static int GetUltiShots()
        {
            return UltiShots[ObjectManager.GetLocalPlayer().SpellBook.GetSpell(SpellSlot.R).Level];
        }

    }

}