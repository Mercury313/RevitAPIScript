using Autodesk.Revit.DB;

namespace RevitCmd
{
    public static class FootprintRoofExt
    {
        /// <param name="slope">set the angle by its slope</param>
        public static FootPrintRoof SetSlope(this FootPrintRoof roof, ModelCurve line, double slope)
        {
            if (!roof.get_DefinesSlope(line))
                roof.set_DefinesSlope(line, true);

            roof.set_SlopeAngle(line, slope);

            return roof;
        }
    }
}
