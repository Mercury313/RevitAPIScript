using Autodesk.Revit.DB;

namespace RevitCmd
{
    public class Dimension
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public XYZ ToXYZ(ForgeTypeId unitTypeId = null) => new XYZ(
            UnitUtils.ConvertToInternalUnits(X, unitTypeId ?? UnitTypeId.Millimeters),
            UnitUtils.ConvertToInternalUnits(Y, unitTypeId ?? UnitTypeId.Millimeters),
            UnitUtils.ConvertToInternalUnits(Z, unitTypeId ?? UnitTypeId.Millimeters)
            );
    }
}
