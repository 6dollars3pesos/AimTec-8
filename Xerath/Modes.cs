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

namespace Xerath {

    public class Modes {

        private static readonly float[] RRanges = {0, 3520, 4840, 6160};

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
//            if (ObjectManager.GetLocalPlayer().ManaPercent() < MenuManager.Menu["laneClear"]["minMana"].Value ||
//                !MenuManager.Menu["laneClear"]["enabled"].Enabled) {
//                return;
//            }
//
//            if (MenuManager.Menu["laneClear"]["q"].Enabled) {
//                SpellWrapper spell = SpellManager.Spells[SpellSlot.Q];
//
//                foreach (Obj_AI_Minion minion in ObjectManager.Get<Obj_AI_Minion>()
//                    .Where(minion => minion != null && minion.IsValid && minion.IsMinion && !minion.IsDead &&
//                                     minion.IsInRange(spell.Range))) {
//
//                    PredictionInput predictionInput = spell.GetPredictionInput(minion);
//                    PredictionOutput aoEPrediction = AoePrediction.GetAoEPrediction(new PredictionInput());
//
//                    int killableMinions = aoEPrediction.AoeTargetsHit
//                        .Count(unit => unit.IsMinion && spell.CanKill(unit));
//                    if (killableMinions >= 3) {
//                        spell.CastMob(minion);
//                        return;
//                    }
//                }
//            }
        }

        private static float GetRRange() {
            return RRanges[ObjectManager.GetLocalPlayer().SpellBook.GetSpell(SpellSlot.R).Level];
        }

    }

}