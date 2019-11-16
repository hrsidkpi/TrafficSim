using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficSim.vehicles;

namespace TrafficSim.city.specials
{
	public class MergePoint : Intersection
	{

		public CityPath priority;
		public List<Vehicle> waiting;
		public List<Vehicle> inside;

		public MergePoint()
		{
			waiting = new List<Vehicle>();
			inside = new List<Vehicle>();
		}

		public override bool Contains(Vehicle v)
		{
			return waiting.Contains(v) || inside.Contains(v);
		}

		public override void Exit(Vehicle v)
		{
			inside.Remove(v);
			if (inside.Count == 0 && waiting.Count > 0)
			{
				inside.Add(waiting[0]);
				waiting.RemoveAt(0);
				
			}
		}

		public override void VehicleApproach(Vehicle v)
		{
			if (!(v is GasolineCar)) throw new NotImplementedException();
			GasolineCar c = v as GasolineCar;
			foreach (CityPath p in c.path)
			{
				if (p == priority)
				{
					inside.Add(v);
					return;
				}
			}
			if (Util.StopDistance(c.speed, GasolineCar.DEACCELERATION) + 25 < c.DistanceToNodeOnPath(priority.e)) return;
			if (inside.Count > 0)
				waiting.Add(v);
			else
				inside.Add(v);
		}
	}
}
