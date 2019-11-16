using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficSim.vehicles;

namespace TrafficSim.city.specials
{
	public class StopSignIntersection : Intersection
	{

		public Vehicle current;

		public List<Vehicle> waiting = new List<Vehicle>();

		public override bool Contains(Vehicle v)
		{
			return current == v || waiting.Contains(v);
		}

		public override void Exit(Vehicle v)
		{

			string report = v + " has exit an intersection. ";

			if (current == v)
			{
				waiting.RemoveAll(check => check == v);

				if (waiting.Count > 0)
				{
					current = waiting[0];
					waiting.RemoveAt(0);
					report += "Next vehicle: " + current;
				}
				else
				{
					current = null;
					report += "No other vehicles waiting.";
				}
			}
			Console.WriteLine(report);
		}

		public override void VehicleApproach(Vehicle v)
		{
			if (current == null)
			{
				Console.WriteLine(v + " has entered an intersection.");
				current = v;
			}
			else if (!waiting.Contains(v)) waiting.Add(v);
		}
	}
}
