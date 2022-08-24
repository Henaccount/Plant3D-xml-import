using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
//using Autodesk.ProcessPower.PartsRepository.Specification;
using Autodesk.ProcessPower.PlantInstance;
using Autodesk.ProcessPower.ProjectManager;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using AcadApp = Autodesk.AutoCAD.ApplicationServices.Application;
using PlantApp = Autodesk.ProcessPower.PlantInstance.PlantApplication;
using System.IO;
using Autodesk.ProcessPower.DataLinks;
using Autodesk.AutoCAD.Windows;
using Autodesk.AutoCAD.Runtime;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Linq;

namespace pssCommands
{
    public class Program
    {
        public static string Delright(double number, int howmany)
        {
            string strnumber = String.Format("{0:F8}", number);
            strnumber = strnumber.Substring(0, strnumber.Length - howmany);
            return strnumber;
        }



        public static void XMLimport3dpiping()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            string dwgtype = PnPProjectUtils.GetActiveDocumentType();



            if (dwgtype != "Piping")
            {  //"PnId", "Piping", "Ortho", "Iso"

                ed.WriteMessage("\nDrawing must be Piping drawing"

                              + " in the current project.\n");

                return;

            }



            object snapmodesaved = Application.GetSystemVariable("SNAPMODE");
            object osmodesaved = Application.GetSystemVariable("OSMODE");
            object os3dmodesaved = Application.GetSystemVariable("3DOSMODE");

            Application.SetSystemVariable("SNAPMODE", 0);
            Application.SetSystemVariable("OSMODE", 0);
            Application.SetSystemVariable("3DOSMODE", 0);



            String sPath = "";

            String sTypes = "xml" + "; *";






            OpenFileDialog thedialog = new OpenFileDialog("", sPath, sTypes, "Select " + "xml" + " File", OpenFileDialog.OpenFileDialogFlags.DefaultIsFolder);

            System.Windows.Forms.DialogResult dr = thedialog.ShowDialog();



            if (dr != System.Windows.Forms.DialogResult.OK)

                return;


            String xmlfile = thedialog.Filename;

            XDocument xdoc = XDocument.Load(xmlfile);

            foreach (var segments in xdoc.Descendants("PipingNetworkSegment"))
            {
                string LineID = segments.Descendants("GenericAttributes").First().Descendants("LineID").First().Attribute("Value").Value;
                string Specification = segments.Descendants("GenericAttributes").First().Descendants("Specification").First().Attribute("Value").Value;
                string NominalDiameter = segments.Descendants("GenericAttributes").First().Descendants("NominalDiameter").First().Attribute("Value").Value;
                string InsulationThickness = segments.Descendants("GenericAttributes").First().Descendants("InsulationThickness").First().Attribute("Value").Value;

                foreach (string type in new string[] { "Pipe" , "PipingComponent" }){
                    ed.WriteMessage(segments.Attribute("Tag").Value + "\n");
                    foreach (var pipes in segments.Descendants(type))
                    {
                        ed.WriteMessage(pipes.Attribute("Specification").Value + "\n");

                        var nodepoints = new List<Point3d>();

                        foreach (var nodes in pipes.Descendants("Node"))
                        {
                            nodepoints.Add(new Point3d(Convert.ToDouble(nodes.Descendants("Location").First().Attribute("X").Value), Convert.ToDouble(nodes.Descendants("Location").First().Attribute("Y").Value), Convert.ToDouble(nodes.Descendants("Location").First().Attribute("Z").Value)));
                            
                        }
                        
                        foreach (Point3d nodepoint in nodepoints) {
                            if(!nodepoint.Equals(nodepoints.First()))
                                ed.Command("LINE", nodepoints.First(), nodepoint, "");

                        }
                    }
                }

                SelectionFilter linesonly = new SelectionFilter(new TypedValue[] { new TypedValue((int)DxfCode.Start, "Line") });
                PromptSelectionResult psr = ed.SelectAll(linesonly);
                SelectionSet sls = psr.Value;
                
                ed.Command("PLANTCONVERTLINETOPIPE", "Size", NominalDiameter + "\"", "Specification", Specification, sls, "");

            }

           

            Application.SetSystemVariable("SNAPMODE", snapmodesaved);
            Application.SetSystemVariable("OSMODE", osmodesaved);
            Application.SetSystemVariable("3DOSMODE", os3dmodesaved);

            ed.WriteMessage("\nDone");
        }
    }
}
