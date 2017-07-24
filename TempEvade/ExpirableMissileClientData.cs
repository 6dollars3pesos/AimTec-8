using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aimtec;

namespace TempEvade {

    public class ExpirableMissileClientData {

        public Obj_AI_BaseMissileClientDataEventArgs E { get; }

        private long StorageTime;

        public ExpirableMissileClientData(Obj_AI_BaseMissileClientDataEventArgs e) {
            this.E = e;
            StorageTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }

        public bool Expired() {
            return DateTimeOffset.Now.ToUnixTimeMilliseconds() - StorageTime >= 10 * 1000;
        }

    }

}