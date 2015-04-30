/*
 * Created by SharpDevelop.
 * User: AMoreira
 * Date: 28/03/2015
 * Time: 16:45
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Threading.Tasks;
using iDroneCup;

namespace iDroneCup_App
{
	/// <summary>
	/// Description of GpsNavigation.
	/// </summary>
	public class GpsNavigation
	{
		// Public Variables //
		public float dist2waypoint { get; set; }
		public float dist2home { get; set; }
		public float droneHeading { get; set; }
		public float droneBearing { get; set; }
		public int fps { get; set; }		
		
		// Private Variables //
		private iDroneCup.Drone mDrone = null;
		private bool runNavigation = false;
		private bool busyNavigation = false;
		private float lastTick = 0;
		
		// GPS safety range -> Modify with care!!
		private float SAFE_GPS_RANGE = 60.0f;
		
		private float DEG2RAD = (float)Math.PI / 180.0f;
		private float RAD2DEG = 180.0f / (float)Math.PI;
		
		public struct Coordinates
		{
			public double longitude;
			public double latitude;
			
			public Coordinates(double mLongitude, double mLatitude)
			{	longitude = mLongitude;
				latitude = mLatitude;
			}
		}
		
		public Coordinates currCoordinates;
		public Coordinates homeCoordinates;
		public Coordinates nextCoordinates;
		
		// GpsNavigation example code starts here ///////////////////////////////////////////////////////////
		
		public GpsNavigation( ref iDroneCup.Drone var )
		{
			mDrone = var;
		}

		public int startNavigation()
		{
			if(mDrone == null)
			{	System.Diagnostics.Debug.WriteLine("startNavigation -> mDrone null");
				return -1;
			}

			// Check: is Compass ready?
			if(!mDrone.iDroneCup_isCompassReady())
			{	System.Diagnostics.Debug.WriteLine("startNavigation -> Compass error");
				return -2;
			}

			// Check: is GPS ready?
			if(!mDrone.iDroneCup_isGpsReady())
			{	System.Diagnostics.Debug.WriteLine("startNavigation -> GPS not ready");			
				return -3;
			}
			
			// Check: home coordinates missing?
			if(homeCoordinates.latitude == 0 || homeCoordinates.longitude == 0)
			{	System.Diagnostics.Debug.WriteLine("startNavigation -> Home Coordinates not defined");			
				return -4;
			}
			
			// Check: next waypoint missing?
			if(nextCoordinates.latitude == 0 || nextCoordinates.longitude == 0)
			{	System.Diagnostics.Debug.WriteLine("startNavigation -> Next Waypoint not defined");			
				return -5;
			}
			
			// Check: is the drone on ground or landing
			if(mDrone.iDroneCup_isLanded() || mDrone.iDroneCup_isLanding())
			{	System.Diagnostics.Debug.WriteLine("startNavigation -> mDrone landed or landing");
				return -6;
			}
			
			runNavigation = true;
			return 0;
		}
		
		public int updateNavigation(int waypointRadius)
		{
			if(!runNavigation)
				return -1;
			
			// Check: is GPS ready?
			if(!mDrone.iDroneCup_isGpsReady())
			{
				this.runNavigation = false;

				mDrone.iDroneCup_Hover();
				mDrone.iDroneCup_Land();
				System.Diagnostics.Debug.WriteLine("updateNavigation -> GPS lost");
			
				return -2;
			}
			
			// Measure distance to waypoint and home
			currCoordinates = new Coordinates(mDrone.iDroneCup_Read_Longitude(),
			                                  mDrone.iDroneCup_Read_Latitude());
			dist2waypoint = calculateDistance(currCoordinates,nextCoordinates);
			dist2home = calculateDistance(currCoordinates,homeCoordinates);
			
			
			// Apply Navigation Algorithm
			if(!busyNavigation)
				Task<int>.Factory.StartNew(() => doNavigation(waypointRadius));
			
			return 0;
		}
		
		public int doNavigation(int waypointRadius)
		{
			busyNavigation = true;
			
			float maxDroneSpeedToDestination = 0.5f;
			float maxDroneSpeedRotation = 0.01f;
			float angleToWaypoint = 0;

			currCoordinates = new Coordinates(mDrone.iDroneCup_Read_Longitude(),
			                                  mDrone.iDroneCup_Read_Latitude());
			
			
			// DEGUB //
			//currCoordinates.latitude = 41.536955;
			//currCoordinates.longitude = -8.627266;
			//homeCoordinates = currCoordinates;
			// DEGUB //
			

			// Check: is out of range? in wrong direction?
			dist2waypoint = calculateDistance(currCoordinates,homeCoordinates);
			if(dist2waypoint > SAFE_GPS_RANGE)
			{
				this.runNavigation = false;
				
				mDrone.iDroneCup_Hover();
				mDrone.iDroneCup_Land();
				System.Threading.Thread.Sleep(10);
				
				System.Diagnostics.Debug.WriteLine("doNavigation -> Check: is out of range? : "+dist2waypoint.ToString());
			}
			
			// Check: is at waypoint?
			else if(isAtWaypoint(waypointRadius))
			{
				// Disable navigation OR do something else: Next waypoint? Action? Land?
				this.runNavigation = false;
				
				mDrone.iDroneCup_Hover();
				System.Threading.Thread.Sleep(10);
				
				System.Diagnostics.Debug.WriteLine("doNavigation -> Check: is at waypoint?");
			}
			
			// Action: Navigation
			else
			{
				// Read Drone Heading
				float trueNorthHeading = mDrone.iDroneCup_Read_Heading(0);
				droneHeading = trueNorthHeading;
				
				// Check: Rotation Error
				float bearing = calculateBearing(currCoordinates, nextCoordinates);
				float angleDiff = bearing - trueNorthHeading;
				angleToWaypoint = (float)(Math.Atan2(Math.Sin(angleDiff*DEG2RAD), Math.Cos(angleDiff*DEG2RAD)) * RAD2DEG);
				droneBearing = bearing;
				
				// Check: Distance Error
				float distanceToWaypoint = calculateDistance(currCoordinates,nextCoordinates);
				
				// Compute: Rotation Correction
				float yawSpeedDesired = angleToWaypoint * maxDroneSpeedRotation;
				yawSpeedDesired = KeepInRange(yawSpeedDesired, -0.25f, 0.25f);
				
				// Compute: Motion Correction
				float tmpsin = (float)Math.Sin(angleToWaypoint*DEG2RAD);
				float tmpcos = (float)Math.Cos(angleToWaypoint*DEG2RAD);
				float pitchSpeedDesired  = (maxDroneSpeedToDestination*tmpcos);
				float rollSpeedDesired = (maxDroneSpeedToDestination*tmpsin);
				
				rollSpeedDesired = KeepInRange(rollSpeedDesired, -0.25f, 0.25f);
				pitchSpeedDesired = KeepInRange(pitchSpeedDesired, -0.25f, 0.25f);
				
				// Optional: Initial yaw correction
				if(angleToWaypoint > 40.0f || angleToWaypoint < -40.0f )
				{
					// Apply only Rotation Correction
					//mDrone.iDroneCup_MoveAdvanced(0,0,0,yawSpeedDesired);
				}
				else
				{
					// Apply Global Correction
					//mDrone.iDroneCup_MoveAdvanced(pitchSpeedDesired,rollSpeedDesired,0,yawSpeedDesired);
				}
			}						
						
			// DEBUG //

			float tickDiff = System.Environment.TickCount - lastTick;
			lastTick = System.Environment.TickCount;
			fps = (int)Math.Round(1000.0f / tickDiff);
			System.Diagnostics.Debug.WriteLine("doNavigation -> Navigation done... " + "FPS: " +fps.ToString() + "angle: " +angleToWaypoint.ToString());
			
			// DEBUG //
			
			busyNavigation = false;
			
			return 0;
		}
		
		public bool isNavRunning()
		{
			return runNavigation;
		}
		
		public int stopNavigation()
		{
			runNavigation = false;
			
			if(!mDrone.iDroneCup_isLanded())
			{	mDrone.iDroneCup_MoveAdvanced(0.0f,0.0f,0.0f,0.0f);
				mDrone.iDroneCup_Hover();
			}
			return 0;
		}
		
		public int setNextWaypoint(Coordinates pto)
		{
			nextCoordinates = pto;
			return 0;
		}
		
		public Coordinates getNextWaypoint()
		{
			return nextCoordinates;
		}
		
		public float setHomeWaypoint(Coordinates pto)
		{
			homeCoordinates = pto;
			return 0;
		}
		
		public Coordinates getHomeWaypoint()
		{
			return homeCoordinates;
		}
		
		public bool isAtWaypoint(int radius)
		{
			double dist = calculateDistance(currCoordinates,nextCoordinates);
			
			if(dist < radius)
				return true;
			else
				return false;
		}
		
		public float calculateDistance(Coordinates pto1, Coordinates pto2)
		{
			double dlon, dlat, a, dist;
			
			dlon = pto1.longitude - pto2.longitude;
			dlat = pto1.latitude - pto2.latitude;
			a = Math.Pow(Math.Sin(DEG2RAD*(dlat/2)),2) + Math.Pow(Math.Sin(DEG2RAD*(dlon/2)),2) * Math.Cos(DEG2RAD*pto1.latitude) * Math.Cos(DEG2RAD*pto2.latitude);
			dist = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1-a));
			
			return (float)(6378140 * dist); /* 6378140 is the radius of the Earth in meters*/
		}
		
		public float calculateBearing(Coordinates pto1, Coordinates pto2)
		{
			double dlon = pto2.longitude - pto1.longitude;
			double bearing = Math.Atan2( Math.Sin(DEG2RAD*dlon)*Math.Cos(DEG2RAD*pto2.latitude) ,
			                            Math.Cos(DEG2RAD*pto1.latitude)*Math.Sin(DEG2RAD*pto2.latitude) - Math.Sin(DEG2RAD*pto1.latitude)*Math.Cos(DEG2RAD*pto2.latitude)*Math.Cos(DEG2RAD*dlon));
			
			bearing = bearing * RAD2DEG;
			
			if(bearing < 0)
				bearing += 360;
			
			return (float)bearing;
		}
		
		public static float KeepInRange(float aValue, float aMin, float aMax)
		{
			if (aValue < aMin) return aMin;
			if (aValue > aMax) return aMax;
			return aValue;
		}
	}
}
