#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#endregion

namespace RAB_Session01_Solution
{
    [Transaction(TransactionMode.Manual)]
    public class CmdFizzBuzz : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            // Set Variables
            int range = 100;
            XYZ insPoint = new XYZ(0,0,0);
            double offsetValue = 0.05;
            double calcOffset = offsetValue * doc.ActiveView.Scale;
            XYZ offset = new XYZ(0, calcOffset, 0);
            string result = "";

            // Create Filtered Element Collector
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(TextNoteType));

            // Create Transaction (outside of loop)
            Transaction t = new Transaction(doc);
            t.Start("Fizz Buzz");

            // Create Loop
            for (int i = 1; i <= range; i++)
            {
                // Check Fizz Buzz
                if (i % 3 == 0 && i % 5 == 0)
                    result = "FizzBuzz";
                else if (i % 3 == 0)
                    result = "Fizz";
                else if (i % 5 == 0)
                    result = "Buzz";
                else
                    result = i. ToString();

                // Create Text Note
                TextNote curNote = TextNote.Create(doc, doc.ActiveView.Id, 
                    insPoint, result, collector.FirstElementId());

                // Increment Insertion Point
                insPoint = insPoint.Subtract(offset);
            }
        
            // Commit Transaction
            t.Commit();
            t.Dispose();

            
                              

            return Result.Succeeded;
        }
    }
}
