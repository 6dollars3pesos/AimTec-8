using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aimtec;

namespace TempEvade.math {

    public class Circle : Polygon {

        public Circle(Vector3 centre, float radius) : this(centre, radius, 20) { }

        public Circle(Vector3 centre, float radius, float units) {
            List<Vector3> polygonPoints = new List<Vector3>();
            for (int i = 0; i < (int) units; i++) {
                float percentage = (float) (i / units * 2 * Math.PI);
                float x = (float) (centre.X + radius * Math.Cos(percentage));
                float z = (float) (centre.Z + radius * Math.Sin(percentage));
                polygonPoints.Add(new Vector3(x, centre.Y, z));
            }

            Points = polygonPoints;
        }

    }

}