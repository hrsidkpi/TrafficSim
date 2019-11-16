using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficSim.city.nodes;
using TrafficSim.gui;
using TrafficSim.vehicles;

namespace TrafficSim.city
{

	public enum CityPathType
	{
		pedestrians, privates, busses, bike
	}

	public class CityPath : SimObject
	{

		public CityNode s, e;
		public List<Vehicle> vehicles;
		public double radius;

		public CityPathType type;

		public static int PATHS_COUNT;

		public CityPath(CityNode s, CityNode e, CityPathType type)
		{
			this.s = s;
			this.e = e;

			this.type = type;

			radius = 0;
			vehicles = new List<Vehicle>();
			PATHS_COUNT++;
		}

		public CityPath(CityNode s, CityNode e, CityPathType type, double radius)
		{
			this.s = s;
			this.e = e;
			this.radius = radius;

			this.type = type;

			vehicles = new List<Vehicle>();
			PATHS_COUNT++;
		}

		public override void Draw()
		{
			double w = e.x - s.x;
			double h = e.y - s.y;
			int len = (int)Math.Sqrt(w * w + h * h);
			float dir = (float)Math.Atan2(h, w);

			float xCenter = (float)s.x + (float)(len * Math.Cos(dir) * 0.5);
			float yCenter = (float)s.y + (float)(len * Math.Sin(dir) * 0.5);


			Sim.instance.DrawSprite(Sprite, new Vector2(xCenter, yCenter), dir);
		}

		public override void Update()
		{
		}

		//Get the vehcile in front of the current one in the lane
		public Vehicle NextVehcile(Vehicle current)
		{
			for(int i = 0; i < vehicles.Count-1; i++)
			{
				if (current == vehicles[i]) return vehicles[i + 1];
			}
			return null;
		}

		public override void CreateSprite()
		{
			double w = e.x - s.x;
			double h = e.y - s.y;
			int len = (int)Math.Sqrt(w * w + h * h);
			float dir = (float)Math.Atan2(h, w);

			float xCenter = (float)s.x + (float)(len * Math.Cos(dir) * 0.5);
			float yCenter = (float)s.y + (float)(len * Math.Sin(dir) * 0.5);

			Color color = Color.Blue;
			switch(type)
			{
				case CityPathType.pedestrians:
					color = Color.Green;
					break;
			}

			Sprite = Assets.FullRectangle(len, 1, color);
		}

		public override bool MouseHitTest(double mx, double my)
		{
			throw new NotImplementedException();
		}

		public override GuiObject SideMenu()
		{
			throw new NotImplementedException();
		}

		///<summary>
		///Returns the expected maximum speed Vehicle v should have upon arrival to this path's end node.
		///</summary>
		public double ExpectedSpeed(Vehicle v)
		{
			return GasolineCar.MAX_SPEED;
		}

	}
}
