using Autodesk.Revit.DB;

namespace RevitCmd
{
    public static class MyProgram
    {
        static string _docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public static void DoStuff(Document document)
        {
            //var json = System.IO.File.ReadAllText(_docPath + "/model.json");



            document.Transaction(_ =>
            {
                var model = new Model(new List<IElement>
                {
                    //todo
                    // new WallElement(new Vector{ X= 0, Y=0,Z=0 }, new Vector{X = 1, Y =0, Z= 0 }, new Dimension{ X= 10000, Y = 500, Z = 3000 }),
                    // new WallElement(new Vector{ X= 0, Y=0,Z=0 }, new Vector{X = -1, Y =0, Z= 0 }, new Dimension{ X= 500, Y = 5000, Z = 3000 }),
                    // new RoofElement(new Vector{ X= 0, Y=0,Z=0 }, new Vector{X = 1, Y =0, Z= 0 }, new Dimension{ X= 10000, Y = 5000, Z = 2000 })
                });

                //var json = model.SerializeJson();

                //var file = new FileInfo(_docPath + "/JSON/JsonTest.json");
                //if (!file.Directory.Exists)
                //{
                //    Directory.CreateDirectory(file.DirectoryName);
                //}
                //File.WriteAllText(file.FullName, json);
                //var modelFromjson = json.DeserializeJson<Model>();

                model.Draw(document);
            });

            document.Transaction(_ =>
            {
                var walls = document.QuOfType<Wall>();
                foreach (var wall in walls)
                {
                    var geo = wall.get_Geometry(
                        new Options()
                        {
                            View = document.ActiveView,
                            ComputeReferences = true, // this is important since we need references for measuring
                            IncludeNonVisibleObjects = false
                        });
                    var solids = geo.OfType<Solid>();
                    foreach (var solid in solids)
                    {
                        var bottomFace = solid.Faces.OfType<Face>()
                                        .Where(o => o.ComputeNormal(new UV(.5, .5)).Z < 0).First(); // get bottom face
                        var ordered = bottomFace
                            .EdgeLoops.OfType<EdgeArray>()
                            .SelectMany(o => o.OfType<Edge>())
                            .OrderBy(o => o.ApproximateLength);

                        // first 2 smallest opposit
                        // last 2 longest opposit
                        var edge0 = ordered.ElementAt(0);
                        var edge1 = ordered.ElementAt(1);
                        var line = ordered.ElementAt(3).AsCurve() as Line;
                        double offset = 2000;

                        document.CreateDimension(edge0, edge1, line, offset);
                    }
                }

            });
            //todo: attach walls to roof!
            // Done!
            document.Transaction(_ =>
            {

                var Instance = document.QuOfType<Wall>();
                var Dict = new Dictionary<long, List<long>>();



                foreach (var wall in Instance)
                {


                    Dict[wall.Id.Value] = new List<long>();
                    var curve = wall.Location as LocationCurve;
                    var endList = curve.get_ElementsAtJoin(1);


                    foreach (var endingConnections in wall.QuJoined<Wall>())
                    {
                        // Determine dimensions for a wall with no open endings
                        if (endingConnections.start.Count() > 0 && endingConnections.end.Count() > 0)
                        {
                            // Determine 
                            if (endingConnections.end.Count() + endingConnections.start.Count() == 2)
                            {
                                // Check for 
                            }

                            if 
                        }

                        // Determine dimensions for a wall with one open ending at the end
                        if (endingConnections.start.Count() > 0 && endingConnections.end.Any())
                        {
                            int ct = endingConnections.start.Count();
                            
                            if (ct > 1)
                            {
                                // implement a screening/algorithm for the connected wall that you want to be meashured against

                                // Calculate the dimensions for a wall with one open ending and one multiending
                            }

                            if (ct == 1)
                            {
                                foreach (var cornerWall in Instance)
                                {
                                    if (cornerWall.Id.Value != wall.Id.Value)
                                    {
                                        long test = cornerWall.Id.Value;

                                        if (test == endingConnections.start.First().Id.Value)
                                        {
                                            // Calculate the dimensions for a wall with one open ending and one Corner
                                        }
                                    }
                                }
                                
                            }

                        }

                        // Determine dimensions for a wall with one open ending at the start
                        if (endingConnections.start.Any() && endingConnections.end.Count() > 0)
                        {
                            int ct = endingConnections.end.Count();

                            if (ct > 1)
                            {
                                // implement a screening/algorithm for the connected wall that you want to be meashured against

                                // Calculate the dimensions for a wall with one open ending and one multiending
                            }

                            if (ct == 1)
                            {
                                foreach (var cornerWall in Instance)
                                {
                                    if (cornerWall.Id.Value != wall.Id.Value)
                                    {
                                        long test = cornerWall.Id.Value;

                                        if (test == endingConnections.end.First().Id.Value)
                                        {
                                            // Calculate the dimensions for a wall with one open ending and one Corner
                                        }
                                    }
                                }

                            }
                        }

                        // Determine dimensions for a wall with two open endings
                        if (endingConnections.start.Any() && endingConnections.end.Any())
                        {
                            // Calculate the dimensions for a wall with two open endings
                            goto End;
                        }

                        End:;
                    }


                    var endList2 = curve.get_ElementsAtJoin(0);

                    foreach (var end in endList2)
                    {
                        if (end is Wall w)
                        {
                            if (wall.Id != w.Id)
                                continue;
                            Dict[wall.Id.Value].Add(w.Id.Value);
                        }
                    }
                }

                foreach (var kvp in Dict)
                {

                    int count = kvp.Value.Count();

                    if (count > 0)
                    {
                        // At least one Ending has a connection
                        if (count == 1)
                        {
                            // One ending of the Wall has a connection
                            foreach (var kvp2 in Dict)
                            {
                                foreach (var kvp3 in kvp2.Value)
                                {
                                    if (kvp.Key == kvp3)
                                    {
                                        // Wall has a corner-connection

                                        goto End;
                                    }
                                }
                            }
                            // Wall has a middle-connection
                        }

                        else
                        {
                            // At least one Ending has a corner-connection
                        }
                    }
                    else
                    {
                        // Wall has no connections
                    }
                End:;
                }

            });

        }


    }
}
