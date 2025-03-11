
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Transaction = Autodesk.Revit.DB.Transaction;
/* // Draws all the Beams that exist
namespace Hoermann.Revit
{

    [Transaction(TransactionMode.Manual)]
    public class BeamConverter : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiApp = commandData.Application;
            var uiDoc = uiApp.ActiveUIDocument;
            var document = uiDoc.Document;

            using (Autodesk.Revit.DB.Transaction tx = new Transaction(document, "Create Beams"))
            {
                tx.Start();

                var framingSymbols = new FilteredElementCollector(document)
                    .OfClass(typeof(FamilySymbol))
                    r.OfCategory(BuiltInCategory.OST_StructuralFraming)
                    .Cast<FamilySymbol>()
                    .ToList();



                // Werte in interne Revit-Einheiten konvertieren
                double columnSpacing = UnitUtils.ConvertToInternalUnits(1000, UnitTypeId.Millimeters);
                double beamLength = UnitUtils.ConvertToInternalUnits(10000, UnitTypeId.Millimeters);
                double alongLineLength = UnitUtils.ConvertToInternalUnits(15000, UnitTypeId.Millimeters);
                double baseHeight = UnitUtils.ConvertToInternalUnits(3000, UnitTypeId.Millimeters);

                double X = 0;
                double Y = 0;
                double Z = baseHeight;

                Level baseLevel = new FilteredElementCollector(document)
                    .OfClass(typeof(Level))
                    .Cast<Level>()
                    .FirstOrDefault(l => l.Elevation == 0);

                Level topLevel = new FilteredElementCollector(document)
                    .OfClass(typeof(Level))
                    .Cast<Level>()
                    .FirstOrDefault(l => l.Elevation == Z);



                XYZ alongStart = new XYZ(X, Y, Z);
                XYZ alongEnd = new XYZ(X, Y + alongLineLength, Z);

                foreach (var symbol in framingSymbols)
                {
                    if (!symbol.IsActive)
                    {
                        symbol.Activate();
                        document.Regenerate();
                    }

                    XYZ start = new XYZ(X, Y, Z);
                    XYZ end = new XYZ(X + beamLength, Y, Z);
                    Line beamLine = Line.CreateBound(start, end);

                    document.Create.NewFamilyInstance(
                        beamLine,
                        symbol,
                        baseLevel,
                        StructuralType.Beam
                    );
                    break;
                    Y += columnSpacing;
                }

                tx.Commit();
            }
            return Result.Succeeded;
        }
    }
}
*/



namespace Hoermann.Revit
{
    [Transaction(TransactionMode.Manual)]
    public class BeamConverter : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiApp = commandData.Application;
            var uiDoc = uiApp.ActiveUIDocument;
            var document = uiDoc.Document;
            string beamType = "Holz 120 x 160";

            using (Transaction tx = new Transaction(document, "Create Holz Beam"))
            {
                tx.Start();

                var framingSymbol = new FilteredElementCollector(document)
                    .OfClass(typeof(FamilySymbol))
                    .OfCategory(BuiltInCategory.OST_StructuralFraming)
                    .Cast<FamilySymbol>()
                    .FirstOrDefault(fs => fs.Name == beamType);



                if (!framingSymbol.IsActive)
                {
                    framingSymbol.Activate();
                    document.Regenerate();
                }


                double beamLength = UnitUtils.ConvertToInternalUnits(10000, UnitTypeId.Millimeters);
                double baseHeight = UnitUtils.ConvertToInternalUnits(3000, UnitTypeId.Millimeters);

                double X = 0;
                double Y = 0;
                double Z = baseHeight;

                Level baseLevel = new FilteredElementCollector(document)
                    .OfClass(typeof(Level))
                    .Cast<Level>()
                    .FirstOrDefault(l => l.Elevation == 0);



                XYZ start = new XYZ(X, Y, Z);
                XYZ end = new XYZ(X + beamLength, Y, Z);
                Line beamLine = Line.CreateBound(start, end);

                document.Create.NewFamilyInstance(
                    beamLine,
                    framingSymbol,
                    baseLevel,
                    StructuralType.Beam
                );

                tx.Commit();
            }

            return Result.Succeeded;
        }
    }
}













