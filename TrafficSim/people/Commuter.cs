using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficSim.city;
using TrafficSim.city.nodes;
using TrafficSim.gui;
using TrafficSim.vehicles;

namespace TrafficSim.people
{
	public class Commuter : SimObject
	{

		public double x, y;
		public GasolineCar car;
		public Vehicle currentRide;

		public CityNode home, work;
		public CityNode lastVisited, goal;

		public static int COMMUTERS_COUNT = 0;

		public static double WALK_SPEED = 0.3;

		public bool parked = false;

		public int workLeaveTime;
		public int homeLeaveTime;

		public Commuter(CityNode start, CityNode home = null, CityNode work = null, GasolineCar car = null)
		{
			this.x = start.x;
			this.y = start.y;

			this.home = home;
			this.work = work;
			lastVisited = start;
			goal = start;

			this.car = car;
			if (car != null) car.owner = this;

			int possibilites = (2 * 60 + 30) / Sim.MINUTES_PER_SECOND;

			homeLeaveTime = 5 * 60 + 30 + r.Next(possibilites) * Sim.MINUTES_PER_SECOND;
			workLeaveTime = 16 * 60 + 30 + r.Next(possibilites) * Sim.MINUTES_PER_SECOND;

			Console.WriteLine("Leave to work: " + Util.GetTimeString(homeLeaveTime) + ", leave back home: " + Util.GetTimeString(workLeaveTime));
		}

		public override void Draw()
		{
			COMMUTERS_COUNT++;

			Sim.instance.DrawSprite(Sprite, new Vector2((float)x, (float)y), 0);
		}


		static Random r = new Random();
		public override void Update()
		{

			//Go to work between 6:30 and 8:00
			if ((Sim.instance.time >= homeLeaveTime && Sim.instance.time < workLeaveTime) && lastVisited == home)
			{
				goal = work;
				parked = false;
			}

			//Go home between 16:30 and 18:00
			if (Sim.instance.time >= workLeaveTime || Sim.instance.time < homeLeaveTime && lastVisited == work)
			{
				goal = home;
				parked = false;
			}

			//If needs to go somewhere
			if (lastVisited != goal)
			{
				//If not currently on a ride, find your car
				if (!parked && currentRide == null)
				{

					if(!car.parked)
					{
						//throw new Exception("Car is driving without owner, someone stole it? (bug)");
						return;
					}

					List<CityPath> pathToCar = City.GetPath(lastVisited, car.lastVisited, CityPathType.pedestrians);
					CityNode subDest = pathToCar[0].e;

					double dir = Util.GetLookatDir(x, y, subDest.x, subDest.y);
					double dist = Util.Distance(x, y, subDest.x, subDest.y);

					if (dist < 4)
					{
						if (subDest == car.lastVisited)
						{
							currentRide = car;
							car.DriveToParking(goal);
						}
						else
						{
							x = subDest.x;
							y = subDest.y;
							lastVisited = subDest;
						}
					}

					else
					{
						x +=  WALK_SPEED * Math.Cos(dir);
						y += WALK_SPEED * Math.Sin(dir);
					}
				}
				else if (!parked)
				{
					x = currentRide.x;
					y = currentRide.y;

					CityNode driveDest = car.dest;
					double dist = Util.Distance(x, y, driveDest.x, driveDest.y);

					if (dist < 4)
					{
						x = driveDest.x;
						y = driveDest.y;
						parked = true;
						currentRide = null;
						lastVisited = driveDest;
						
					}

				}
				else
				{
					List<CityPath> pathToGoal = City.GetPath(lastVisited, goal, CityPathType.pedestrians);
					CityNode subDest = pathToGoal[0].e;

					double dir = Util.GetLookatDir(x, y, subDest.x, subDest.y);
					double dist = Util.Distance(x, y, subDest.x, subDest.y);

					if (dist < 4)
					{
						x = subDest.x;
						y = subDest.y;
						lastVisited = subDest;

						if(subDest == goal)
						{
							Console.WriteLine(this + " arrived to his destination.");
						}
					}

					else
					{
						x += WALK_SPEED * Math.Cos(dir);
						y += WALK_SPEED * Math.Sin(dir);
					}
				}
			}

		}

		public override void CreateSprite()
		{
			Sprite = Assets.FullCircle(3, Color.Red);
		}

		public override bool MouseHitTest(double mx, double my)
		{
			return Util.Distance(x, y, mx, my) <= 3;
		}

		public override GuiObject SideMenu()
		{
			GuiObject dimensionCopy = Sim.instance.sideMenu;
			return new StackPanel(dimensionCopy.x, dimensionCopy.y, dimensionCopy.width, dimensionCopy.height);
		}
	}
}
