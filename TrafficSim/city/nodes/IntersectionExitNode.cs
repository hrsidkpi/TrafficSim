using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficSim.city.specials;

namespace TrafficSim.city.nodes
{
	public class IntersectionExitNode : CityNode
	{

		public Intersection intersection;

		public IntersectionExitNode(int x, int y, Intersection intersection) : base(x, y)
		{
			this.intersection = intersection;
		}

		public IntersectionExitNode(int x, int y, Intersection intersection, string name) : base(x, y, name)
		{
			this.intersection = intersection;
		}

	}
}
