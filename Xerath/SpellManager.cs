using System;
using System.Collections.Generic;
using Aimtec;
using Aimtec.SDK.Prediction.Skillshots;
using Spell = Aimtec.SDK.Spell;

namespace Xerath {

    public class SpellManager {

        public static Dictionary<SpellSlot, SpellWrapper> Spells = new Dictionary<SpellSlot, SpellWrapper>();


        public static SpellWrapper Get(SpellSlot spellSlot) {
            return Spells[spellSlot];
        }

        static SpellManager() {
            Spells[SpellSlot.Q] = new SpellWrapper(SpellSlot.Q, 8, 1400);
            Spells[SpellSlot.W] = new SpellWrapper(SpellSlot.W, 15, 1100);
            Spells[SpellSlot.E] = new SpellWrapper(SpellSlot.E, 5, 1050);
            Spells[SpellSlot.R] = new SpellWrapper(SpellSlot.R, 5, 6160);

            Spells[SpellSlot.Q].SetSkillshot(0.6f, 95f, float.MaxValue, false, SkillshotType.Line, false, HitChance.VeryHigh);
            Spells[SpellSlot.Q].SetCharged("XerathArcanopulseChargeUp", "XerathArcanopulseChargeUp", 750, 1400, 1.5f);
            Spells[SpellSlot.W].SetSkillshot(0.7f, 125, float.MaxValue, false, SkillshotType.Circle, false, HitChance.VeryHigh);
            Spells[SpellSlot.E].SetSkillshot(0.25f, 60f, 1400f, true, SkillshotType.Line, false, HitChance.VeryHigh);
            Spells[SpellSlot.R].SetSkillshot(0.7f, 130f, float.MaxValue, false, SkillshotType.Circle, false, HitChance.VeryHigh);
        }

    }

}