using Autodesk.Revit.DB;

namespace RevitCmd
{
    public static class DocumentExt
    {
        /// <summary>
        /// OfClass() applies an <see cref="Autodesk.Revit.DB.ElementClassFilter"/> to the collector.
        /// </summary>
        public static FilteredElementCollector FilterOf<T>(this Document document)
            where T : Element
        {
            //typeof(Element) would throw an exception
            if (!typeof(T).IsSubclassOf(typeof(Element)))
                return new FilteredElementCollector(document);

            var collector = new FilteredElementCollector(document)
                .OfClass(typeof(T));

            return collector;
        }
        public static IEnumerable<T> QuOfType<T>(this Document document)
            where T : Element
        {
            IEnumerable<T> collector = document
                .FilterOf<T>()
                .Cast<T>();

            return collector;
        }
    }
}
