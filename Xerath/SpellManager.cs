using System;
using System.Collections.Generic;
using Aimtec;
using Aimtec.SDK.Prediction.Skillshots;
using Spell = Aimtec.SDK.Spell;

namespace Xerath {

    public class SpellManager {

        private static readonly float MaxERange = 1050;

        public static Dictionary<SpellSlot, SpellWrapper> Spells = new Dictionary<SpellSlot, SpellWrapper>();

        public static SpellWrapper Get(SpellSlot spellSlot) {
            return Spells[spellSlot];
        }

        public static void UpdateERange() {
            float percentage = (float) (MenuManager.Menu["eRange"].Value / 100.0);
            Spells[SpellSlot.E].Range = MaxERange * percentage;
        }

        static SpellManager() {
            Spells[SpellSlot.Q] = new SpellWrapper(SpellSlot.Q, 0, 1400);
            Spells[SpellSlot.W] = new SpellWrapper(SpellSlot.W, 0, 1100);
            Spells[SpellSlot.E] = new SpellWrapper(SpellSlot.E, 0, MaxERange);
            Spells[SpellSlot.R] = new SpellWrapper(SpellSlot.R, 0, 6160);

            UpdateERange();

            Spells[SpellSlot.Q].SetSkillshot(0.6f, 95f, float.MaxValue, false, SkillshotType.Line, false,
                HitChance.VeryHigh);
            Spells[SpellSlot.Q].SetCharged("XerathArcanopulseChargeUp", "XerathArcanopulseChargeUp", 750, 1400, 1.5f);
            Spells[SpellSlot.W].SetSkillshot(0.7f, 125, float.MaxValue, false, SkillshotType.Circle, false,
                HitChance.VeryHigh);
            Spells[SpellSlot.E].SetSkillshot(0.25f, 60f, 1400f, true, SkillshotType.Line, false, HitChance.VeryHigh);
            Spells[SpellSlot.R].SetSkillshot(0.7f, 130f, float.MaxValue, false, SkillshotType.Circle, false,
                HitChance.VeryHigh);
        }

    }

}