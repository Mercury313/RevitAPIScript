using Autodesk.Revit.DB;

namespace RevitCmd
{
    public static class MyProgram
    {
        static string _docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public static void DoStuff(Document document)
        {
            var json = System.IO.File.ReadAllText(_docPath + "/model.json");

            var dim = new Dimension { X = 10000, Y = 200, Z = 3000 }.ToXYZ();
            var pos = new XYZ(0, 0, 0);

            var line = Line.CreateBound(pos, pos + new XYZ(dim.X, dim.Y / 2, 0));

            var type = new FilteredElementCollector(document)
                .OfClass(typeof(WallType))
                .First();
            var level = new FilteredElementCollector(document)
                .OfClass(typeof(Level))
                .First();

            Wall.Create(document, line, type.Id, level.Id, dim.Z, 0, false, false);
        }
    }
}
