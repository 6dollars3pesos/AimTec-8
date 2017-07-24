using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aimtec;
using Aimtec.SDK.Events;
using Aimtec.SDK.Extensions;

namespace TempEvade {

    public class Program {

        private static List<ExpirableMissileClientData> _missileClientData = new List<ExpirableMissileClientData>();

        public static void Main(string[] args) {
            GameEvents.GameStart += GameEvents_GameStart;
        }

        private static void GameEvents_GameStart() {
            Game.OnUpdate += Game_OnUpdate;
            Render.OnPresent += Render_OnPresent;
            Obj_AI_Base.OnProcessSpellCast += ObjAiHeroOnOnProcessSpellCast;
        }

        private static void ObjAiHeroOnOnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs e) {
            if (sender.IsHero) {
                Console.WriteLine(e.SpellData.Name);
                _missileClientData.Add(new ExpirableMissileClientData(e));
            }
        }

        private static void Game_OnUpdate() {
            List<ExpirableMissileClientData> expiredItems = new List<ExpirableMissileClientData>();
            foreach (ExpirableMissileClientData expirableMissileClientData in _missileClientData) {
                if (expirableMissileClientData.Expired()) {
                    expiredItems.Add(expirableMissileClientData);
                }
            }
            foreach (ExpirableMissileClientData expiredItem in expiredItems) {
                _missileClientData.Remove(expiredItem);
            }
        }

        private static void Render_OnPresent() {
            foreach (ExpirableMissileClientData expirableMissileClientData in _missileClientData) {
                Obj_AI_BaseMissileClientDataEventArgs e = expirableMissileClientData.E;
                if (e != null) {
                    Vector3 start = e.Start;
                    Vector3 end = e.End;

                    Vector2 start2D = new Vector2();
                    Render.WorldToScreen(start, out start2D);
                    Vector2 end2D = new Vector2();
                    Render.WorldToScreen(end, out end2D);

                    Render.Line(start2D, end2D, Color.Red);
                }
            }
        }

    }

}