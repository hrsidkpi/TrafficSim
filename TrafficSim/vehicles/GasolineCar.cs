using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficSim.city;
using TrafficSim.city.nodes;
using TrafficSim.city.specials;
using TrafficSim.gui;
using TrafficSim.people;

namespace TrafficSim.vehicles
{
	public class GasolineCar : Vehicle
	{

		//1 pixel = 0.25 meter
		//1 meter = 4 pixel

		public static double MAX_SPEED = Util.ToPixels(190 * 1000) / Util.ToTicks(3600);
		public static double ACCELERATION = Util.ToPixels(0.5 * 9.8) / Util.ToTicks(1) / Util.ToTicks(1);
		public static double DEACCELERATION = Util.ToPixels(0.8 * 9.8) / Util.ToTicks(1) / Util.ToTicks(1);

		public static double MAX_TURN_ACCELERATION = Util.ToPixels(28.0) / Util.ToTicks(1) / Util.ToTicks(1);
		public static double TURN_RADIUS = MAX_SPEED * MAX_SPEED / MAX_TURN_ACCELERATION;

		public static double CAR_WIDTH = Util.ToPixels(2);
		public static double CAR_LENGTH = Util.ToPixels(4.5);
		public static double STOP_BEHIND_DISTANCE = CAR_LENGTH / 2 + Util.ToPixels(3);

		public static double DRIVER_VIEW_DISTANCE = Util.ToPixels(300);

		public CityNode lastVisited;


		public string LastVisitedString
		{
			get
			{
				return lastVisited.ToString();
			}
		}

		public CityNode dest; //The car destination. This is a parking node.
		public List<CityPath> path;
		public int currentDestId;
		public CityNode goal; //The commuter's destination. This can be any node.

		public List<SpeedLimiter> limits = new List<SpeedLimiter>();
		public double MinLimit
		{
			get
			{
				return limits.Min(l => l.limit);
			}
		}
		public string LimitsString
		{
			get
			{
				string s = "";
				foreach (SpeedLimiter l in limits)
				{
					s += l.limit + "px/tick because \n" + l.name + ".\n";
					foreach (object o in l.additionalData) s += o.ToString();
				}
				return s;
			}
		}

		//Used for forcing a full stop at stop signs.
		public bool stopped = false;

		public bool parked = true;

		public string ParkedString
		{
			get
			{
				return parked ? "parked." : "driving.";
			}
		}

		public CityNode source;

		public Commuter owner;

		public CityPath CurrentCityPath
		{
			get
			{
				if (currentDestId >= path.Count) return path.Last();
				return path[currentDestId];
			}
		}

		public CityPath NextCityPath
		{
			get
			{
				if (currentDestId + 1 >= path.Count) return path.Last();
				return path[currentDestId + 1];
			}
		}


		public double dir;
		public double speed;

		public double Speed
		{
			get { return speed; }
		}

		public double speedGoal;

		public string Name
		{
			get
			{
				return "Car number " + id;
			}
		}

		public GasolineCar(CityNode start)
		{
			CARS_COUNT++;

			this.x = start.x;
			this.y = start.y;

			this.capacity = 5;

			this.lastVisited = start;

			this.source = start;

			this.dir = Util.GetLookatDir(x, y, start.paths[0].e.x, start.paths[0].e.y);
			limits.Add(new SpeedLimiter(MAX_SPEED, "max speed"));

			parked = true;
			(lastVisited as ParkingNode).vehicle = this;
		}

		public void DriveTo(CityNode dest)
		{

			Console.WriteLine(this + " is driving to " + dest + "...");
			Console.WriteLine("its owner's goal is " + owner.goal);

			if (owner.currentRide != this)
			{
				throw new Exception("Car trying to drive without owner? (bug)");
			}

			(lastVisited as ParkingNode).vehicle = null;
			this.dest = dest;
			path = City.GetPath(lastVisited, dest, CityPathType.privates);
			currentDestId = 0;
			CurrentCityPath.vehicles.Insert(0, this);
			parked = false;
		}

		public void DriveToParking(CityNode goal)
		{
			this.goal = goal;
			dest = City.ClosestParking(goal);
			DriveTo(dest);
		}

		public void UpdateParkingDest(CityNode goal)
		{
			dest = City.ClosestParking(goal);
			UpdatePath();
		}

		public void UpdatePath()
		{
			//Can't update path when not on a node
			if (lastVisited.x != x || lastVisited.y != y)
				throw new Exception("Trying to update path when not on a node.");
			CurrentCityPath.vehicles.Remove(this);
			path = City.GetPath(lastVisited, dest, CityPathType.privates);
			currentDestId = 0;
			CurrentCityPath.vehicles.Insert(0, this);
		}

