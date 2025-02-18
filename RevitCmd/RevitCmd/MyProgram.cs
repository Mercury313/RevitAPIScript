using Autodesk.Revit.DB;

namespace RevitCmd
{
    public static class MyProgram
    {
        static string _docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public static void DoStuff(Document document)
        {
            //var json = System.IO.File.ReadAllText(_docPath + "/model.json");



            var model = new Model(new List<IElement>
            {
                new WallElement(new Vector{ X= 0, Y=0,Z=0 }, new Vector{X = 1, Y =0, Z= 0 }, new Dimension{ X= 10000, Y = 500, Z = 3000 }),
                new WallElement(new Vector{ X= 0, Y=0,Z=0 }, new Vector{X = -1, Y =0, Z= 0 }, new Dimension{ X= 500, Y = 5000, Z = 3000 }),
                new RoofElement(new Vector{ X= 0, Y=0,Z=0 }, new Vector{X = 1, Y =0, Z= 0 }, new Dimension{ X= 10000, Y = 5000, Z = 2000 })
            });

            //var json = model.SerializeJson();

            //var file = new FileInfo(_docPath + "/JSON/JsonTest.json");
            //if (!file.Directory.Exists)
            //{
            //    Directory.CreateDirectory(file.DirectoryName);
            //}
            //File.WriteAllText(file.FullName, json);
            //var modelFromjson = json.DeserializeJson<Model>();

            //document.Transaction(_ =>
            //{
            //    model.Draw(document);
            //});

            //var json = model.SerializeJson();

            //var model2 = json.DeserializeJson<ModelDtO>();

            //todo: attach walls to roof!
            // Done!


            //    {
            //        var geo = wall.get_Geometry(
            //            new Options()
            //            {
            //                View = document.ActiveView,
            //                ComputeReferences = true, // this is important since we need references for measuring
            //                IncludeNonVisibleObjects = false
            //            });
            //        var solids = geo.OfType<Solid>();
            //        foreach (var solid in solids)
            //        {
            //            var bottomFace = solid.Faces.OfType<Face>()
            //                            .Where(o => o.ComputeNormal(new UV(.5, .5)).Z < 0).First(); // get bottom face
            //            var ordered = bottomFace
            //                .EdgeLoops.OfType<EdgeArray>()
            //                .SelectMany(o => o.OfType<Edge>())
            //                .OrderBy(o => o.ApproximateLength);

            //            // first 2 smallest opposit
            //            // last 2 longest opposit
            //            var edge0 = ordered.ElementAt(0);
            //            var edge1 = ordered.ElementAt(1);
            //            var line = ordered.ElementAt(3).AsCurve() as Line;
            //            double offset = 2000;

            //            document.CreateDimension(edge0, edge1, line, offset);
            //        }
            //    }

            //});
            //todo: attach walls to roof!
            // Done!


        }


    }
}
