using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Util;
using Aimtec.SDK.Util.ThirdParty;
using Rectangle = Xerath.math.Rectangle;

namespace Xerath {

    public class Farming {

        private static readonly int MinimumMinions = 2;


        public static Vector3? GetLineClearLocation(SpellWrapper spell) {
            List<Obj_AI_Base> minions = ObjectManager.Get<Obj_AI_Base>()
                .Where(x => x.IsValidTarget(spell.Range) && x.IsValidSpellTarget()).ToList();
            if (minions.Count < MinimumMinions) {
                return null;
            }

            List<Vector3> minionCoords = minions.Select(x => x.ServerPosition).ToList();

            List<Vector3> possibleAimPositions = new List<Vector3>();
            possibleAimPositions.AddRange(minionCoords);

            for (int i = 0; i < minionCoords.Count(); i++) {
                for (int j = 0; j < minionCoords.Count(); j++) {
                    if (!minionCoords[j].Equals(minionCoords[i])) {
                        possibleAimPositions.Add((minionCoords[j] + minionCoords[i]) / 2);
                    }
                }
            }


            Vector3? bestAimPosition = null;
            int highestMinionCount = MinimumMinions;
            foreach (Vector3 possibleAimPosition in possibleAimPositions) {

                Rectangle spellBoundingBox = new Rectangle(ObjectManager.GetLocalPlayer().Position, possibleAimPosition,
                    spell.Width);

                int count = minions.Count(m => spellBoundingBox.Contains(m.Position) && spell.CanKill(m));
                if (count >= highestMinionCount) {
                    bestAimPosition = possibleAimPosition;
                    highestMinionCount = count;
                }
            }

            return bestAimPosition;
        }

        public static Vector3? GetCircularClearLocation(SpellWrapper spell) {
            List<Obj_AI_Base> minions = ObjectManager.Get<Obj_AI_Base>()
                .Where(x => x.IsValidTarget(spell.Range) && x.IsValidSpellTarget()).ToList();
            List<Vector2> positions = minions.Select(x => x.ServerPosition.To2D()).ToList();

            Vector3? bestAimPosition = null;
            int highestMinionCount = MinimumMinions;

            if (minions.Count >= MinimumMinions) {
                foreach (Vector2 minionCoordinate in positions) {

                    int count = minions.Count(m => m.Distance(minionCoordinate) <= spell.Width && spell.CanKill(m));
                    if (count >= highestMinionCount) {
                        bestAimPosition = minionCoordinate.To3D();
                        highestMinionCount = count;
                    }
                }

                foreach (Vector2 p1 in positions) {
                    foreach (Vector2 p2 in positions) {

                        if (!p1.Equals(p2)) {
                            Vector2 center;
                            float radius;
                            List<Vector2> combination = new List<Vector2>(new Vector2[] { p1, p2 });

                            Mec.FindMinimalBoundingCircle(combination, out center, out radius);

                            int count = minions.Count(m => m.Distance(center) <= spell.Width && spell.CanKill(m));
                            if (count >= highestMinionCount) {
                                bestAimPosition = center.To3D();
                                highestMinionCount = count;
                            }
                        }
                    }
                }
            }

            return bestAimPosition;
        }

    }

}