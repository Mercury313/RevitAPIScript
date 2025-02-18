using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitCmd.Models;
using Xbim.Common.Geometry;
using Xbim.Ifc2x3.GeometricModelResource;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.Ifc2x3.ProfileResource;
using Xbim.Ifc2x3.RepresentationResource;
using Xbim.Ifc2x3.SharedBldgElements;

namespace RevitCmd
{
    [Transaction(TransactionMode.Manual)]
    public class CreateRooffromIfcCmd : IExternalCommand
    {
        public static UIApplication Application { get; private set; }
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiDoc = commandData.Application.ActiveUIDocument;
            Application = commandData.Application;
            string fullPath = "";

            try
            {
                RunProgram(uiDoc.Document, fullPath);
            }
            catch (Exception e)
            {
                TaskDialog.Show(e.GetType().Name, $"{e.Message}{Environment.NewLine}{e.StackTrace}");
            }

            return Result.Succeeded;
        }
        public static void RunProgram(Document document, string fullPath)
        {
            var allRoofSlopes = CalculateRoofSlopes.Run(fullPath);

            // generate Roofs from the calculated Slopes
            foreach (KeyValuePair<IfcRoof, List<XbimVector3D>> entry in allRoofSlopes)
            {
                var slab1 = entry.Key.IsDecomposedBy as IfcRelAggregates;
                var slab2 = slab1.RelatedObjects.First() as IfcElementAssembly;
                var slab3 = slab2.IsDecomposedBy.First() as IfcRelAggregates;
                var slab = slab3.RelatedObjects.First() as IfcSlab;
                var slabRepresentation = slab.Representation as IfcProductDefinitionShape;
                var representationRepresentations = slabRepresentation.Representations.First() as IfcShapeRepresentation;
                var items = representationRepresentations.Items.First() as IfcExtrudedAreaSolid;
                var sweptArea = items.SweptArea as IfcRectangleProfileDef;
                double xDim = sweptArea.XDim;
                double yDim = entry.Value[0].Y;
                var profile = new CurveArray();

                var profileLines = new List<XYZ>()
                {
                    XYZ.Zero,
                    new XYZ(xDim, 0, 0),
                    new XYZ(xDim, yDim, 0),
                    new XYZ(0, yDim, 0)
                }.ToLines(closed: true);
                foreach (var curve in profileLines)
                {
                    profile.Append(curve);
                }
                FootPrintRoof roof = document.Create.NewFootPrintRoof(profile, level, type,
                out ModelCurveArray modelCurves);
                foreach (var line in modelCurves.QuCurves<ModelLine>())
                {
                    var test = line.Location as LocationCurve;
                    test.Curve



                    roof.SetSlope(line, 0.5);
                }

                foreach (XbimVector3D vector in entry.Value)
                {
                    roof.SetSlope(line, 0.5);
                }
                roof.EaveCuts = EaveCutterType.TwoCutSquare; // What is this??

            }













        }

    }
}
