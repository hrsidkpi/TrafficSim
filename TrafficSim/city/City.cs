using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficSim.city.nodes;
using TrafficSim.gui;

namespace TrafficSim.city
{
	public class City : SimObject
	{

		public List<CityNode> nodes = new List<CityNode>();

		public override void Draw()
		{
			foreach (CityNode n in nodes) n.Draw();
		}

		public override void Update()
		{
			foreach (CityNode n in nodes) n.Update();
		}

		public static CityNode ClosestParking(CityNode goal)
		{
			List<CityNode> current = new List<CityNode>();
			List<CityNode> next = new List<CityNode>();
			current.Add(goal);

			int len = 0;
			while (len < 100)
			{
				foreach (CityNode n in current)
				{
					if (n is ParkingNode && (n as ParkingNode).vehicle == null)
					{
						return n;
					}
					foreach (CityPath p in n.paths)
					{
						next.Add(p.e);
					}
				}

				current = next;
				next = new List<CityNode>();

				len++;
			}

			throw new Exception("NO AVAILABLE PARKING");
		}

		public static List<CityPath> GetPath(CityNode start, CityNode end, CityPathType type)
		{
			List<CityNode> tested = new List<CityNode>();
			List<PathFinderNode> current = new List<PathFinderNode>();
			List<PathFinderNode> next = new List<PathFinderNode>();
			current.Add(new PathFinderNode(new CityPath(null, start, type), null));

			int len = 0;
			while (len < 100)
			{
				foreach (PathFinderNode n in current)
				{
					if (n.path.e == end)
					{
						return n.GetPath();
					}
					tested.Add(n.path.e);
					foreach (CityPath p in n.path.e.paths)
					{
						if (tested.Contains(p.e)) continue;
						if (p.type != type) continue;
						next.Add(new PathFinderNode(p, n));
					}
				}

				current = next;
				next = new List<PathFinderNode>();

				len++;
			}

			throw new Exception("NO PATH FOUND");
		}

		public CityNode NearestNode(int x, int y, CityPathType type)
		{
			double minDist = 100000;
			CityNode nearest = null;
			foreach(CityNode n in nodes)
			{
				bool typeGood = false;
				foreach (CityPath p in n.paths)
					if (p.type == type)
						typeGood = true;
				if(typeGood)
				{
					double dist = Util.Distance(x, y, n.x, n.y);
					if (dist <= minDist)
					{
						minDist = dist;
						nearest = n;
					}
				}
			}
			return nearest;
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
