using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Autodesk.AutoCAD.Runtime;
using AcadApp = Autodesk.AutoCAD.ApplicationServices.Application;

using Autodesk.AutoCAD.EditorInput;
//v1 smlx -> gtc
//v2 start end / end start suche, bislang nur die haelfte der profile gefunden
//v3 omit last digit (8th) on search, set all offsets to 0

[assembly: CommandClass(typeof(pssCommands.Commands))]

namespace pssCommands
{
    public class Commands
    {
        #region Commands

        [CommandMethod("XMLimport3dpiping", CommandFlags.UsePickSet)]
        public static void XMLimport3dpiping()
        {
            Program.XMLimport3dpiping();
        }



        #endregion
    }
}
