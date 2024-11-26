using System;
using System.Collections.Generic;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using VRage.Game;
using VRage.Game.Components;
using System.Text;
using VRage.Utils;
using VRageMath;
using ProtoBuf;
using VRage.Game.ModAPI;

namespace AresAtWar.Managers
{
	public class GravityManager
    {
		public static List<Gravity> AllActiveGravity = new List<Gravity>();

		private static string _AllActiveGravityName = "AaW-ActiveGravity";

		public static void Setup()
        {
			//Get Existing Event Data
			string AllActiveGravityString = "";
			if (MyAPIGateway.Utilities.GetVariable<string>(_AllActiveGravityName, out AllActiveGravityString))
			{
				var GravityListSerialized = Convert.FromBase64String(AllActiveGravityString);
				AllActiveGravity = MyAPIGateway.Utilities.SerializeFromBinary<List<Gravity>>(GravityListSerialized);
			}
		}

		public static void SaveData()
		{
			//AllActiveGPS
			var AllGravityListSerialized = MyAPIGateway.Utilities.SerializeToBinary<List<Gravity>>(AllActiveGravity);
			var AllGravityListString = Convert.ToBase64String(AllGravityListSerialized);
			MyAPIGateway.Utilities.SetVariable<string>(_AllActiveGravityName, AllGravityListString);
		}



	}
	[ProtoContract]
	public class Gravity
	{
		[ProtoMember(1)] public DateTime StartingDate;
		[ProtoMember(2)] public Vector3D Position;
		[ProtoMember(3)] public IMyNaturalGravityComponent GravityProvider;
		[ProtoMember(3)] public int DurationMs;

		public Gravity()
		{
		}

		public Gravity(Vector3D position, int durationMs)
		{
			StartingDate = MyAPIGateway.Session.GameDateTime;
			DurationMs = durationMs;
			Position = position;

			Add();

		}


		public void Update()
        {

			var difference = MyAPIGateway.Session.GameDateTime - StartingDate;

			var totalmin = difference.TotalMilliseconds;
			if (totalmin > DurationMs)
			{
				RemovefromActive();
				return;
			}


		}

		public void Add()
        {
            //GravityProvider = MyAPIGateway.GravityProviderSystem.AddNaturalGravity(Position, 1, 25000, 0.1, 5);


            var Moon = new SpecialGravity(100,2000,800, new Vector3D(-2609819.61, -1056265.59, -4738760.21),41f);


            MyAPIGateway.GravityProviderSystem.AddNaturalModAPI(Position, Moon);



			//GravityManager.AllActiveGravity.Add(this);
		}

		public void RemovefromActive()
        {
			if(GravityProvider != null)
            {
				MyAPIGateway.GravityProviderSystem.RemoveNaturalGravity(GravityProvider);
            }
            else
            {
				MyVisualScriptLogicProvider.SendChatMessage("GravityProvider is null", "AaW");
				IMyNaturalGravityComponent temp;
				MyAPIGateway.GravityProviderSystem.GetStrongestNaturalGravityWell(Position, out temp);

				if(temp != null)
                {
					MyAPIGateway.GravityProviderSystem.RemoveNaturalGravity(temp);
                }

            }

			MyVisualScriptLogicProvider.SendChatMessage("GravityProvider end", "AaW");
			GravityManager.AllActiveGravity.Remove(this);


		}

    }


    internal class SpecialGravity : IMyModAPINaturalGravityImplementation
    {
        private double _baseRadius;      // Radius at the base (100)
        private double _endRadius;       // Radius at the top (2000)
        private double _height;          // Total height of the funnel
        private Vector3D _startBaseCoords; // Starting base coordinates (funnel origin at bottom)
        private float _magnitude;        // Maximum outward force

        public SpecialGravity(double baseRadius, double endRadius, double height, Vector3D startBaseCoords, float magnitude)
        {
            _baseRadius = baseRadius;
            _endRadius = endRadius;
            _height = height;
            _startBaseCoords = startBaseCoords;
            _magnitude = magnitude;
        }

        // Check if a trajectory intersects the funnel-shaped gravity field
        public double? DoesTrajectoryIntersectNaturalGravity(RayD trajectory, double raySize)
        {
            BoundingBoxD funnelBox = new BoundingBoxD(
                _startBaseCoords - new Vector3D(_endRadius, _endRadius, 0),
                _startBaseCoords + new Vector3D(_endRadius, _endRadius, _height)
            );

            return funnelBox.Intersects(trajectory);
        }

        // Returns the maximum base radius of the funnel
        public float GetGravityLimit()
        {
            return (float)_endRadius;
        }

        // Computes the gravity multiplier based on distance from the funnel axis
        public float GetGravityMultiplier(Vector3D worldPoint)
        {
            if (IsPositionInRange(worldPoint))
            {
                return (float)_magnitude;
            }

            return 0f;
        }

        // Return the axis-aligned bounding box for the funnel
        public void GetProxyAABB(out BoundingBoxD aabb)
        {
            aabb = new BoundingBoxD(
                _startBaseCoords - new Vector3D(_endRadius, _endRadius, 0),
                _startBaseCoords + new Vector3D(_endRadius, _endRadius, _height)
            );
        }

        // Computes the inverse gravity force vector at a given point
        public Vector3 GetWorldGravity(Vector3D worldPoint)
        {
            if (IsPositionInRange(worldPoint))
            {
                Vector3D direction = worldPoint - _startBaseCoords;
                direction.Normalize();

                Vector3D outwardForce = direction * _magnitude ;
                return (Vector3)outwardForce;
            }

            return Vector3.Zero;
        }

        // Computes the normalized gravity direction
        public Vector3 GetWorldGravityNormalized(Vector3D worldPoint)
        {
            if (IsPositionInRange(worldPoint))
            {
                Vector3D direction = worldPoint - _startBaseCoords;
                direction.Normalize();
                return (Vector3)direction;
            }

            return Vector3.Zero;
        }

        // Check if a position is within the funnel-shaped gravity field
        public bool IsPositionInRange(Vector3D worldPoint)
        {
            double relativeHeight = worldPoint.Z - _startBaseCoords.Z;

            if (relativeHeight < 0 || relativeHeight > _height)
                return false;

            // Compute the effective radius at the current height
            double maxRadiusAtHeight = ComputeRadiusAtHeight(relativeHeight);

            // Compute horizontal distance from axis
            double distanceFromAxis = Math.Sqrt(
                Math.Pow(worldPoint.X - _startBaseCoords.X, 2) +
                Math.Pow(worldPoint.Y - _startBaseCoords.Y, 2)
            );

            return distanceFromAxis <= maxRadiusAtHeight;
        }

        // Computes the effective radius at a given height
        private double ComputeRadiusAtHeight(double relativeHeight)
        {
            return _baseRadius + (_endRadius - _baseRadius) * (relativeHeight / _height);
        }

        // Triggered when the position changes (optional, unused in this implementation)
        public void OnPositionChanged(Vector3D position)
        {
            // No specific action needed on position change for this example
        }
    }




}


