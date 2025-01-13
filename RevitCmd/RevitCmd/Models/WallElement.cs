using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RevitCmd
{
    public class WallElement : _Element
    {
        public WallElement(IVector position, IVector direction, IShape shape)
            : base(position, direction, shape)
        {
        }

        public override void Draw(Document document)
        {
            TaskDialog.Show("Wall", "drawing wall element");

            var type = document.QuOfType<WallType>()
                .First();
            var level = document.QuOfType<Level>()
                .First();

            var pos = Position.ToXYZ();
            var dim = Shape.ToXYZ();
            var line = pos.NewBoundLine(
                pos + new XYZ(dim.X, dim.Y / 2, 0));

            var wall = Wall.Create(document, line,
                type.Id, level.Id, dim.Z,
                0, false, false);
        }
    }
}
