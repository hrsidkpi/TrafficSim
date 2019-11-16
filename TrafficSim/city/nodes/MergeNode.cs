using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficSim.city.specials;

namespace TrafficSim.city.nodes
{
	public class MergeNode : IntersectionExitNode
	{

		public MergeNode(int x, int y, MergePoint intersection) : base(x, y, intersection)
		{
		}

		public MergeNode(int x, int y, MergePoint intersection, string name) : base(x, y, intersection, name)
		{
		}

		public override string ToString()
		{
			return "Merge node " + id + (name != "" ? ": " + name : "");
		}

	}
}
