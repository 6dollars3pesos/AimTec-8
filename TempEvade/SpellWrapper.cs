using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aimtec;

namespace TempEvade {

    public class SpellWrapper {

        private static long _spellCounter = 0;

        public long Id { get; }

        public float EndTime { get; }

        public Vector3 StartPos { get; }
        public Vector3 EndPos { get; }

        public int CasterId { get; }

        public float Range { get; }

        public float Width { get; }

        public bool Dodged { get; set; }

        public SpellWrapper(float endTime, Vector3 startPos, Vector3 endPos, int casterId, float range, float width) {
            Id = _spellCounter++;
            EndTime = endTime;
            StartPos = startPos;
            EndPos = endPos;
            CasterId = casterId;
            Range = range;
            Width = width;
            Dodged = false;
        }


        protected bool Equals(SpellWrapper other) {
            return Id == other.Id;
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SpellWrapper) obj);
        }

        public override int GetHashCode() {
            return Id.GetHashCode();
        }

    }

}