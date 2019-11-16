using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficSim.vehicles;

namespace TrafficSim.city.specials
{
	public abstract class Intersection
	{

		public abstract void Exit(Vehicle v);
		public abstract void VehicleApproach(Vehicle v); 
		public abstract bool Contains(Vehicle v);

	}
}
