﻿using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RevitCmd
{
    public class RoofElement : _Element
    {
        public RoofElement(IVector position, IVector direction, IShape shape)
            : base(position, direction, shape)
        {
        }

        public override void Draw(Document document)
        {
            TaskDialog.Show("Roof", "drawing roof element");

            var type = document.QuOfType<RoofType>()
                .FirstOrDefault(_ => _.Name == "Ziegeldach 360");

            var level = document.QuOfType<Level>()
                .FirstOrDefault(_ => _.Name == "Ebene 1");

            TaskDialog.Show("Attaching Walls", "Test");
            var walls = document.QuOfType<Wall>();

            foreach (var wall in walls)
            {
                try
                {
                    wall.AddAttachment(level.Id, AttachmentLocation.Top);
                }
                catch
                {
                    TaskDialog.Show("Roof", "drawing roof element");
                }
            }


            var profile = new CurveArray();
            //todo
            var dim = Shape.ToXYZ();
            var profileLines = new List<XYZ>()
            {
                XYZ.Zero,
                new XYZ(dim.X,0,0),
                new XYZ(dim.X,dim.Y,0),
                new XYZ(0, dim.Y,0)
            }.ToLines(closed: true);
            foreach (var curve in profileLines)
            {
                profile.Append(curve);
            }

            var modelCurves = new ModelCurveArray();
            FootPrintRoof roof = document.Create.NewFootPrintRoof(profile, level, type,
                out modelCurves);

            var i = 0;
            foreach (var line in modelCurves.QuCurves<ModelLine>())
            {
                //todo
                if (true/*condition*/)
                {
                    roof.SetSlope(line, 0.5);
                }

                i++;
            }

            roof.EaveCuts = EaveCutterType.TwoCutSquare;
        }
    }
}