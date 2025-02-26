using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Diagnostics;

namespace RevitCmd
{
    [Transaction(TransactionMode.Manual)]
    public class MyExternalCommand : IExternalCommand
    {
        public static UIApplication Application { get; private set; }
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Application = commandData.Application;

            try
            {
                RunProgram(Application.ActiveUIDocument.Document);
            }
            catch (Exception e)
            {
                string displayStackTrace = "";
                var stackframes = new StackTrace(e, true).GetFrames();
                if (stackframes.Count() == 0)
                {
                    displayStackTrace = e.StackTrace;
                }
                else
                {
                    foreach (var frame in stackframes)
                    {
                        if (frame.GetFileName() == null)
                        {
                            continue;
                        }
                        var method = frame.GetMethod();
                        displayStackTrace += $"{method.DeclaringType.FullName}.{method.Name} in {frame.GetFileName()}:line {frame.GetFileLineNumber()}{Environment.NewLine}";
                    }
                }
                TaskDialog.Show(e.GetType().Name, $"{e.Message}{Environment.NewLine}{displayStackTrace}");
            }

            return Result.Succeeded;
        }
        public static void RunProgram(Document document)
        {
            MyProgram.DoStuff(document);
        }
    }
}
