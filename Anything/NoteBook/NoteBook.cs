using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoteBook
{
    class NoteBook
    {
        public void AnythingPluginMain()
        {
            
        }

        public string MdlName { get; } = "Note";

        public string ManageOperation { get; set; } = "None";

        public bool NeedArgument { get; } = true;

        public object Argument { get; set; } = null;

        public bool ApplyForHotKey { get; set; } = true;

        public string HotKey { get; set; } = "Ctrl+Alt+D0";
    }
}
