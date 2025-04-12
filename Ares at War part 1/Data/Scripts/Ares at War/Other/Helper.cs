using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using Sandbox.Game;

namespace AresAtWar.SessionCore
{

	public static class Helper
	{

		public static Random random = new Random();

		public static Vector3D RandomPerpendicular(Vector3D referenceDir)
		{

			var refDir = Vector3D.Normalize(referenceDir);
			return Vector3D.Normalize(MyUtils.GetRandomPerpendicularVector(ref refDir));

		}



		public static Vector3D BylenCenter = new Vector3D(-2070540.14510301, -1017587.98462825, -3367463.76531797);


		public static Vector3D MilaCenter = new Vector3D(2299140.85489699, 1166634.01537175, 2935888.23468203);

		public static Vector3D NebulaCenter = new Vector3D(-17115, -4610, -8171);


		public static bool IsPointInBylenBelt(Vector3D position)
        {
			var Bylendistance = Vector3D.Distance(BylenCenter, position);
            var Bylenydistance = Math.Abs((position.Y - BylenCenter.Y));

            if (((650000 <= Bylendistance && Bylendistance <= 740000) || (780000 <= Bylendistance && Bylendistance <= 985000) || (1025000 <= Bylendistance && Bylendistance <= 1100000)) && Bylenydistance <= 6000)
			{
				return true;
			}


			return false;
        }

        public static bool IsPointInMilaBelt(Vector3D position)
        {
            var Miladistance = Vector3D.Distance(MilaCenter, position);
            var Milaydistance = Math.Abs((position.Y - MilaCenter.Y));

            if (((610000 <= Miladistance && Miladistance <= 700000) || (800000 <= Miladistance && Miladistance <= 835000) || (850000 <= Miladistance && Miladistance <= 880000)) && Milaydistance <= 5500)
            {
                return true;
            }


            return false;
        }

        public static Vector3D? GeneratePointInBylenBelt(Vector3D currentPosition, double MinDistance, double MaxDistance)
        {
            int iterationCount = 0;

            // Step 1: Start a loop to try generating a valid point
            do
            {
                // Step 2: Calculate a random distance between MinDistance and MaxDistance
                double randomDistance = random.NextDouble() * (MaxDistance - MinDistance) + MinDistance;

                // Step 3: Calculate the direction vector from currentPosition (new circle center) to MilaCenter
                Vector3D direction = BylenCenter - currentPosition;

                // Step 4: Project the direction onto the XZ-plane (keeping Y the same)
                direction.Y = 0;  // Set Y to 0 for 2D projection on the XZ-plane

                // Step 5: Normalize the direction vector to use for rotation
                Vector3D normalizedDirection = direction.Normalized();

                // Step 6: Randomly pick an angle on the circle (in radians)
                double angle = random.NextDouble() * 2 * Math.PI;  // Random angle between 0 and 2*PI

                // Step 7: Calculate a random point on the edge of the circle with the chosen radius
                double x = currentPosition.X + randomDistance * Math.Cos(angle);
                double z = currentPosition.Z + randomDistance * Math.Sin(angle);

                // Step 8: Create a new point on the circle, keeping the Y coordinate equal to MilaCenter.Y
                Vector3D newPoint = new Vector3D(x, BylenCenter.Y + random.Next(-400,400), z);

                // Step 9: Calculate the distance from MilaCenter to the new point
                double distance = (newPoint - BylenCenter).Length();

                // Step 10: Check if the new distance is within valid BylenBelt ranges based on MilaCenter
                if (((650000 <= distance && distance <= 740000) ||
                     (780000 <= distance && distance <= 985000) ||
                     (1025000 <= distance && distance <= 1100000)))
                {
                    return newPoint;  // Return the valid point if found
                }

                iterationCount++;

            } while (iterationCount < 20);  // Retry up to 20 times

            // If no valid point is found after 20 attempts, return null
            return null;
        }






