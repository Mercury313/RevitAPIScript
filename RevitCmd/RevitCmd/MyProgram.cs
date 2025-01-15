using Autodesk.Revit.DB;

namespace RevitCmd
{
    public static class MyProgram
    {
        static string _docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public static void DoStuff(Document document)
        {
            //var json = System.IO.File.ReadAllText(_docPath + "/model.json");

            //var model = JsonConvert.DeserializeObject<IModel>(json);

            var model = new Model(new List<IElement>
            {
                //todo
                new WallElement(new Vector{ X= 0, Y=0,Z=0 }, new Vector{X = 1, Y =0, Z= 0 }, new Dimension{ X= 10000, Y = 500, Z = 3000 }),
                new RoofElement(new Vector{ X= 0, Y=0,Z=0 }, new Vector{X = 1, Y =0, Z= 0 }, new Dimension{ X= 10000, Y = 5000, Z = 2000 })
            });

            model.Draw(document);

            //todo: attach walls to roof!
            // Done!

        }
    }
}
