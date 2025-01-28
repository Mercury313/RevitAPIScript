using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitCmd
{
    public static class ElementExt
    {
        public static Reference NewReference(this Element element)
            => new Reference(element);

        public static IEnumerable<Reference> AsReference(this IEnumerable<Element> elements)
            => elements.Select(e => e.NewReference());
    }
}
