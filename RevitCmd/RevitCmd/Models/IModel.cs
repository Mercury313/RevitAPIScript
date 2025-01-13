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
    public interface IShape : IXYZ
    {
    }
    public interface IVector : IXYZ
    {
    }
    public interface IDrawable
    {
        public void Draw(Document document);
    }
    public interface IElement : IDrawable
    {
        IVector Position { get; }
        IVector Direction { get; }
        IShape Shape { get; }
    }
    public interface IModel : IDrawable
    {
        public IEnumerable<IElement> Elements { get; }
    }
}
