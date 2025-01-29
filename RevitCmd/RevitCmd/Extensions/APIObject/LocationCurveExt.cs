using Autodesk.Revit.DB;

namespace RevitCmd
{
    public static class LocationCurveExt
    {
        public static IEnumerable<T> QuJoinedAt<T>(this LocationCurve location, int end = 0)
            where T : Element
        {
            foreach (var joined in location.get_ElementsAtJoin(end))
            {
                if (joined is not T element)
                    continue;

                yield return element;
            }
        }
        public static IEnumerable<(IEnumerable<T> start, IEnumerable<T> end)> QuJoined<T>(this LocationCurve location)
            where T : Element
        {
            yield return (location.QuJoinedAt<T>(0), location.QuJoinedAt<T>(1));
        }
    }
}
