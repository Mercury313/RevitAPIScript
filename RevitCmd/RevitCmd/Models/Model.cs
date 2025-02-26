

using Autodesk.Revit.DB;

namespace RevitCmd
{
    public class Model : IModel
    {
        public IEnumerable<IElement> Elements { get; private set; }

        public Model(IEnumerable<IElement> elements)
        {
            this.Elements = elements;
        }
        public void Draw(Document document)
        {
            foreach (var element in Elements)
            {
                element.Draw(document);
            }
        }
    }
}
