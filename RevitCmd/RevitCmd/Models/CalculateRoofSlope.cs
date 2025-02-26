using Autodesk.Revit.UI;
using Xbim.Common.Geometry;
using Xbim.Ifc;
using Xbim.Ifc2x3.GeometricConstraintResource;
using Xbim.Ifc2x3.GeometryResource;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.Ifc2x3.SharedBldgElements;

namespace RevitCmd.Models
{
    public static class CalculateRoofSlopes
    {
        public static Dictionary<IfcRoof, List<XbimVector3D>> Run(string fullpath)
        {
            fullpath = @"C:\Users\FRMI\source\repos\IFCBaukasten\IFCBaukasten\IFC-Datei\26905_angebot_NR126 1.ifc";
            fullpath = Path.GetFullPath(fullpath);
            using var model = IfcStore.Open(fullpath);
            var roofs = model.Instances.OfType<IfcRoof>();
            var allRoofSlopes = new Dictionary<IfcRoof, List<XbimVector3D>>();


            try
            {
                foreach (IfcRoof roof in roofs)
                {
                    var roofSlopes = new List<XbimVector3D>();
                    Console.WriteLine(roof.ToString());
                    var isDecomposedBy = roof.IsDecomposedBy.First() as IfcRelAggregates;

                    var relatedObjectsNumber = isDecomposedBy.RelatedObjects.Count;
                    var relatedObjects = isDecomposedBy.RelatedObjects[0] as IfcElementAssembly;
                    Console.WriteLine(relatedObjects.ToString());
                    relatedObjectsNumber = relatedObjectsNumber - 1;
                    while (relatedObjectsNumber >= 0)
                    {
                        var roofSide = isDecomposedBy.RelatedObjects[relatedObjectsNumber] as IfcElementAssembly;
                        relatedObjectsNumber = relatedObjectsNumber - 1;
                        if (roofSide == null)
                        {
                            continue;
                        }
                        var objectPlacement = roofSide.ObjectPlacement as IfcLocalPlacement;
                        var relativePlacement = objectPlacement.RelativePlacement as IfcAxis2Placement3D;
                        var axis = relativePlacement.Axis;
                        var refDirection = relativePlacement.RefDirection;

                        var aVector = new XbimVector3D(axis.X, axis.Y, axis.Z);
                        var dVector = new XbimVector3D(refDirection.X, refDirection.Y, refDirection.Z);

                        double sVectorX = aVector.Y * dVector.Z - aVector.Z - dVector.Y;
                        double sVectorY = aVector.Z * dVector.X - aVector.X - dVector.Z;
                        double sVectorZ = aVector.X * dVector.Y - aVector.Y - dVector.X;

                        var slopeVector = new XbimVector3D(sVectorX, sVectorY, sVectorZ);
                        roofSlopes.Add(slopeVector);

                    }
                    allRoofSlopes.Add(roof, roofSlopes);
                }
            }

            catch
            {
                string errorMessage = "Something went wrong while calculating roof slopes";
                TaskDialog.Show("Error", errorMessage);
            }
            return allRoofSlopes;
        }
    }
}
