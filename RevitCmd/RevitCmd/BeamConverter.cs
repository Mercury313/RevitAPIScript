
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Transaction = Autodesk.Revit.DB.Transaction;
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
                    .OfCategory(BuiltInCategory.OST_StructuralFraming)
                    .Cast<FamilySymbol>()
                    .ToList();

                if (!framingSymbols.Any())
                {
                    message = "No beam family symbols found.";
                    return Result.Failed;
                }

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

                if (baseLevel == null || topLevel == null)
                {
                    message = "Base level or top level not found.";
                    return Result.Failed;
                }

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

                    Y += columnSpacing;
                }

                tx.Commit();
            }
            return Result.Succeeded;
        }
    }
}


/*
[Transaction(TransactionMode.Manual)]
public class BeamConverter
{
    ??= new RelayCommand(() => StaticEvents.UIAppActionEvH.Raise(app =>
        {
        var document = ActiveUIDocument.Document;

        using (Transaction tx = new Transaction(document, "Create Beams"))
        {
            tx.Start();

            var framingSymbols = document
                .QuOfType<FamilySymbol>(BuiltInCategory.OST_StructuralFraming)
                .DistinctByFamily();

            double columnSpacing = 1000;
            double beamLength = 10000;
            double alongLineLength = 15000;
            double baseHeight = 3000;

            // Convert the values from millimeters to internal units
            columnSpacing = UnitUtils.ConvertToInternalUnits(columnSpacing, UnitTypeId.Millimeters);
            beamLength = UnitUtils.ConvertToInternalUnits(beamLength, UnitTypeId.Millimeters);
            alongLineLength = UnitUtils.ConvertToInternalUnits(alongLineLength, UnitTypeId.Millimeters);
            baseHeight = UnitUtils.ConvertToInternalUnits(baseHeight, UnitTypeId.Millimeters);

            double X = 0;
            double Y = 0;
            double Z = baseHeight;

            Level baseLevel = document.QuLevels().FirstOrDefault(l => l.Elevation == 0);
            Level topLevel = document.QuLevels().FirstOrDefault(l => l.Elevation == Z);

            if (baseLevel == null || topLevel == null)
                return;

            XYZ alongStart = new XYZ(X, Y, Z);
            XYZ alongEnd = new XYZ(X, Y + alongLineLength, Z);

            foreach (var symbol in framingSymbols)
            {
                if (!symbol.IsValidObject)
                    continue;

                if (!symbol.IsActive)
                    symbol.Activate();

                XYZ start = new XYZ(X, Y, Z);
                XYZ end = new XYZ(X + beamLength, Y, Z);
                Line beamLine = Line.CreateBound(start, end);

                var beamInstance = document.NewFamilyInstance(
                    beamLine,
                    symbol,
                    baseLevel,
                    StructuralType.Beam
                );

                Y += columnSpacing;
            }

            tx.Commit();
        }
    }


*/







