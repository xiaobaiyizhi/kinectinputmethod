using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect.Toolkit.Interaction;

namespace Models
{
    public class InteractionClient:IInteractionClient
    {
        public InteractionInfo GetInteractionInfoAtLocation(int skeletonTrackingId, InteractionHandType handType, double x, double y)
        {
            return new InteractionInfo
            {
                IsPressTarget = false,
                IsGripTarget = false,
                PressAttractionPointX = 0.5,
                PressAttractionPointY = 0.5,
            };
        }
    }
}