		public static int CARS_COUNT = 0;

		public override void Draw()
		{
			Sim.instance.DrawSprite(Sprite, new Vector2((float)x, (float)y), dir);
			Sim.instance.DrawString("" + id, new Vector2((float)x, (float)y), Color.Red);
		}

		public Vehicle VehicleInfront()
		{
			if (id == 5)
				Console.Write("");
			if (CurrentCityPath.NextVehcile(this) != null)
				return CurrentCityPath.NextVehcile(this);
			for (int i = currentDestId + 1; i < path.Count; i++)
			{
				if (path[i].vehicles.Count > 0)
					return path[i].vehicles[0];
			}
			return null;
		}

		public override void Update()
		{

			if (path != null)
			{
				CheckFarObstacles();
				CheckaArrivalAndParking();
				CheckCloseVehicle();
				HandleMerging();
				HandleStopSign();
				HandleTurnSpeed();
				ExecuteMovement();
				HandleNodeArrival();
			}
		}

		/// <summary>
		/// Scan if a merge point is ahead of the car, and if so, handle merging.
		/// </summary>
		private void HandleMerging()
		{

			double totalDist = Util.Distance(x, y, CurrentCityPath.e.x, CurrentCityPath.e.y);
			foreach (CityPath p in path)
			{
				if(p != CurrentCityPath) totalDist += Util.Distance(p.s.x, p.s.y, p.e.x, p.e.y);

				if (p.e is MergeNode)
				{
					if (2 * Util.StopDistance(speed, DEACCELERATION) + 25 >= totalDist)
					{
						MergeNode m = p.e as MergeNode;
						if (!m.intersection.Contains(this))
							m.intersection.VehicleApproach(this);
						MergePoint point = m.intersection as MergePoint;
						if (point.inside.Contains(this))
						{
							limits.RemoveAll(l => l.name == "merge");
							return;
						}
						else if(Util.StopDistance(speed, DEACCELERATION) + 25 >= totalDist)
						{
							limits.RemoveAll(l => l.name == "merge");
							limits.Add(new SpeedLimiter(0, "merge"));
						}
					}
				}
			}
		}

		/// <summary>
		/// Scan if the final destination is within stopping distance. If it is, stop and park there.
		/// </summary>
		public void CheckaArrivalAndParking()
		{
			if (Util.Distance(x, y, dest.x, dest.y) <= Util.StopDistance(speed, DEACCELERATION))
			{
				limits.RemoveAll(l => l.name == "arrival");
				limits.Add(new SpeedLimiter(0, "arrival"));
			}
			else
			{
				limits.RemoveAll(l => l.name == "arrival");
			}

			//Snap to the parking node, and enter "parking" state.
			if (limits.Where(l => l.name == "arrival").Count() > 0 && speed <= 0.2)
			{
				x = dest.x;
				y = dest.y;
				speed = 0;
				parked = true;
				limits.RemoveAll(l => l.name == "arrival");
				lastVisited = dest;
				(lastVisited as ParkingNode).vehicle = this;
				Console.WriteLine(this + " has arrived and parked at its destination");
			}

			if (parked)
			{
				limits.RemoveAll(l => l.name == "parked");
				limits.Add(new SpeedLimiter(0, "parked"));
			}
			else
			{
				limits.RemoveAll(l => l.name == "parked");

			}
		}

		/// <summary>
		/// Scan for upcoming stop signs, stop there, and then wait for the intersection to clear before proceeding.
		/// </summary>
		public void HandleStopSign()
		{

			foreach(CityPath p in PathsInView())
			{
				CityNode node = p.e;
				if(node is StopSignNode)
				{
					StopSignNode n = node as StopSignNode;
					double dist = DistanceToNodeOnPath(n);

					if(!stopped)
					{
						if (dist <= Util.StopDistance(speed, DEACCELERATION) + 25)
						{
							limits.RemoveAll(l => l.name == "stop sign");
							limits.Add(new SpeedLimiter(0, "stop sign"));
						}
						if (dist < 26)
						{
							if (speed <= 0.1)
							{
								stopped = true;
								n.intersection.VehicleApproach(this);
							}
						}
					}
					else
					{
						if (n.intersection.current == this)
						{
							limits.RemoveAll(l => l.name == "stop sign");
						}
						else
						{
							limits.RemoveAll(l => l.name == "stop sign");
							limits.Add(new SpeedLimiter(0, "stop sign"));
						}
					}
				}
			}
		}

