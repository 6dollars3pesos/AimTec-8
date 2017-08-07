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
using Xerath.math;

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
            if (MenuGUI.IsChatOpen()) {
                return;
            }

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
            if (MenuManager.Menu["gapCloser"].Enabled && d.Unit.IsHero) {
                Obj_AI_Hero hero = (Obj_AI_Hero) d.Unit;

                if (hero.IsInRange(SpellManager.Get(SpellSlot.E).Range) &&
                    d.EndPos.Distance(ObjectManager.GetLocalPlayer().Position.To2D()) <= AntiGapGloserRange) {
                    SpellManager.Get(SpellSlot.E).CastMob(hero);
                }
            }
        }

        private static void Render_OnPresent() {
            if (MenuManager.Menu["drawings"]["q"].Enabled) {
                SpellManager.Get(SpellSlot.Q).DrawRange(Color.Blue);
            }
            if (MenuManager.Menu["drawings"]["w"].Enabled) {
                SpellManager.Get(SpellSlot.W).DrawRange(Color.Red);
            }
            if (MenuManager.Menu["drawings"]["e"].Enabled) {
                SpellManager.Get(SpellSlot.E).DrawRange(Color.Green);
            }

            if (SpellManager.Get(SpellSlot.R).Ready && MenuManager.Menu["drawings"]["r"].Enabled) {
                Vector2 centre;
                Render.WorldToMinimap(ObjectManager.GetLocalPlayer().Position, out centre);

                Vector3 maxRangePosition = ObjectManager.GetLocalPlayer().Position;
                maxRangePosition.X += Modes.GetRRange();

                Vector2 end;
                Render.WorldToMinimap(maxRangePosition, out end);
                float radius = Math.Abs(end.X - centre.X);

                Drawing.Draw2DCircle(centre, radius, Color.Red);
            }

            if (IsCastingR() && MenuManager.GetRMode().Equals(RMode.NearMouse) &&
                MenuManager.Menu["drawings"]["rMouse"].Enabled) {
                Render.Circle(Game.CursorPos, RNearMouseRange, 30, Color.Orange);
            }

            if (MenuManager.Menu["drawings"]["rKillable"].Enabled && SpellManager.Get(SpellSlot.R).Ready) {
                foreach (Obj_AI_Hero hero in ObjectManager.Get<Obj_AI_Hero>())
                {
                    if (hero != null && hero.IsEnemy && !hero.IsDead && hero.IsVisible) {
                        if (SpellManager.Get(SpellSlot.R).CanKill(hero, Modes.GetUltiShots())) {
                            double overkillDmg = Math.Abs(hero.Health - SpellManager.Get(SpellSlot.R).GetSpellDamage(hero, Modes.GetUltiShots()));
                            Vector2 drawingPosition = new Vector2(hero.FloatingHealthBarPosition.X + 10, hero.FloatingHealthBarPosition.Y - 35);
                            Render.Text(drawingPosition, Color.Red, "OVERKILL DMG: " + overkillDmg.ToString("0"));
                        }
                    }
                }
            }
        }

    }

}