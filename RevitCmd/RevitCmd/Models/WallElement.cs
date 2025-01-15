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
                .FirstOrDefault(_ => _.Name == "Ziegel+WD hart 300+160");

            var level = document.QuOfType<Level>()
                .First();

            var pos = Position.ToXYZ();


            var dim = Shape.ToXYZ();
            pos = new XYZ(pos.X, pos.Y + dim.Y / 2, pos.Z);
            var line = pos.NewBoundLine(
                pos + new XYZ(dim.X, 0, 0));

            var wall = Wall.Create(document, line,
                type.Id, level.Id, dim.Z,
                0, false, false);
        }
    }
}