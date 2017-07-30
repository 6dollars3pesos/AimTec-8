using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace Xerath.math {

    public class Rectangle : Polygon {

        public Rectangle(Vector3 startPosition, Vector3 endPosition, float width) {
            var direction = (startPosition - endPosition).Normalized();
            var perpendicular = Perpendicular(direction);

            var leftBottom = startPosition + width * perpendicular;
            var leftTop = startPosition - width * perpendicular;

            var rightBottom = endPosition - width * perpendicular;
            var rightLeft = endPosition + width * perpendicular;

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