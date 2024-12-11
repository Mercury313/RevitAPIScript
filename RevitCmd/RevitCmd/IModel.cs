using Autodesk.Revit.DB;

namespace RevitCmd
{
    public interface IXYZ
    {
        double X { get; set; }
        double Y { get; set; }
        double Z { get; set; }

        public XYZ ToXYZ(ForgeTypeId unitTypeId = null) => new XYZ(
            UnitUtils.ConvertToInternalUnits(X, unitTypeId ?? UnitTypeId.Millimeters),
            UnitUtils.ConvertToInternalUnits(Y, unitTypeId ?? UnitTypeId.Millimeters),
            UnitUtils.ConvertToInternalUnits(Z, unitTypeId ?? UnitTypeId.Millimeters));
    }
    public interface IDimension : IXYZ
    {
    }
    public interface IPosition : IXYZ
    {
    }

    public interface IDirection : IXYZ
    {
    }
    public interface IElement
    {
        IPosition Position { get; set; }
        IDirection Direction { get; set; }
        IDimension Dimension { get; set; }
    }
    public interface IModel
    {
        List<IElement> Elements { get; }
    }
}
