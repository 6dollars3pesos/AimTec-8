using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aimtec;
using Aimtec.SDK.Util.ThirdParty;

namespace Xerath.math {

    public class Polygon {

        private readonly List<Vector3> _points;

        public Polygon() : this(new List<Vector3>()) { }

        public Polygon(List<Vector3> vec3List) {
            _points = vec3List;
        }

        public void Add(Vector3 vec3) {
            _points.Add(vec3);
        }
        
        public ReadOnlyCollection<Vector3> Points() {
            return _points.AsReadOnly();
        }

        public bool Contains(Vector3 point) {
            List<IntPoint> intPoints = _points.Select(vec3 => new IntPoint(vec3.X, vec3.Z)).ToList();
            IntPoint p = new IntPoint(point.X, point.Z);

            return Clipper.PointInPolygon(p, intPoints) == 1;
        }

    }

}