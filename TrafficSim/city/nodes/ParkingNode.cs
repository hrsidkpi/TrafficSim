using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficSim.people;
using TrafficSim.vehicles;

namespace TrafficSim.city.nodes
{
	public class ParkingNode : CityNode
	{

		public Vehicle vehicle;

		public ParkingNode(int x, int y) : base(x, y)
		{
			vehicle = null;
		}

		public override string ToString()
		{
			return "Parking node " + id + (name != "" ? ": " + name : "");
		}

	}
}