        /*
        public static Vector3D? GeneratePointInBylenBelt(Vector3D currentPosition, float MinrotationDegrees, float MaxrotationDegrees)
        {

            double rotationDegrees = random.Next((int)MinrotationDegrees * 10, (int)MaxrotationDegrees * 10) / 10;
            int randomSign = random.Next(0, 2) * 2 - 1; // This will be either 1 or -1



            // Step 1: Get the direction vector from BylenCenter to currentPosition
            Vector3D direction = currentPosition - BylenCenter;

            // Step 2: Project the direction onto the XZ-plane (keeping Y the same)

            direction.Y = 0; // Set Y to 0 for 2D projection on the XZ-plane

            // Step 3: Normalize the projected direction vector for rotation
            Vector3D normalizedDirection = direction.Normalized();

            // Step 4: Rotate the vector by a given number of degrees in the XZ-plane
            double radians = randomSign * rotationDegrees * (Math.PI / 180.0); // Convert degrees to radians
            double rotatedX = normalizedDirection.X * Math.Cos(radians) - normalizedDirection.Z * Math.Sin(radians);
            double rotatedZ = normalizedDirection.X * Math.Sin(radians) + normalizedDirection.Z * Math.Cos(radians);


            // Create the rotated vector and maintain the original distance
            Vector3D rotatedDirection = new Vector3D(rotatedX, 0, rotatedZ).Normalized();

            // Step 5: Pick a new distance that satisfies the BylenBelt conditions
            int iterationCount = 0;

            double newDistance;
            do
            {
                iterationCount++;

                // Pick a random distance within maxDistance
                newDistance = random.Next(780000, 1050000);

                // Check if the distance is within the valid BylenBelt ranges
                if (((650000 <= newDistance && newDistance <= 740000) ||
                     (780000 <= newDistance && newDistance <= 985000) ||
                     (1025000 <= newDistance && newDistance <= 1100000)))
                {

                    // If a valid distance is found, create the new point
                    Vector3D newPoint = BylenCenter + (rotatedDirection * newDistance);
                    newPoint.Y = BylenCenter.Y; //+ ((random.NextDouble()-0.5) * 4000); // Restore the Y component
                    return newPoint;
                }

            } while (iterationCount < 20);

            return null;
        }
        */

        public static Vector3D? GeneratePointInMilaBelt(Vector3D currentPosition, float MinrotationDegrees, float MaxrotationDegrees)
        {

            double rotationDegrees = random.Next((int)MinrotationDegrees*10, (int)MaxrotationDegrees*10) / 10;
            // Step 1: Get the direction vector from BylenCenter to currentPosition
            Vector3D direction = currentPosition - MilaCenter;

            // Step 2: Project the direction onto the XZ-plane (keeping Y the same)
            double originalY = direction.Y; // Save the Y component
            direction.Y = 0; // Set Y to 0 for 2D projection on the XZ-plane

            // Step 3: Normalize the projected direction vector for rotation
            Vector3D normalizedDirection = direction.Normalized();

            // Step 4: Rotate the vector by a given number of degrees in the XZ-plane
            double radians = rotationDegrees * (Math.PI / 180.0); // Convert degrees to radians
            double rotatedX = normalizedDirection.X * Math.Cos(radians) - normalizedDirection.Z * Math.Sin(radians);
            double rotatedZ = normalizedDirection.X * Math.Sin(radians) + normalizedDirection.Z * Math.Cos(radians);


            // Create the rotated vector and maintain the original distance
            Vector3D rotatedDirection = new Vector3D(rotatedX, 0, rotatedZ).Normalized();

            // Step 5: Pick a new distance that satisfies the BylenBelt conditions
            int iterationCount = 0;

            double newDistance;
            do
            {
                iterationCount++;

                // Pick a random distance within maxDistance
                newDistance = random.Next(610000, 880000);




                // Check if the distance is within the valid BylenBelt ranges
                if (((610000 <= newDistance && newDistance <= 700000) ||
                     (800000 <= newDistance && newDistance <= 835000) ||
                     (850000 <= newDistance && newDistance <= 880000)))
                {
                    // If a valid distance is found, create the new point
                    Vector3D newPoint = MilaCenter + (rotatedDirection * newDistance);
                    newPoint.Y = MilaCenter.Y * (random.NextDouble() * 4000); 
                    return newPoint;
                }

            } while (iterationCount < 20);

            return null;
        }



    }

}
