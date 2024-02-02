using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Common;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI;
using ProtoBuf;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace AaW_CustomEnergyWeapons{
	
	public static partial class Utilities{
		
		public static Vector4 ConvertColor(Color color){
			
			return new Vector4(color.X / 10, color.Y / 10, color.Z / 10, 0.1f);
			
		}
		
		public static Color ConvertColorFromVector(Vector3 colorVector){
			
			Color newColor = new Color(0,0,0,255);
			
			//Red
			if(colorVector.X < 0){
				
				newColor.R = 0;
				
			}else if(colorVector.X > 255){
				
				newColor.R = 255;
				
			}else if(colorVector.X >= 0 || colorVector.X <= 255){
				
				newColor.R = (byte)Math.Floor((double)colorVector.X);
				
			}
			
			//Green
			if(colorVector.Y < 0){
				
				newColor.G = 0;
				
			}else if(colorVector.Y > 255){
				
				newColor.G = 255;
				
			}else if(colorVector.Y >= 0 || colorVector.Y <= 255){
				
				newColor.G = (byte)Math.Floor((double)colorVector.Y);
				
			}
			
			//Blue
			if(colorVector.Z < 0){
				
				newColor.B = 0;
				
			}else if(colorVector.Z > 255){
				
				newColor.B = 255;
				
			}else if(colorVector.Z >= 0 || colorVector.Z <= 255){
				
				newColor.B = (byte)Math.Floor((double)colorVector.Z);
				
			}
			
			return newColor;
			
		}
		
		public static Vector3 ConvertColorToVector(Color color){
			
			var vectorColor = Vector3.Zero;
			vectorColor.X = (float)color.R;
			vectorColor.Y = (float)color.G;
			vectorColor.Z = (float)color.B;
			return vectorColor;
			
		}
		
		public static List<Vector3D> CreateElectricityOffset(double maxRange, double minForwardStep, double maxForwardStep, double maxOffset){
			
			double currentForwardDistance = 0;
			var offsetList = new List<Vector3D>();
			
			while(currentForwardDistance < maxRange){
				
				currentForwardDistance += RandomDouble(minForwardStep, maxForwardStep);
				var lateralXDistance = RandomDouble(maxOffset * -1, maxOffset);
				var lateralYDistance = RandomDouble(maxOffset * -1, maxOffset);
				offsetList.Add(new Vector3D(lateralXDistance, lateralYDistance, currentForwardDistance * -1));
				
			}
			
			return offsetList;
			
		}
		
		public static void DisplayElectricEffect(MatrixD startMatrix, Vector3D endCoords, float beamRadius, Color color, List<Vector3D> offsetList, bool isDedicated = false){
			
			if(offsetList.Count < 2 || isDedicated == true){
				
				return;
				
			}
			
			var maxDistance = Vector3D.Distance(startMatrix.Translation, endCoords);
			
			for(int i = 0; i < offsetList.Count; i++){
				
				var fromBeam = Vector3D.Zero;
				var toBeam = Vector3D.Zero;
				
				if(i == 0){
					
					fromBeam = startMatrix.Translation;
					toBeam = Vector3D.Transform(offsetList[i], startMatrix);
					
				}else{
					
					fromBeam = Vector3D.Transform(offsetList[i - 1], startMatrix);
					toBeam = Vector3D.Transform(offsetList[i], startMatrix);
					
				}
				
				var vectorColor = color.ToVector4();
				MySimpleObjectDraw.DrawLine(fromBeam, toBeam, MyStringId.GetOrCompute("WeaponLaser"), ref vectorColor, beamRadius);
				
				if(Vector3D.Distance(startMatrix.Translation, toBeam) > maxDistance){
					
					break;
					
				}

			}
			
		}
		
		public static Vector3 GetColorFadeIncrement(Color oldColor, Color newColor, int laserTicks){
			
			Vector3 increment = new Vector3(0,0,0);
			double red = 0;
			double green = 0;
			double blue = 0;
			
			//Red
			if(newColor.R > oldColor.R){
				
				red = (double)newColor.R - (double)oldColor.R;
				red /= (double)laserTicks;
				
			}else if(newColor.R < oldColor.R){
				
				red = (double)oldColor.R - (double)newColor.R;
				red /= (double)laserTicks;
				red *= -1;
				
			}
			
			//Green
			if(newColor.G > oldColor.G){
				
				green = (double)newColor.G - (double)oldColor.G;
				green /= (double)laserTicks;
				
			}else if(newColor.G < oldColor.G){
				
				green = (double)oldColor.G - (double)newColor.G;
				green /= (double)laserTicks;
				green *= -1;
				
			}
			
			//Blue
			if(newColor.B > oldColor.B){
				
				blue = (double)newColor.B - (double)oldColor.B;
				blue /= (double)laserTicks;
				
			}else if(newColor.B < oldColor.B){
				
				blue = (double)oldColor.B - (double)newColor.B;
				blue /= (double)laserTicks;
				blue *= -1;
				
			}
			
			increment.X = (float)red;
			increment.Y = (float)green;
			increment.Z = (float)blue;
			
			return increment;
			
		}
		
	}
	
}