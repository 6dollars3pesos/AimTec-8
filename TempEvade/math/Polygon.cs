using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aimtec;
using Aimtec.SDK.Util.ThirdParty;

namespace TempEvade.math {

    public class Polygon {

        public List<Vector3> Points { get; set; }

        public Polygon() : this(new List<Vector3>()) { }

        public Polygon(List<Vector3> vec3List) {
            Points = vec3List;
        }

        public void Add(Vector3 vec3) {
            Points.Add(vec3);
        }

        public List<Vector3> GetOutsidePoints(Polygon polygon) {
            List<Vector3> outsidePoints = new List<Vector3>();

            foreach (Vector3 vector3 in polygon.Points) {
                if (!Contains(vector3)) {
                    outsidePoints.Add(vector3);
                }
            }

            return outsidePoints;
        }

        public bool Contains(Polygon polygon) {
            foreach (Vector3 vector3 in polygon.Points) {
                if (!Contains(vector3)) {
                    return false;
                }
            }

            return true;
        }

        public bool ContainsPartially(Polygon polygon) {
            foreach (Vector3 vector3 in polygon.Points) {
                if (Contains(vector3)) {
                    return true;
                }
            }

            return false;
        }

        public bool Contains(Vector3 point) {
            List<IntPoint> intPoints = Points.Select(vec3 => new IntPoint(vec3.X, vec3.Z)).ToList();
            IntPoint p = new IntPoint(point.X, point.Z);

            return Clipper.PointInPolygon(p, intPoints) == 1;
        }

        public void Draw() {
            for (int i = 0; i < Points.Count - 1; i++) {
                DrawLine(i, i + 1);
            }

            DrawLine(Points.Count - 1, 0);
        }

        private void DrawLine(int index1, int index2) {
            Vector3 vec3Start = Points[index1];
            Vector2 vec2Start;
            Render.WorldToScreen(vec3Start, out vec2Start);
            Vector3 vec3End = Points[index2];
            Vector2 vec2End;
            Render.WorldToScreen(vec3End, out vec2End);

            Render.Line(vec2Start, vec2End, Color.Red);
        }

    }

}