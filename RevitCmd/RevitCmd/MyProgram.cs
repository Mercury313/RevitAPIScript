using Autodesk.Revit.DB;
using Newtonsoft.Json;

namespace RevitCmd
{
    public static class MyProgram
    {
        static string _docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public static void DoStuff(Document document)
        {
            var json = System.IO.File.ReadAllText(_docPath + "/model.json");

            var model = JsonConvert.DeserializeObject<IModel>(json);

            var type = new FilteredElementCollector(document)
                .OfClass(typeof(WallType))
                .First();
            var level = new FilteredElementCollector(document)
                .OfClass(typeof(Level))
                .First();

            foreach (var element in model.Elements)
            {
                var pos = element.Position.ToXYZ();
                var dim = element.Dimension.ToXYZ();
                var line = Line.CreateBound(pos, pos + new XYZ(dim.X, dim.Y / 2, 0));

                var wall = Wall.Create(document, line, type.Id, level.Id, dim.Z, 0, false, false);
            }
        }
    }
}
