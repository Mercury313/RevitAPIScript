namespace RevitCmd
{
    public class ModelDtO
    {
        public IEnumerable<ElementDtO> Elements { get; private set; }
        public ModelDtO(IEnumerable<ElementDtO> elements)
        {
            this.Elements = elements;
        }
    }
    public class ElementDtO
    {
        public Vector Position { get; set; }

        public Vector Direction { get; set; }

        public Dimension Shape { get; set; }
    }
}
