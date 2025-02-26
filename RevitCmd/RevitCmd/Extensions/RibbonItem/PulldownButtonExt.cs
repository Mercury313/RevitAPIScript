using Autodesk.Revit.UI;

namespace RevitCmd
{
    public static class PulldownButtonExt
    {
        public static PushButton AddPushButton<T>(this PulldownButton pulldown, string txt = null)
        {
            Type type = typeof(T);
            txt ??= type.Name;

            var data = new PushButtonData(type.Name, txt, type.Assembly.Location, type.FullName);
            return pulldown.AddPushButton(data);
        }
    }
}
