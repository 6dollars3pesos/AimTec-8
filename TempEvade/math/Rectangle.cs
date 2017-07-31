using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace TempEvade.math {

    public class Rectangle : Polygon {

        public Rectangle(Vector3 startPosition, Vector3 endPosition, float width) {
            var direction = (startPosition - endPosition).Normalized();
            var perpendicular = Perpendicular(direction);

            var leftBottom = startPosition + perpendicular * width;
            var leftTop = startPosition - perpendicular * width;

            var rightBottom = endPosition - perpendicular * width;
            var rightLeft = endPosition + perpendicular * width;


            Add(leftBottom);
            Add(leftTop);
            Add(rightBottom);
            Add(rightLeft);
        }


        public Vector3 Perpendicular(Vector3 v) {
            return new Vector3(-v.Z, v.Y, v.X);
        }

    }

}