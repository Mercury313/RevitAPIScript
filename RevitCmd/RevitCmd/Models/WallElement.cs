
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

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
                .First(_ => _.Name == wallType);

            var level = document.QuOfType<Level>()
                .First(_=>_.Name == wallLevel);

            var pos = Position.ToXYZ();

            var dir = Direction.ToXYZ();

            var dim = Shape.ToXYZ();

            pos = new XYZ(pos.X, pos.Y + dim.Y / 2, pos.Z);

            var line = pos.NewBoundLine(new XYZ(pos.X + dir.X, pos.Y + dir.Y, pos.Z + dir.Z)*dim.X);
                

            var wall = Wall.Create(document, line,
                type.Id, level.Id, dim.Z,
                0, false, false);


            //var refArray = new ReferenceArray();


            //var wallRef = new Reference(wall);

            //refArray.Append(wallRef);
            //document.Create.NewDimension(document.ActiveView, line, refArray);
        }
    }
}