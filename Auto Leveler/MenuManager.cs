using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aimtec;
using Aimtec.SDK.Menu;
using Aimtec.SDK.Menu.Components;
using Aimtec.SDK.Orbwalking;
using Aimtec.SDK.Util;

namespace Auto_Leveler {

    class MenuManager {

        private static readonly Menu Menu = new Menu("autoLeveler", "Auto Leveler", true);

        public static Dictionary<SpellSlot, int> Spells = new Dictionary<SpellSlot, int>();

        public static void Create() {
            new Orbwalker().Attach(Menu);

            Menu levels = new Menu("levels", "Levels") {
                new MenuSlider("q", "Q", 1, 1, 4),
                new MenuSlider("w", "W", 2, 1, 4),
                new MenuSlider("e", "E", 3, 1, 4),
                new MenuSlider("r", "R", 4, 1, 4)
            };

            levels.OnValueChanged += LevelsOnOnValueChanged;

            Menu.Add(levels);

            Menu.Attach();

            ReadSpellValues();
        }

        private static void ReadSpellValues() {
            Spells[SpellSlot.Q] = Menu["levels"]["q"].Value;
            Spells[SpellSlot.W] = Menu["levels"]["w"].Value;
            Spells[SpellSlot.E] = Menu["levels"]["e"].Value;
            Spells[SpellSlot.R] = Menu["levels"]["r"].Value;
        }

        private static void LevelsOnOnValueChanged(MenuComponent sender, ValueChangedArgs args) {
            if (HasValidPriorities()) {
                ReadSpellValues();
            }
            else {
                Console.WriteLine("Invalid spell priorities!");
            }
        }

        private static bool HasValidPriorities() {
            List<int> priorities = new List<int>(new int[] {
                Menu["levels"]["q"].Value, Menu["levels"]["w"].Value,
                Menu["levels"]["e"].Value, Menu["levels"]["r"].Value
            });

            foreach (int priority in priorities) {
                if (priorities.FindAll(x => x == priority).Count > 1) {
                    return false;
                }
            }

            return true;
        }

    }

}