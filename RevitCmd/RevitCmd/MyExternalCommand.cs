using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RevitCmd
{
    [Transaction(TransactionMode.Manual)]
    public class MyExternalCommand : IExternalCommand
    {
        public static UIApplication Application { get; private set; }
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiDoc = commandData.Application.ActiveUIDocument;
            Application = commandData.Application;
            TaskDialog.Show($"Hi {Environment.UserName}", $"You are here: {uiDoc.Document.PathName} {uiDoc.ActiveView}");

            try
            {
                RunProgram(uiDoc.Document);
            }
            catch (Exception e)
            {
                TaskDialog.Show(e.GetType().Name, $"{e.Message}{Environment.NewLine}{e.StackTrace}");
            }

            return Result.Succeeded;
        }
        public static void RunProgram(Document document)
        {
            MyProgram.DoStuff(document);
        }
    }
}
