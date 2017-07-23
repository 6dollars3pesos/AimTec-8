using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aimtec;
using Aimtec.SDK.Events;
using Aimtec.Core;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Damage.JSON;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Menu;
using Aimtec.SDK.Menu.Components;
using Aimtec.SDK.Orbwalking;
using Aimtec.SDK.Prediction.Skillshots;
using Aimtec.SDK.TargetSelector;
using Aimtec.SDK.Util;

namespace Xerath {

    class Program {

        public static readonly float RNearMouseRange = 500;

        public static readonly float AntiGapGloserRange = 350;

        public static bool TapKeyPressed;


        public static void Main(string[] args) {
            GameEvents.GameStart += GameEvents_GameStart;
        }

        private static void GameEvents_GameStart() {
            if (!ObjectManager.GetLocalPlayer().ChampionName.Equals("Xerath")) {
                Console.WriteLine("Wrong champion");
                return;
            }

            MenuManager.Create();

            Game.OnUpdate += Game_OnUpdate;
            Render.OnPresent += Render_OnPresent;
            Dash.HeroDashed += DashOnHeroDashed;
            Game.OnWndProc += GameOnOnWndProc;
        }

        private static void GameOnOnWndProc(WndProcEventArgs wndProcEvent) {
            if (wndProcEvent.Message == (uint) WindowsMessages.WM_KEYUP) {
                MenuKeyBind tapMenu = (MenuKeyBind) MenuManager.Menu["rMode"]["tap"]["key"];
                if (wndProcEvent.WParam == (ulong) tapMenu.Key && IsCastingR()) {
                    TapKeyPressed = true;
                }
            }
        }

        private static void Game_OnUpdate() {
            if (ObjectManager.GetLocalPlayer().IsDead || MenuGUI.IsChatOpen()) {
                return;
            }

            switch (Orbwalker.Implementation.Mode) {
                case OrbwalkingMode.Combo:
                    Modes.OnCombo();
                    break;
                case OrbwalkingMode.Mixed:
                    Modes.OnMixed();
                    break;
                case OrbwalkingMode.Laneclear:
                    Modes.OnLaneClear();
                    break;
            }

            if (IsCastingR()) {
                Modes.OnCastingR();
            }
        }

        private static bool IsCastingR() {
            return ObjectManager.GetLocalPlayer().HasBuff("XerathLocusOfPower2");
        }

        private static void DashOnHeroDashed(object sender, Dash.DashArgs d) {
            if (d.Unit.IsHero) {
                Obj_AI_Hero hero = (Obj_AI_Hero) d.Unit;

                if (hero.IsInRange(SpellManager.Get(SpellSlot.E).Range) &&
                    d.EndPos.Distance(ObjectManager.GetLocalPlayer().Position.To2D()) <= AntiGapGloserRange) {
                    SpellManager.Get(SpellSlot.E).CastMob(hero);
                }
            }
        }

        private static void Render_OnPresent() {
            SpellManager.Get(SpellSlot.Q).DrawRange(Color.Blue);
            SpellManager.Get(SpellSlot.W).DrawRange(Color.Red);
            SpellManager.Get(SpellSlot.E).DrawRange(Color.Green);

            if (IsCastingR() && MenuManager.GetRMode().Equals(RMode.NearMouse)) {
                Render.Circle(Game.CursorPos, RNearMouseRange, 30, Color.Orange);
            }
        }

    }

}