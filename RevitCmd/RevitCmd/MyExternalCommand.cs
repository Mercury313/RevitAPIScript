using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RevitCmd
{
    [Transaction(TransactionMode.Manual)]
    public class MyExternalCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiDoc = commandData.Application.ActiveUIDocument;
            TaskDialog.Show($"Hi {Environment.UserName}", $"You are here: {uiDoc.Document.PathName} {uiDoc.ActiveView}");

            try
            {
                MyTransaction(uiDoc.Document);
            }
            catch (Exception e)
            {
                TaskDialog.Show(e.GetType().Name, $"{e.Message}{Environment.NewLine}{e.StackTrace}");
            }

            return Result.Succeeded;
        }

        private void MyTransaction(Document document)
        {
            using (var txn = new Transaction(document, "MyTxn"))
            {
                txn.Start();

                try
                {
                    MyProgram.DoStuff(document);

                    txn.Commit();
                }
                catch
                {
                    if (txn.HasStarted())
                    {
                        txn.RollBack();
                    }

                    throw;
                }
            }
        }
    }
}
