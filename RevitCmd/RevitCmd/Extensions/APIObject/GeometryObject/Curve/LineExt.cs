using Autodesk.Revit.DB;

namespace RevitCmd
{
    public static class LineExt
    {
        public readonly static Line AxisX = XYZ.Zero.NewBoundLine(XYZ.BasisX);
        public readonly static Line AxisY = XYZ.Zero.NewBoundLine(XYZ.BasisY);
        public readonly static Line AxisZ = XYZ.Zero.NewBoundLine(XYZ.BasisZ);
    }
}
