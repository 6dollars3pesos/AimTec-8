using System;
using System.Threading;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Aimtec;
using Aimtec.SDK.Events;
using Aimtec.SDK.Util;

namespace Auto_Leveler {

    public class Program {

        private static readonly int BaseY = (int) (Render.Height - Render.Height / 4);
        private static readonly int BaseX = (int) (Render.Width / 2.0);

        private static readonly string InvalidSpellConfigurationText = "Invalid spell priority configuration";

        private static readonly float InvalidSpellConfigurationTextWidth =
            MiscUtils.MeasureTextWidth(InvalidSpellConfigurationText);

        private static readonly Random Random = new Random();


        public static void Main(string[] args) {
            GameEvents.GameStart += GameEvents_GameStart;
        }

        private static void GameEvents_GameStart() {
            MenuManager.Create();

            Render.OnPresent += RenderOnOnPresent;
            Obj_AI_Base.OnLevelUp += ObjAiBaseOnOnLevelUp;
        }

        private static void RenderOnOnPresent() {
            if (!MenuManager.HasValidConfiguration()) {
                Render.Text(BaseX - InvalidSpellConfigurationTextWidth / 2, BaseY, Color.Red,
                    InvalidSpellConfigurationText);
            }
        }

        private static void ObjAiBaseOnOnLevelUp(Obj_AI_Base objAi, Obj_AI_BaseLevelUpEventArgs e) {
            if (e.NewLevel < MenuManager.StartAt()) {
                return;
            }

            if (objAi.GetType() != typeof(Obj_AI_Hero)) {
                return;
            }

            Obj_AI_Hero hero = (Obj_AI_Hero) objAi;
            if (hero.Name == null || !hero.Name.Equals(ObjectManager.GetLocalPlayer().Name)) {
                return;
            }

            List<KeyValuePair<SpellSlot, int>> pairs = MenuManager.LevelPriorities.ToList();
            pairs.Sort((p1, p2) => p1.Value.CompareTo(p2.Value));

            if (e.NewLevel <= 3 && MenuManager.LevelQWEAtleastOnce()) {
                foreach (KeyValuePair<SpellSlot, int> pair in pairs) {
                    Spell spell = hero.SpellBook.GetSpell(pair.Key);

                    if (spell.Level == 0) {
                        LevelSpell(pair.Key);
                    }
                }
            }
            else {
                foreach (KeyValuePair<SpellSlot, int> pair in pairs) {
                    Spell spell = hero.SpellBook.GetSpell(pair.Key);
                    int maxLevel;

                    if (pair.Key.Equals(SpellSlot.R)) {
                        maxLevel = 3;
                    }
                    else {
                        maxLevel = 5;
                    }

                    if (spell.Level < maxLevel && e.NewLevel >= MenuManager.LevelAts[pair.Key]) {
                        LevelSpell(pair.Key);
                    }
                }
            }
        }

        private static void LevelSpell(SpellSlot spellSlot) {
            Action levelAction = () => ObjectManager.GetLocalPlayer().SpellBook.LevelSpell(spellSlot);
            ;
            if (MenuManager.HumanizerEnabled()) {
                DelayAction.Queue(Random.Next(1200, 3300), levelAction);
            }
            else {
                levelAction.Invoke();
            }
        }

    }

}