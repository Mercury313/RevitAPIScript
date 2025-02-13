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
            => document.FilterOf<T>()
                .Cast<T>();

        public static Transaction NewTransaction(this Document document, string name)
            => new Transaction(document, name ?? "Transaction");
        public static void Transaction(this Document document, Action action, string? txnName = null)
        {
            using (Transaction txn = document.NewTransaction(txnName ?? action.Method.Name))
            {
                txn.Start();
                try
                {
                    action?.Invoke();
                    txn.Commit();
                }
                catch
                {
                    if (txn.HasStarted())
                    {
                        txn.RollBack();
                    }

                    throw;
                }
            }
        }

        public static void Transaction(this Document document, Action<Document> action, string? txnName = null)
        {
            using (var txn = document.NewTransaction(txnName ?? action.Method.Name))
            {
                txn.Start();
                try
                {
                    action?.Invoke(document);

                    txn.Commit();

                }
                catch
                {
                    if (txn.HasStarted())
                    {
                        txn.RollBack();
                    }

                    throw;
                }
            }
        }

        public static Autodesk.Revit.DB.Dimension CreateDimension(this Document document, Reference ref1, Reference ref2, Line? line, double offset)
        {
            ReferenceArray referenceArray = new ReferenceArray();

            referenceArray.Append(ref1);

            referenceArray.Append(ref2);


            var offSetLine = line.CreateOffset(UnitUtils.ConvertToInternalUnits(offset, UnitTypeId.Millimeters), XYZ.BasisZ) as Line;

            // Create a dimension between start and end points of the wall
            return document.Create.NewDimension(
                   document.ActiveView,
                   offSetLine,
                   referenceArray);

        }

    }
}
