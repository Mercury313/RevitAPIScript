using Autodesk.Revit.DB;

namespace RevitCmd
{
    public abstract class _Element : IElement
    {
        public IVector Position { get; private set; }
        public IVector Direction { get; private set; }
        public IShape Shape { get; private set; }

        public _Element(IVector position, IVector direction, IShape shape)
        {
            Position = position;
            Direction = direction;
            Shape = shape;
        }

        public abstract void Draw(Document document);
    }
}
