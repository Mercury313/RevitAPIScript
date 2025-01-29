using Autodesk.Revit.DB;

namespace RevitCmd
{
    public static class WallExt
    {

        public static Wall AddAttachment(this Wall wall, RoofBase roof, AttachmentLocation AttachmentLocation = AttachmentLocation.Top)
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
        public static Wall AddAttachment(this Wall wall, Ceiling ceiling, AttachmentLocation AttachmentLocation = AttachmentLocation.Top)
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

        public static IEnumerable<T> QuJoinedAt<T>(this Wall element, int end = 0)
            where T : Element
        {
            if (element.Location is not LocationCurve location)
                return null;

            return location.QuJoinedAt<T>(end)
                .Where(e => e.Id != element.Id);
        }
        public static IEnumerable<(IEnumerable<T> start, IEnumerable<T> end)> QuJoined<T>(this Wall element)
            where T : Element
        {
            if (element.Location is not LocationCurve location)
                return null;

            return location.QuJoined<T>()
                .Select(e => (
                e.start.Where(e => e.Id != element.Id),
                e.end.Where(e => e.Id != element.Id)));
        }

    }
}
