using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aimtec;
using Aimtec.SDK.Events;
using Aimtec.SDK.Util;

namespace Recall_Tracker {

    public class Program {

        private static readonly List<string> RecallingChampions = new List<string>();

        private static readonly int BaseY = (int) (Render.Height - Render.Height / 3.7);
        private static readonly int BaseX = (int) (Render.Width / 2.0);

        public static void Main(string[] args) {
            GameEvents.GameStart += GameEvents_GameStart;
        }

        private static void GameEvents_GameStart() {
            Obj_AI_Base.OnTeleport += ObjAiHeroOnOnTeleport;
            Render.OnPresent += Render_OnPresent;
        }

        private static void Render_OnPresent() {
            int dY = 0;
            foreach (string championName in RecallingChampions) {
                string text = championName + " is recalling";
                float textWidth = MiscUtils.MeasureTextWidth(text);
                Render.Text(BaseX - textWidth / 2, BaseY + dY, Color.Red, text);
                dY += 20;
            }
        }

        private static void ObjAiHeroOnOnTeleport(Obj_AI_Base sender, Obj_AI_BaseTeleportEventArgs e) {
            if (sender != null && sender.IsValid && sender.IsEnemy && sender.GetType() == typeof(Obj_AI_Hero) &&
                e.DisplayName != null) {
                Obj_AI_Hero hero = (Obj_AI_Hero) sender;
                switch (e.DisplayName) {
                    case "Recall":
                        RecallingChampions.Add(hero.ChampionName);
                        break;
                    case "":
                        RecallingChampions.Remove(hero.ChampionName);
                        break;
                }
            }
        }

    }

}