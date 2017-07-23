using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aimtec;

namespace Xerath {

    class Utils {

        public static Random Random = new Random();


        public static Vector3 RandomizeVector(Vector3 vector, int deviation) {
            int randomX = Random.Next(0, deviation);

            if (Random.NextDouble() >= 0.5) {
                randomX *= -1;
            }

            int randomY = Random.Next(0, deviation);

            if (Random.NextDouble() >= 0.5) {
                randomY *= -1;
            }

            vector.X += randomX;
            vector.Y += randomY;
            return vector;
        }


    }

}