using Autodesk.Revit.DB;

namespace RevitCmd
{
    public static class XyzExt
    {
        public static Line NewBoundLine(this XYZ origin, XYZ vec)
            => Line.CreateBound(origin, vec);

        /// <summary>
        /// connect points by lines, (optionally) close the loop
        /// </summary>
        public static IList<Line> ToLines(this IList<XYZ> points, bool closed = false)
        {
            List<Line> curves = [];
            for (int i = 0; i < points.Count - 1; i++)
            {
                Line curve = points[i].NewBoundLine(points[i + 1]);
                curves.Add(curve);
            }
            if (!closed)
                return curves;

            Line closingC = points[points.Count - 1].NewBoundLine(points[0]);
            curves.Add(closingC);

            return curves;
        }
    }
}
