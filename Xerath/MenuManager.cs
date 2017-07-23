using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aimtec.SDK.Menu;
using Aimtec.SDK.Menu.Components;
using Aimtec.SDK.Orbwalking;
using Aimtec.SDK.Util;

namespace Xerath {

    public enum RMode {

        None,
        Auto,
        Tap,
        NearMouse

    }

    public class MenuManager {

        public static Menu Menu = new Menu("Xerath", "King Xerx3s", true);

        public static void Create() {
            new Orbwalker().Attach(Menu);

            Menu combo = new Menu("combo", "Combo") {
                new MenuBool("q", "Q"),
                new MenuBool("w", "W"),
                new MenuBool("e", "E"),
            };
            Menu harass = new Menu("harass", "Harass (Mixed Mode)") {
                new MenuBool("q", "Q"),
                new MenuBool("w", "W"),
                new MenuBool("e", "E"),
            };
            Menu rMode = new Menu("rMode", "R Mode") {
                new MenuBool("auto", "Auto"),
                new Menu("tap", "Tap") {
                    new MenuKeyBind("key", "Key", KeyCode.T, KeybindType.Press),
                    new MenuBool("enabled", "Enabled", false),
                },
                new MenuBool("nearMouse", "Near Mouse", false),
            };
            Menu farm = new Menu("laneClear", "Lane Clear") {
                new MenuBool("q", "Q"),
                new MenuBool("w", "W"),
                new MenuSlider("minMana", "Minimum Mana %", 60),
                new MenuBool("enabled", "Enabled"),
            };

            Menu.Add(combo);
            Menu.Add(harass);
            Menu.Add(rMode);
           // Menu.Add(farm);

            Menu.Attach();
        }

        public static RMode GetRMode() {
            if (Menu["rMode"]["auto"].Enabled)
            {
                return RMode.Auto;
            }
            if (Menu["rMode"]["tap"]["enabled"].Enabled) {
                return RMode.Tap;
            }
            else if (Menu["rMode"]["nearMouse"].Enabled) {
                return RMode.NearMouse;
            }

            return RMode.None;
        }

    }

}