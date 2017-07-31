using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aimtec;
using Aimtec.SDK.Events;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Orbwalking;
using Aimtec.SDK.Prediction.Collision;
using TempEvade.math;
using Rectangle = TempEvade.math.Rectangle;

namespace TempEvade {

    public class Program {

        private static DateTime assemblyLoadTime = DateTime.Now;

        private static List<SpellWrapper> currentSpells = new List<SpellWrapper>();
        private static List<SpellWrapper> deletedSpells = new List<SpellWrapper>();
        private static List<SpellWrapper> newSpells = new List<SpellWrapper>();

        private static bool _blockOrbWalker = false;

        public static float TickCount {
            get { return (int) DateTime.Now.Subtract(assemblyLoadTime).TotalMilliseconds; }
        }

        public static void Main(string[] args) {
            GameEvents.GameStart += GameEvents_GameStart;
        }

        private static void GameEvents_GameStart() {
            MenuManager.Create();

            Game.OnUpdate += Game_OnUpdate;
            Render.OnPresent += Render_OnPresent;

            Obj_AI_Hero.OnProcessSpellCast += ObjAiHeroOnOnProcessSpellCast;
            MissileClient.OnCreate += MissileClientOnOnCreate;
            MissileClient.OnDestroy += MissileClientOnOnDestroy;

            Orbwalker.Implementation.PreMove += ImplementationOnPreMove;
            Orbwalker.Implementation.PreAttack += ImplementationOnPreAttack;
        }

        private static void ImplementationOnPreAttack(object sender, PreAttackEventArgs preAttackEventArgs) {
            if (_blockOrbWalker) {
                preAttackEventArgs.Cancel = true;
            }
        }

        private static void ImplementationOnPreMove(object sender, PreMoveEventArgs preMoveEventArgs) {
            if (_blockOrbWalker) {
                preMoveEventArgs.Cancel = true;
            }
        }

        private static void MissileClientOnOnDestroy(GameObject sender) { }

        private static void MissileClientOnOnCreate(GameObject sender) {
            if (sender.GetType() == typeof(MissileClient)) {
                MissileClient missile = (MissileClient) sender;
                if (!missile.SpellCaster.IsHero || missile.SpellCaster.IsMe || missile.SpellCaster.IsAlly) {
                    return;
                }

                SpellDataBaseRecord spellRecord = SpellDatabase.GetRecord(missile.SpellData.Name);
                if (spellRecord != null) {
                    switch (spellRecord.spellType) {
                        case SpellType.Line:
                            float endTick = spellRecord.spellDelay +
                                            (spellRecord.range / spellRecord.projectileSpeed) * 1000;

                            SpellWrapper spellWrapper = new SpellWrapper(Game.TickCount + endTick,
                                missile.StartPosition, missile.EndPosition,
                                missile.SpellCaster.NetworkId, spellRecord.range * 1.25f, spellRecord.radius * 1.30f);
                            newSpells.Add(spellWrapper);
                            break;
                    }
                }
            }
        }

        private static void ObjAiHeroOnOnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs e) {
            if (sender.IsHero && sender.IsEnemy) {
                SpellDataBaseRecord spellRecord = SpellDatabase.GetRecord(e.SpellData.Name);
                if (spellRecord != null) {
                    switch (spellRecord.spellType) {
                        case SpellType.Line:
                            float endTick = spellRecord.spellDelay +
                                            (spellRecord.range / spellRecord.projectileSpeed) * 1000;

                            SpellWrapper spellWrapper = new SpellWrapper(Game.TickCount + endTick, e.Start, e.End,
                                sender.NetworkId, spellRecord.range * 1.25f, spellRecord.radius * 1.5f);
                            newSpells.Add(spellWrapper);
                            break;
                    }
                }
            }
        }

        private static void Game_OnUpdate() {
            if (!MenuManager.Menu["enabled"].Enabled) {
                return;
            }

            bool shouldBlock = false;

            foreach (SpellWrapper currentSpell in currentSpells) {
                Rectangle spellRectangle = new Rectangle(currentSpell.StartPos, currentSpell.EndPos,
                    currentSpell.Width);
                Circle playerCircle = new Circle(ObjectManager.GetLocalPlayer().Position,
                    ObjectManager.GetLocalPlayer().BoundingRadius);

                if (spellRectangle.ContainsPartially(playerCircle)) {
                    playerCircle.Draw();
                    shouldBlock = true;

                    if (!currentSpell.Dodged) {
                        Obj_AI_Hero caster = ObjectManager.Get<Obj_AI_Hero>()
                            .First(h => h.NetworkId == currentSpell.CasterId);
                        if (Evade(spellRectangle, caster.Position)) {
                            currentSpell.Dodged = true;
                        }
                    }
                }
            }

            if (shouldBlock) {
                _blockOrbWalker = true;
            }
            else {
                _blockOrbWalker = false;
            }

            currentSpells.AddRange(newSpells);

            DeleteOldSpells();

            CheckSpellEnd();
        }

        private static void DeleteOldSpells() {
            foreach (SpellWrapper deletedSpell in deletedSpells) {
                currentSpells.Remove(deletedSpell);
            }
        }

        private static void CheckSpellEnd() {
            foreach (SpellWrapper currentSpell in currentSpells) {
                if (currentSpell.EndTime < Game.TickCount) {
                    deletedSpells.Add(currentSpell);
                }
            }
        }

        private static bool Evade(Polygon spellBox, Vector3 casterPosition) {
            List<Vector3> possibleEscapes = new List<Vector3>();
            float escapeRadius = 60;
            while (possibleEscapes.Count == 0) {
                Circle escapeCircle = new Circle(ObjectManager.GetLocalPlayer().Position, escapeRadius, 50);
                possibleEscapes.AddRange(spellBox.GetOutsidePoints(escapeCircle));
                escapeRadius *= 5;
            }

            Vector3 closestEscape = possibleEscapes.First();
            float smallestRange = closestEscape.Distance(ObjectManager.GetLocalPlayer().Position);
            foreach (Vector3 possibleEscape in possibleEscapes) {
                float possibleRange = possibleEscape.Distance(ObjectManager.GetLocalPlayer().Position);
                if (possibleRange > smallestRange && !NavMesh.WorldToCell(possibleEscape).Flags
                        .HasFlag(NavCellFlags.Wall)) {
                    closestEscape = possibleEscape;
                    smallestRange = possibleRange;
                }
            }

            return ObjectManager.GetLocalPlayer().IssueOrder(OrderType.MoveTo, closestEscape);
        }

        private static void Render_OnPresent() {
            foreach (SpellWrapper currentSpell in currentSpells) {
                Rectangle rectangle = new Rectangle(currentSpell.StartPos, currentSpell.EndPos, currentSpell.Width);
                rectangle.Draw();
            }
        }

    }

}