		/// <summary>
		/// Scan for current or upcoming sharp turns and slow down to pass them smoothly.
		/// </summary>
		public void HandleTurnSpeed()
		{
			double totalDist = Util.Distance(x, y, CurrentCityPath.e.x, CurrentCityPath.e.y);

			limits.RemoveAll(l => l.name == "turn");
			if(CurrentCityPath.radius > 0)
			{
				limits.Add(new SpeedLimiter(CurrentCityPath.radius * MAX_TURN_ACCELERATION, "turn"));
			}
			foreach (CityPath p in path)
			{
				if (p == CurrentCityPath) continue;

				if(p.radius > 0)
				{
					double requiredSpeed = MAX_TURN_ACCELERATION * p.radius;
					if(Util.SlowDistance(speed, DEACCELERATION, requiredSpeed) >= totalDist)
					{
						limits.Add(new SpeedLimiter(requiredSpeed, "turn"));
					}
				}

				totalDist += Util.Distance(p.s.x, p.s.y, p.e.x, p.e.y);
			}


			//Slow down before turns
			if (Util.Distance(x, y, CurrentCityPath.e.x, CurrentCityPath.e.y) < 140 && (NextCityPath.radius > 0 || CurrentCityPath.radius > 0))
			{
				double minRadius = 0;
				if (NextCityPath.radius > 0)
				{
					if (CurrentCityPath.radius > 0) minRadius = Math.Min(NextCityPath.radius, CurrentCityPath.radius);
					else minRadius = NextCityPath.radius;
				}
				else
				{
					minRadius = CurrentCityPath.radius;
				}
				if (CurrentCityPath.e != dest)
				{
					limits.RemoveAll(l => l.name == "turn");
					limits.Add(new SpeedLimiter(MAX_TURN_ACCELERATION * minRadius, "turn", minRadius));
				}
			}
			else if (NextCityPath.radius == 0 && CurrentCityPath.radius == 0)
				limits.RemoveAll(l => l.name == "turn");

		}

		/// <summary>
		/// Accelerate or brake according to the minimum speed limiter on this car, and steer according to the current path.
		/// </summary>
		public void ExecuteMovement()
		{

			//Find the minimum speed limiter on this car.
			speedGoal = limits.Min(l => l.limit);

			//Brake or accelerate according to the speed goal.
			double change = Math.Min(speedGoal, MAX_SPEED) - speed;
			if (change < 0) speed += Math.Max(change, -DEACCELERATION);
			if (change > 0) speed += Math.Min(change, ACCELERATION);

			//Steer according to the Lookat direction to the current destination node.
			dir = dir % (Math.PI * 2);
			if (dir < 0) dir += Math.PI * 2;

			double targetDir = Util.GetLookatDir(x, y, CurrentCityPath.e.x, CurrentCityPath.e.y) % (Math.PI * 2);
			if (targetDir < 0) targetDir += Math.PI * 2;

			double dirDiff = Util.DirectionDiff(dir, targetDir);

			if (dirDiff != 0)
			{
				double wMax = CurrentCityPath.radius == 0 ? MAX_TURN_ACCELERATION / speed : speed / CurrentCityPath.radius;

				double wActual = Math.Min(Math.Abs(dirDiff), wMax);

				dir += wActual * (double)(dirDiff > 0.0 ? 1.0 : -1.0);
			}


			//Apply the current speed of the car to its position.
			x += speed * Math.Cos(dir);
			y += speed * Math.Sin(dir);

		}

		/// <summary>
		/// Check if the car is very close to its current node destination (it will never be exactly on it due to nominal time calculations).
		/// If it is, snap to the node and start moving towards the next one.
		/// </summary>
		public void HandleNodeArrival()
		{

			//Find the width distance and length distance of the car from the destination node
			double angle = Util.GetLookatDir(x, y, CurrentCityPath.e.x, CurrentCityPath.e.y) % 360;
			if (angle < 0) angle += Math.PI * 2;
			double dirPositive = dir % 360;
			if (dirPositive < 0) dirPositive += Math.PI * 2;

			double d = angle - MathHelper.ToRadians(270) - dirPositive;
			double distToTarget = Util.Distance(x, y, CurrentCityPath.e.x, CurrentCityPath.e.y);

			//Check if the distances are close enough
			if (distToTarget * Math.Cos(d) < 8 && distToTarget * Math.Sin(d) < 2)
			{

				lastVisited = CurrentCityPath.e;

				//If this car just exit an intersection, notify the intersection that it is now free.
				if (CurrentCityPath.e is IntersectionExitNode)
				{
					(CurrentCityPath.e as IntersectionExitNode).intersection.Exit(this);
				}

				//Snap to the node
				x = CurrentCityPath.e.x;
				y = CurrentCityPath.e.y;

				//If this is not the final destination of the trip, recalculate route to make sure no changes were made
				if (CurrentCityPath.e != dest)
				{
					UpdateParkingDest(goal);
				}
			}
		}

