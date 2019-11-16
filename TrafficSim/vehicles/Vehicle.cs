using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficSim.city;
using TrafficSim.people;

namespace TrafficSim.vehicles
{
	public abstract class Vehicle : SimObject
	{

		public static int nextId = 0;

		public int capacity;
		public List<Commuter> commuters;

		public double x, y;
		public int id;


		public Vehicle()
		{
			id = nextId;
			nextId++;
		}

		public override string ToString()
		{
			return "Vehicle number " + id;
		}

	}
}
