using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;

namespace RevitCmd
{
    public static class WallExt
    {
        
        public static Wall AddAttachment(this Wall wall, RoofBase roof ,AttachmentLocation AttachmentLocation = AttachmentLocation.Top)
        {
            wall.AddAttachment(roof.Id, AttachmentLocation);

            return wall;
        }

        public static Wall AddAttachment<T>(this Wall wall, T ceiling, AttachmentLocation AttachmentLocation)
            where T : CeilingAndFloor
        {
            wall.AddAttachment(ceiling.Id, AttachmentLocation);

            return wall;
        }
        public static Wall AddAttachment(this Wall wall, Ceiling ceiling , AttachmentLocation AttachmentLocation = AttachmentLocation.Top)
        {
            wall.AddAttachment<Ceiling>(ceiling, AttachmentLocation);

            return wall;
        }
        public static Wall AddAttachment(this Wall wall, Floor floor, AttachmentLocation AttachmentLocation = AttachmentLocation.Base)
        {
            wall.AddAttachment<Floor>(floor, AttachmentLocation);

            return wall;
        }

        public static Wall AddAttachment(this Wall wall, Wall wallToAttach, AttachmentLocation AttachmentLocation = AttachmentLocation.Top)
        {
            wall.AddAttachment(wallToAttach.Id, AttachmentLocation);

            return wall;
        }

    }
}