		///<summary>
		///Find the vehicle that is in front of this car, and maintain distance from him.
		///</summary>
		public void CheckCloseVehicle()
		{

			Vehicle before = VehicleInfront();

			//When approaching the car in front, slow down.
			if (before != null && Util.Distance(x, y, before.x, before.y) <= Util.StopDistance(speed, DEACCELERATION) + STOP_BEHIND_DISTANCE)
			{

				//If the car before is parked, this car needs to reverse and find another route.
				if (before is GasolineCar && (before as GasolineCar).parked && CurrentCityPath.vehicles.Contains(before))
				{
					x = lastVisited.x;
					y = lastVisited.y;
					CurrentCityPath.vehicles.Remove(this);
					UpdateParkingDest(goal);
				}

				limits.RemoveAll(l => l.name == "queue");
				limits.Add(new SpeedLimiter(0, "queue", before));
			}
			else
				limits.RemoveAll(l => l.name == "queue");
		}

		///<summary>
		///Scan for speed limiters ahead of the car.
		///</summary>
		public void CheckFarObstacles()
		{

			limits.RemoveAll(l => l.name == "obstacle");

			double totalDist = Util.Distance(x, y, CurrentCityPath.e.x, CurrentCityPath.e.y);
			foreach (CityPath p in path)
			{
				if (p == CurrentCityPath) continue;


				totalDist += Util.Distance(p.s.x, p.s.y, p.e.x, p.e.y);
				double expectedSpeed = p.ExpectedSpeed(this);
				if (Util.SlowDistance(speed, DEACCELERATION, expectedSpeed) + 25 >= totalDist)
				{
					if (expectedSpeed != MAX_SPEED)
					{
						if (limits.Where(l => l.name == "obstacle" && l.limit < expectedSpeed).Count() > 0)
							continue;
						limits.RemoveAll(l => l.name == "obstacle");

						limits.Add(new SpeedLimiter(expectedSpeed, "obstacle"));
					}
				}
			}
		}

		private List<CityPath> PathsInView()
		{
			double totalDist = Util.Distance(x, y, CurrentCityPath.e.x, CurrentCityPath.e.y);
			List<CityPath> res = new List<CityPath>() { CurrentCityPath };
			foreach(CityPath p in path)
			{
				if (p == CurrentCityPath) continue;
				totalDist += Util.Distance(p.s.x, p.s.y, p.e.x, p.e.y);
				res.Add(p);
				if (totalDist > DRIVER_VIEW_DISTANCE) return res;
			}
			return res;
		}

		public double DistanceToNodeOnPath(CityNode n)
		{
			double totalDist = Util.Distance(x, y, CurrentCityPath.e.x, CurrentCityPath.e.y);
			foreach(CityPath p in path)
			{
				if(p != CurrentCityPath) totalDist += Util.Distance(p.s.x, p.s.y, p.e.x, p.e.y);
				if (p.e == n) return totalDist;
			}
			return totalDist;
		}

		public override void CreateSprite()
		{
			Sprite = Assets.FullRectangle((int)CAR_LENGTH, (int)CAR_WIDTH, Color.Black);
		}

		public override bool MouseHitTest(double mx, double my)
		{
			mx -= x;
			my -= y;

			double nx = mx * Math.Cos(dir) - my * Math.Sin(dir);
			double ny = mx * Math.Sin(dir) + my * Math.Cos(dir);

			nx += x;
			ny += y;

			return (Math.Abs(nx - x) <= Sprite.Width / 2) && (Math.Abs(ny - y) <= Sprite.Height / 2);

		}

		public override GuiObject SideMenu()
		{
			GuiObject dimensionCopy = Sim.instance.sideMenu;
			StackPanel p = new StackPanel(dimensionCopy.x, dimensionCopy.y, dimensionCopy.width, dimensionCopy.height);
			p.AddChild(new GuiText(0, 0, 100, 50, Assets.Font24, "&l", new ObjectLinker(this, "Name")));

			p.AddChild(new GuiText(0, 50, 100, 50, Assets.Font18, "&l", new ObjectLinker(this, "ParkedString")));
			p.AddChild(new GuiText(0, 100, 100, 50, Assets.Font18, "Speed: &l", new ObjectLinker(this, "Speed")));
			p.AddChild(new GuiText(0, 150, 100, 50, Assets.Font18, "Speed goal: &l", new ObjectLinker(this, "MinLimit")));
			p.AddChild(new GuiText(0, 200, 100, 50, Assets.Font18, "Last visited: &l", new ObjectLinker(this, "LastVisitedString")));
			p.AddChild(new GuiText(0, 250, 100, 50, Assets.Font18, "&l", new ObjectLinker(this, "LimitsString")));


			return p;
		}
	}
}
