using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aimtec;
using Aimtec.SDK.Events;
using Aimtec.SDK.Util;

namespace Auto_Leveler {

    public class Program {

        private static Random Random = new Random();

        public static void Main(string[] args) {
            GameEvents.GameStart += GameEvents_GameStart;
        }

        private static void GameEvents_GameStart() {
            MenuManager.Create();

            Obj_AI_Base.OnLevelUp += ObjAiBaseOnOnLevelUp;
        }

        private static void ObjAiBaseOnOnLevelUp(Obj_AI_Base objAi, Obj_AI_BaseLevelUpEventArgs e) {
            if (objAi.GetType() == typeof(Obj_AI_Hero)) {
                Obj_AI_Hero hero = (Obj_AI_Hero) objAi;

                List<KeyValuePair<SpellSlot, int>> pairs = MenuManager.Spells.ToList();
                pairs.Sort((p1, p2) => p1.Value.CompareTo(p2.Value));

                foreach (KeyValuePair<SpellSlot, int> pair in pairs) {
                    Spell spell = hero.SpellBook.GetSpell(pair.Key);
                    int maxLevel;

                    if (pair.Key.Equals(SpellSlot.R)) {
                        maxLevel = 3;
                    }
                    else {
                        maxLevel = 5;
                    }

                    if (spell.Level < maxLevel) {
                        Action levelAction = () => hero.SpellBook.LevelSpell(pair.Key);
                        if (MenuManager.HumanizerEnabled()) {
                            DelayAction.Queue(Random.Next(1200, 3300), levelAction);
                        }
                        else {
                            levelAction.Invoke();
                        }
                    }
                }
            }
        }

    }

}