using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aimtec.SDK.Menu;
using Aimtec.SDK.Menu.Components;

namespace TempEvade {

    public class MenuManager {

        public static Menu Menu = new Menu("tempEvade", "TempEvade", true);

        public static void Create() {
            Menu.Add(new MenuBool("enabled", "Enabled"));
            Menu.Attach();
        }

    }

}