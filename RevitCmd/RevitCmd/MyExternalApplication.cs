﻿using Autodesk.Revit.UI;

namespace RevitCmd
{
    public class MyExternalApplication : IExternalApplication
    {
        const string PanelName = "MyPanel";
        public Result OnStartup(UIControlledApplication application)
        {
            var addinPanel = application.GetRibbonPanels(Tab.AddIns)
                .FirstOrDefault(p => p.Name.Equals(PanelName));

            addinPanel ??= application.CreateRibbonPanel(PanelName);

            var pulldownBtn = addinPanel.AddItem(new PulldownButtonData("MyButton", "My Command")) as PulldownButton;
            pulldownBtn.AddPushButton<MyExternalCommand>();
            pulldownBtn.AddPushButton<CreateRooffromIfcCmd>();
            return Result.Succeeded;
        }
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }
}
