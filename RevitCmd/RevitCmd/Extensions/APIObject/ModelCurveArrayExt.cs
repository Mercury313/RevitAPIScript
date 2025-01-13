using Autodesk.Revit.DB;

namespace RevitCmd
{
    public static class ModelCurveArrayExt
    {
        public static IEnumerable<T> QuCurves<T>(this ModelCurveArray subject)
            where T : ModelCurve
        {
            var iterator = subject.ForwardIterator();
            while (iterator.MoveNext())
            {
                if (iterator.Current is T curve)
                    yield return curve;
            }
        }
    }
}
