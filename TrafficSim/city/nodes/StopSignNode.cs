using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficSim.city.specials;

namespace TrafficSim.city.nodes
{
	public class StopSignNode : CityNode
	{

		public StopSignIntersection intersection;

		public StopSignNode(int x, int y, StopSignIntersection intersection) : base(x, y)
		{
			this.intersection = intersection;
		}

		public StopSignNode(int x, int y, StopSignIntersection intersection, string name) : base(x, y, name)
		{
			this.intersection = intersection;
		}
	}
}
