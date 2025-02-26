using Autodesk.Revit.DB;
using System.Diagnostics;

namespace RevitCmd.Models
{
    public static class DimensionProgram
    {
        public static void Run(Document document)
        {
            document.Transaction(_ =>
            {

                var Instance = document.QuOfType<Wall>();
                var Dict = new Dictionary<long, List<long>>();



                foreach (var wall in Instance)
                {
                    WallType Name = wall.WallType;
                    var geo = wall.get_Geometry(
                        new Options()
                        {
                            View = MyExternalCommand.Application.ActiveUIDocument.ActiveGraphicalView,
                            ComputeReferences = true, // this is important since we need references for measuring
                            IncludeNonVisibleObjects = false
                        });
                    var solid = geo.OfType<Solid>().First();
                    var bottomFace = solid.Faces.OfType<Face>()
                                    .Where(o => o.ComputeNormal(new UV(.5, .5)).Z < 0).First(); // get bottom face
                    var ordered = bottomFace
                        .EdgeLoops.OfType<EdgeArray>()
                        .SelectMany(o => o.OfType<Edge>())
                        .OrderBy(o => ((Edge)o).ApproximateLength);

                    (IEnumerable<Wall> start, IEnumerable<Wall> end) endingConnections = wall.QuJoined<Wall>().First();

                    var startCon = endingConnections.start;
                    var startCount = startCon.Count();

                    var endCon = endingConnections.end;
                    var endCount = endCon.Count();

                    // Determine dimensions for a wall with no open endings
                    if (startCount > 0 && endCount > 0)
                    {
                        // both single connections // Done
                        if (startCount == 1 && endCount == 1)
                        {
                            Wall firstCorner = startCon.First();
                            Wall secondCorner = endCon.First();

                            var orientation1 = firstCorner.Orientation;


                            var orientation2 = secondCorner.Orientation;


                            var geo1 = firstCorner.get_Geometry(
                                                    new Options()
                                                    {
                                                        View = document.ActiveView,
                                                        ComputeReferences = true,
                                                        IncludeNonVisibleObjects = false
                                                    });
                            var solid1 = geo1.OfType<Solid>().First();

                            var list1 = solid1.Faces.OfType<PlanarFace>().Select(f => f.FaceNormal).ToList();

                            var Face1 = solid1.Faces.OfType<PlanarFace>()
                                        .Where(o => o.FaceNormal.IsAlmostEqualTo(orientation1)).First();

                            var geo2 = secondCorner.get_Geometry(
                                                    new Options()
                                                    {
                                                        View = document.ActiveView,
                                                        ComputeReferences = true,
                                                        IncludeNonVisibleObjects = false
                                                    });
                            var solid2 = geo2.OfType<Solid>().First();

                            var Face2 = solid2.Faces.OfType<PlanarFace>()
                                        .Where(o => o.FaceNormal.IsAlmostEqualTo(orientation2)).First(); // get bottom face


                            var firstFace = Face1.Reference;
                            var secondFace = Face2.Reference;
                            var curve = wall.Location as LocationCurve;
                            var line = curve.Curve as Line;

                            double offset = -2000;

                            document.CreateDimension(firstFace, secondFace, line, offset);
                            Debug.WriteLine("Created dimension for Bi-connected Wall");
                            continue;
                        }
                        // one single start connection
                        if (startCount == 1)
                        {
                            Wall firstCorner = startCon.First();
                            Wall secondCorner = endCon.Where(o => o.Name == wall.Name).First();
                            // Wall secondCorner = endCon.First();

                            var orientation1 = firstCorner.Orientation;


                            var orientation2 = secondCorner.Orientation;


                            var geo1 = firstCorner.get_Geometry(
                                                    new Options()
                                                    {
                                                        View = document.ActiveView,
                                                        ComputeReferences = true,
                                                        IncludeNonVisibleObjects = false
                                                    });
                            var solid1 = geo1.OfType<Solid>().First();

                            var list1 = solid1.Faces.OfType<PlanarFace>().Select(f => f.FaceNormal).ToList();

                            var Face1 = solid1.Faces.OfType<PlanarFace>()
                                        .Where(o => o.FaceNormal.IsAlmostEqualTo(orientation1)).First();

                            var geo2 = secondCorner.get_Geometry(
                                                    new Options()
                                                    {
                                                        View = document.ActiveView,
                                                        ComputeReferences = true,
                                                        IncludeNonVisibleObjects = false
                                                    });
                            var solid2 = geo2.OfType<Solid>().First();

                            var Face2 = solid2.Faces.OfType<PlanarFace>()
                                        .Where(o => o.FaceNormal.IsAlmostEqualTo(orientation2)).First();

                            var firstFace = Face1.Reference;
                            var secondFace = Face2.Reference;
                            var curve = wall.Location as LocationCurve;
                            var line = curve.Curve as Line;

                            double offset = -2000;

                            document.CreateDimension(firstFace, secondFace, line, offset);
                            Debug.WriteLine("Created dimension for Monostart-connected Wall");
                            continue;
                        }
                        // one single end connection
                        if (endCount == 1)
                        {
                            Wall firstCorner = startCon.Where(o => o.Name == wall.Name).First();
                            Wall secondCorner = endCon.First();

                            var orientation1 = firstCorner.Orientation;


                            var orientation2 = secondCorner.Orientation;


                            var geo1 = firstCorner.get_Geometry(
                                                    new Options()
                                                    {
                                                        View = document.ActiveView,
                                                        ComputeReferences = true,
                                                        IncludeNonVisibleObjects = false
                                                    });
                            var solid1 = geo1.OfType<Solid>().First();

                            var list1 = solid1.Faces.OfType<PlanarFace>().Select(f => f.FaceNormal).ToList();

                            var Face1 = solid1.Faces.OfType<PlanarFace>()
                                        .Where(o => o.FaceNormal.IsAlmostEqualTo(orientation1)).First();

                            var geo2 = secondCorner.get_Geometry(
                                                    new Options()
                                                    {
                                                        View = document.ActiveView,
                                                        ComputeReferences = true,
                                                        IncludeNonVisibleObjects = false
                                                    });
                            var solid2 = geo2.OfType<Solid>().First();

                            var Face2 = solid2.Faces.OfType<PlanarFace>()
                                        .Where(o => o.FaceNormal.IsAlmostEqualTo(orientation2)).First(); // get bottom face


                            var firstFace = Face1.Reference;
                            var secondFace = Face2.Reference;
                            var curve = wall.Location as LocationCurve;
                            var line = curve.Curve as Line;

                            double offset = -2000;

                            document.CreateDimension(firstFace, secondFace, line, offset);
                            Debug.WriteLine("Created dimension for Monoend-connected Wall");
                            continue;
                        }
                        // no single connections
                        else
                        {
                            Wall firstCorner = startCon.Where(o => o.Name == wall.Name).First();
                            Wall secondCorner = endCon.Where(o => o.Name == wall.Name).First();

                            var orientation1 = firstCorner.Orientation;


                            var orientation2 = secondCorner.Orientation;


                            var geo1 = firstCorner.get_Geometry(
                                                    new Options()
                                                    {
                                                        View = document.ActiveView,
                                                        ComputeReferences = true,
                                                        IncludeNonVisibleObjects = false
                                                    });
                            var solid1 = geo1.OfType<Solid>().First();

                            var list1 = solid1.Faces.OfType<PlanarFace>().Select(f => f.FaceNormal).ToList();

                            var Face1 = solid1.Faces.OfType<PlanarFace>()
                                        .Where(o => o.FaceNormal.IsAlmostEqualTo(orientation1)).First();

                            var geo2 = secondCorner.get_Geometry(
                                                    new Options()
                                                    {
                                                        View = document.ActiveView,
                                                        ComputeReferences = true,
                                                        IncludeNonVisibleObjects = false
                                                    });
                            var solid2 = geo2.OfType<Solid>().First();

                            var Face2 = solid2.Faces.OfType<PlanarFace>()
                                        .Where(o => o.FaceNormal.IsAlmostEqualTo(orientation2)).First(); // get bottom face


                            var firstFace = Face1.Reference;
                            var secondFace = Face2.Reference;
                            var curve = wall.Location as LocationCurve;
                            var line = curve.Curve as Line;

                            double offset = -2000;

                            document.CreateDimension(firstFace, secondFace, line, offset);
                            Debug.WriteLine("Created dimension for Bi-Multi-connected Wall");
                            continue;
                        }
                    }

                    // Determine dimensions for a wall with one open ending
                    if (startCount > 0 || endCount > 0)
                    {
                        if (startCount > 0)
                        {
                            Wall firstCorner = startCon.First();


                            var orientation1 = firstCorner.Orientation;



                            var geo1 = firstCorner.get_Geometry(
                                                    new Options()
                                                    {
                                                        View = document.ActiveView,
                                                        ComputeReferences = true,
                                                        IncludeNonVisibleObjects = false
                                                    });
                            var solid1 = geo1.OfType<Solid>().First();

                            var list1 = solid1.Faces.OfType<PlanarFace>().Select(f => f.FaceNormal).ToList();

                            var Face1 = solid1.Faces.OfType<PlanarFace>()
                                        .Where(o => o.FaceNormal.IsAlmostEqualTo(orientation1)).First();


                            var firstFace = Face1.Reference;
                            var edge1 = ordered.ElementAt(1).Reference;
                            var curve = wall.Location as LocationCurve;
                            var line = curve.Curve as Line;

                            double offset = -2000;

                            document.CreateDimension(firstFace, edge1, line, offset);
                            Debug.WriteLine("Created dimension for start-connected Wall");
                            continue;
                        }

                        if (endCount > 0)
                        {
                            Wall firstCorner = endCon.First();


                            var orientation1 = firstCorner.Orientation;



                            var geo1 = firstCorner.get_Geometry(
                                                    new Options()
                                                    {
                                                        View = document.ActiveView,
                                                        ComputeReferences = true,
                                                        IncludeNonVisibleObjects = false
                                                    });
                            var solid1 = geo1.OfType<Solid>().First();

                            var list1 = solid1.Faces.OfType<PlanarFace>().Select(f => f.FaceNormal).ToList();

                            var Face1 = solid1.Faces.OfType<PlanarFace>()
                                        .Where(o => o.FaceNormal.IsAlmostEqualTo(orientation1)).First();


                            var firstFace = Face1.Reference;
                            var edge1 = ordered.ElementAt(1).Reference;
                            var curve = wall.Location as LocationCurve;
                            var line = curve.Curve as Line;

                            double offset = -2000;

                            document.CreateDimension(edge1, firstFace, line, offset);
                            Debug.WriteLine("Created dimension for end-connected Wall");
                            continue;
                        }


                    }


                    // Determine dimensions for a wall with two open endings
                    else
                    {
                        var edge0 = ordered.ElementAt(0).Reference;
                        var edge1 = ordered.ElementAt(1).Reference;
                        var line = ordered.ElementAt(3).AsCurve() as Line;
                        double offset = 2000;

                        document.CreateDimension(edge0, edge1, line, offset);
                        Debug.WriteLine("Created dimension for Un-connected Wall");
                    }

                }

            });
        }
    }
}
