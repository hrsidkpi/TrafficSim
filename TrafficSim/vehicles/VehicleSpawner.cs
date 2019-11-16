using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficSim.city;
using TrafficSim.gui;

namespace TrafficSim.vehicles
{
	public class VehicleSpawner<T> : SimObject
		where T : Vehicle
	{

		public static int MAX_VEHICLE_AMOUNT = 80;

		public CityNode node;
		public List<CityNode> goals;
		public int timeToSpawn;

		Random rand = new Random();

		public T lastSpawn;

		//Leave goal null to get random goal
		public VehicleSpawner(CityNode node, int timeToSpawn, List<CityNode> goals)
		{
			this.node = node;
			this.timeToSpawn = timeToSpawn;

			this.goals = goals;
		}

		public override void Draw()
		{
		}


		int time = 0;
		public override void Update()
		{
			time++;

			if (time >= timeToSpawn && GasolineCar.CARS_COUNT < MAX_VEHICLE_AMOUNT)
			{
				time = 0;

				CityNode goal = goals[rand.Next(goals.Count)];

				if (lastSpawn == null || Util.Distance(lastSpawn.x, lastSpawn.y, node.x, node.y) >= Util.ToPixels(10))
				{
					T t = (T)Activator.CreateInstance(typeof(T), node, goal);
					Console.WriteLine("Spawning " + t);
					Sim.instance.vehicles.Add(t);

					lastSpawn = t;
				}

			}
		}

		public override void CreateSprite()
		{
			Sprite = null;
		}

		public override bool MouseHitTest(double mx, double my)
		{
			throw new NotImplementedException();
		}

		public override GuiObject SideMenu()
		{
			throw new NotImplementedException();
		}
	}
}
