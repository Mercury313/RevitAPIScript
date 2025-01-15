using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RevitCmd
{
    public class WallElement : _Element
    {
        string wallType;
        string wallLevel;
        public WallElement(IVector position, IVector direction, IShape shape, string wallType = "Ziegel+WD hart 300+160", string wallLevel = "Ebene 0")
            : base(position, direction, shape)
        {
            this.wallType = wallType;
            this.wallLevel = wallLevel;
        }

        public override void Draw(Document document)
        {
            // TaskDialog.Show("Wall", "drawing wall element");

            var type = document.QuOfType<WallType>()
                .FirstOrDefault(_ => _.Name == wallType);

            var level = document.QuOfType<Level>()
                .FirstOrDefault(_=>_.Name == wallLevel);

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