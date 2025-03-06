using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Hoermann.Revit
{
    public interface ICreateBeamFromIfcCmd
    {
        Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements);

    }
}


