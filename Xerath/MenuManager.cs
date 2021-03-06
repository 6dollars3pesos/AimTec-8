﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aimtec;
using Aimtec.SDK.Menu;
using Aimtec.SDK.Menu.Components;
using Aimtec.SDK.Orbwalking;
using Aimtec.SDK.Util;

namespace Xerath {

    public enum RMode {

        None,
        Auto,
        Tap,
        NearMouse,
        SelectedTarget

    }

    public class MenuManager {

        public static Menu Menu = new Menu("Xerath", "King Xerx3s", true);

        public static void Create() {
  
            MenuSeperator title = new MenuSeperator("menuTitle", "King Xerx3s");
            Menu combo = new Menu("combo", "Combo") {
                new MenuSeperator("comboTitle", "Combo"),
                new MenuBool("q", "Q"),
                new MenuBool("w", "W"),
                new MenuBool("e", "E"),
            };
            Menu harass = new Menu("harass", "Harass (Mixed Mode)") {
                new MenuSeperator("harassTitle", "Harass"),
                new MenuBool("q", "Q"),
                new MenuBool("w", "W"),
                new MenuBool("e", "E", false),
            };
            Menu rMode = new Menu("rMode", "R Mode") {
                new MenuSeperator("rModeTitle", "R Mode"),
                new MenuBool("auto", "Auto"),
                new Menu("tap", "Tap") {
                    new MenuKeyBind("key", "Key", KeyCode.T, KeybindType.Press),
                    new MenuBool("enabled", "Enabled", false),
                },
                new MenuBool("nearMouse", "Near Mouse", false),
            };
            Menu farm = new Menu("laneClear", "Lane Clear") {
                new MenuSeperator("laneClearTitle", "Lane Clear"),
                new MenuBool("q", "Q"),
                new MenuBool("w", "W"),
                new MenuSlider("minMana", "Minimum Mana %", 60),
                new MenuBool("enabled", "Enabled"),
            };
            MenuBool gapGloser = new MenuBool("gapCloser", "Auto E on Gap Closer");
            MenuSlider eRange = new MenuSlider("eRange", "E Range", 84);
            Menu drawings = new Menu("drawings", "Drawings") {
                new MenuSeperator("drawingTitle", "Drawings"),
                new MenuBool("q", "Q Range"),
                new MenuBool("w", "W Range"),
                new MenuBool("e", "E Range"),
                new MenuBool("r", "R Range on Minimap"),
                new MenuBool("rKillable", "Draw killable target with R"),
                new MenuBool("rMouse", "Draw Circle Near Mouse in R Near Mouse Mode"),
            };

            eRange.OnValueChanged += ERangeOnOnValueChanged;

            Menu.Add(title);

            Orbwalker orbwalker = new Orbwalker();
            orbwalker.Attach(Menu);

            Menu.Add(combo);
            Menu.Add(harass);
            Menu.Add(rMode);
            Menu.Add(farm);
            Menu.Add(gapGloser);
            Menu.Add(eRange);
            Menu.Add(drawings);

            Menu.Attach();
        }

        private static void ERangeOnOnValueChanged(MenuComponent sender, ValueChangedArgs args) {
            SpellManager.UpdateERange();
        }

        public static RMode GetRMode() {
            if (Menu["rMode"]["auto"].Enabled) {
                return RMode.Auto;
            }
            if (Menu["rMode"]["tap"]["enabled"].Enabled) {
                return RMode.Tap;
            }
            if (Menu["rMode"]["nearMouse"].Enabled) {
                return RMode.NearMouse;
            }

            return RMode.None;
        }

    }

}