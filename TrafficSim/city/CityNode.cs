using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficSim.gui;

namespace TrafficSim.city
{
	public class CityNode : SimObject
	{

		public int x, y;
		public string name = "";
		public int id;

		public static int NODE_COUNT = 0;

		//All connected nodes. Vehicles will drive between nodes on a circular curve.
		public List<CityPath> paths = new List<CityPath>();

		public CityNode(int x, int y)
		{
			this.x = x;
			this.y = y;
			this.id = NODE_COUNT;
			NODE_COUNT++;
		}

		public CityNode(int x, int y, string name)
		{
			this.x = x;
			this.y = y;
			this.name = name;

			this.id = NODE_COUNT;
			NODE_COUNT++;
		}

		public override void Draw()
		{
			Sim.instance.DrawSprite(Sprite, new Vector2(x, y), 0);
			Sim.instance.DrawString(""+id, new Vector2(x, y), Assets.Font12, Color.Black);

			foreach (CityPath p in paths) p.Draw();

		}

		public override void Update()
		{
		}

		public override string ToString()
		{
			return "City node " + id + (name != "" ? ": " + name : "");
		}

		//Connect this node to next with the specified transportation type, and return the path created.
		public CityPath Connect(CityNode next, CityPathType type)
		{
			if (next == null) throw new Exception("Trying to connect a node to null");
			CityPath p = new CityPath(this, next, type);
			paths.Add(p);
			return p;
		}

		//Connect this node to next with the specified transportation type and turn radius, and return the path created.
		public CityPath Connect(CityNode next, CityPathType type, double radius)
		{
			if (next == null) throw new Exception("Trying to connect a node to null");
			CityPath p = new CityPath(this, next, type, radius);
			paths.Add(p);
			return p;
		}

		public override void CreateSprite()
		{
			Sprite = Assets.FullCircle(4, Color.Red);
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
