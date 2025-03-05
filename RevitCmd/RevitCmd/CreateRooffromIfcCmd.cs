﻿using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitCmd.Models;
using System.Diagnostics;
using Xbim.Common.Geometry;
using Xbim.Ifc;
using Xbim.Ifc2x3.GeometricConstraintResource;
using Xbim.Ifc2x3.GeometricModelResource;
using Xbim.Ifc2x3.GeometryResource;
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
            string fullPath = @"C:\Users\FRMI\source\repos\IFCBaukasten\IFCBaukasten\IFC-Datei\26905_angebot_NR126 1.ifc";
            string roofLevel = "Ebene 1";
            string roofType = "Ziegeldach 360";

            try
            {
                RunProgram(uiDoc.Document, fullPath, roofLevel, roofType);
            }
            catch (Exception e)
            {
                TaskDialog.Show(e.GetType().Name, $"{e.Message}{Environment.NewLine}{e.StackTrace}");
            }

            return Result.Succeeded;
        }
        public static void RunProgram(Document document, string fullPath, string roofLevel, string roofType)
        {



            using var model = IfcStore.Open(fullPath);
            var allRoofSlopes = CalculateRoofSlopes.Run(model);
            //generate Roofs from the calculated Slopes

            foreach (KeyValuePair<IfcRoof, List<XbimVector3D>> entry in allRoofSlopes)

            {
                var count = 0;
                //var Vectors = List;
                var Vectors = entry.Value;
                var isDecomposedBy = entry.Key.IsDecomposedBy.First() as IfcRelAggregates;
                var relatedObjects = isDecomposedBy.RelatedObjects.First() as IfcElementAssembly;
                var relatedObjectsIsDecomposedBy = relatedObjects.IsDecomposedBy.First() as IfcRelAggregates;
                var slab = relatedObjectsIsDecomposedBy.RelatedObjects.First() as IfcSlab;

                var slabRepresentation = slab.Representation as IfcProductDefinitionShape;
                var representationRepresentations = slabRepresentation.Representations.First() as IfcShapeRepresentation;
                var items = representationRepresentations.Items.First() as IfcExtrudedAreaSolid;
                var sweptArea = items.SweptArea as IfcRectangleProfileDef;
                double xDim = sweptArea.XDim;
                xDim = UnitUtils.ConvertToInternalUnits(xDim, UnitTypeId.Millimeters);
                Debug.WriteLine(xDim);
                double yDim = entry.Value[0].Y;
                yDim = UnitUtils.ConvertToInternalUnits(yDim, UnitTypeId.Millimeters);
                Debug.WriteLine(yDim);

                var profile = new CurveArray();

                var type = document.QuOfType<RoofType>()
                .FirstOrDefault(_ => _.Name == roofType);

                var level = document.QuOfType<Level>()
                    .FirstOrDefault(_ => _.Name == roofLevel);

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


                FootPrintRoof roof = null;
                ModelCurveArray modelCurves = new ModelCurveArray();
                document.Transaction(_ =>
                {
                    roof = document.Create.NewFootPrintRoof(profile, level, type,
                                        out modelCurves);
                });

                var slap = isDecomposedBy.RelatedObjects.First() as IfcElementAssembly;
                var objectPlacement = slap.ObjectPlacement as IfcLocalPlacement;
                var relativePlacement = objectPlacement.RelativePlacement as IfcAxis2Placement3D;
                var refDirection = relativePlacement.RefDirection;
                var posRefDirectionVector = new XYZ(refDirection.X, refDirection.Y, refDirection.Z);
                var negRefDirectionVector = new XYZ(-refDirection.X, -refDirection.Y, -refDirection.Z);

                foreach (var line in modelCurves.QuCurves<ModelLine>())
                { // für jede Linie im modelcurves-Array
                    var footPrintLocation = line.Location as LocationCurve;
                    var footPrintCurve = footPrintLocation.Curve as Line;
                    var footPrintVector = footPrintCurve.Direction;

                    if (footPrintVector.IsAlmostEqualTo(posRefDirectionVector) == true)
                    {
                        var slopeVector = Vectors[count];
                        Debug.WriteLine(slopeVector);
                        var slabSlope = slopeVector.Y / slopeVector.X;
                        Debug.WriteLine(slabSlope);
                        document.Transaction(_ =>
                        {
                            roof.SetSlope(line, slabSlope);
                        });
                        count += 1;
                        continue;
                    }
                    if (footPrintVector.IsAlmostEqualTo(negRefDirectionVector) == true)
                    {
                        var slopeVector = Vectors[count];
                        Debug.WriteLine(slopeVector);
                        var slabSlope = slopeVector.Y / slopeVector.X;
                        Debug.WriteLine(slabSlope);
                        document.Transaction(_ =>
                        {
                            roof.SetSlope(line, slabSlope);
                        });
                        count += 1;
                        continue;
                    }
                }
                // roof.EaveCuts = EaveCutterType.TwoCutSquare; // Zum Dachkanten erstellen

            }
        }
    }
}

