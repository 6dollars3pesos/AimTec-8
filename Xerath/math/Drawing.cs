using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aimtec;

namespace Xerath.math {

    public class Drawing {

        public static void Draw2DCircle(Vector2 centre, float radius, Color color) {
            for (int i = 0; i < 20; i++) {
                float x1 = (float) (centre.X + radius * Math.Cos(i / 20.0 * 2 * Math.PI));
                float y1 = (float) (centre.Y + radius * Math.Sin(i / 20.0 * 2 * Math.PI));

                float x2 = (float) (centre.X + radius * Math.Cos((i + 1) / 20.0 * 2 * Math.PI));
                float y2 = (float) (centre.Y + radius * Math.Sin((i + 1) / 20.0 * 2 * Math.PI));

                Render.Line(x1, y1, x2, y2, color);
            }
        }

    }

}