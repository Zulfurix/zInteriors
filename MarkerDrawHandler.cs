using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace zInteriors_Client
{
    public class MarkerDrawHandler : BaseScript
    {
        private readonly Vector3 MARKER_VECTOR3_DIMENSIONS = new Vector3(1, 1, 1);
        private readonly System.Drawing.Color MARKER_COLOR = System.Drawing.Color.FromArgb(175, 0, 155, 225);
        private readonly float MARKER_DRAW_DISTANCE = 30f;

        public MarkerDrawHandler()
        {
            EventHandlers["onClientResourceStart"] += new Action<string>(OnClientResourceStart);
            EventHandlers["zInteriors:drawFollowingMarker"] += new Action(DrawFollowingMarker);
            EventHandlers["zInteriors:undrawFollowingMarker"] += new Action(UndrawFollowingMarker);
            EventHandlers["zInteriors:drawTemporaryMarkers"] += new Action(DrawTemporaryMarkers);
            EventHandlers["zInteriors:undrawTemporaryMarkers"] += new Action(UndrawTemporaryMarkers);
            EventHandlers["zInteriors:drawInteriorMarkers"] += new Action(DrawInteriorMarkers);
        }

        private void OnClientResourceStart(string resourceName)
        {
            if (GetCurrentResourceName() != resourceName) return;
        }

        private async Task FollowingMarkerTick()
        {
            Vector3 renderPosition = Game.PlayerPed.Position + new Vector3(0, 0, -1);

            World.DrawMarker(MarkerType.VerticalCylinder, renderPosition, Vector3.Zero, Vector3.Zero, MARKER_VECTOR3_DIMENSIONS, MARKER_COLOR);
        }

        private async Task TemporaryMarkersTick()
        {
            if (InteriorHandler.TempInterior.Entrance != null)
                World.DrawMarker(MarkerType.VerticalCylinder, InteriorHandler.TempInterior.Entrance, Vector3.Zero, Vector3.Zero, MARKER_VECTOR3_DIMENSIONS, MARKER_COLOR);

            if (InteriorHandler.TempInterior.Exit != null)
                World.DrawMarker(MarkerType.VerticalCylinder, InteriorHandler.TempInterior.Exit, Vector3.Zero, Vector3.Zero, MARKER_VECTOR3_DIMENSIONS, MARKER_COLOR);
        }

        private async Task InteriorMarkersTick()
        {
            foreach (dynamic interior in InteriorHandler.Interiors)
            {
                if (Game.PlayerPed.IsInRangeOf(interior.Entrance, MARKER_DRAW_DISTANCE))
                    World.DrawMarker(MarkerType.VerticalCylinder, interior.Entrance, Vector3.Zero, Vector3.Zero, MARKER_VECTOR3_DIMENSIONS, MARKER_COLOR);
                if (Game.PlayerPed.IsInRangeOf(interior.Exit, MARKER_DRAW_DISTANCE))
                    World.DrawMarker(MarkerType.VerticalCylinder, interior.Exit, Vector3.Zero, Vector3.Zero, MARKER_VECTOR3_DIMENSIONS, MARKER_COLOR);
            }
        }

        private void DrawFollowingMarker()
        {
            Tick += FollowingMarkerTick;
        }


        private void UndrawFollowingMarker()
        {
            Tick -= FollowingMarkerTick;
        }

        private void DrawTemporaryMarkers()
        {
            Tick += TemporaryMarkersTick;
        }

        private void UndrawTemporaryMarkers()
        {
            Tick -= TemporaryMarkersTick;
        }

        private void DrawInteriorMarkers()
        {
            Tick += InteriorMarkersTick;
        }
    }
}
