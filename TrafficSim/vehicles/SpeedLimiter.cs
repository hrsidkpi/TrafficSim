using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficSim.vehicles
{
	public class SpeedLimiter
	{

		public double limit;
		public string name;
		public List<object> additionalData;

		public SpeedLimiter(double limit, string name)
		{
			this.limit = limit;
			this.name = name;
			this.additionalData = new List<object>();
		}

		public SpeedLimiter(double limit, string name, params object[] additionalData)
		{
			this.limit = limit;
			this.name = name;
			this.additionalData = additionalData.ToList();
		}

		public override string ToString()
		{
			string s = "Speed limit " + Math.Round(limit, 2) + "px/tick because\n " + name;
			if(additionalData.Count > 0)
			{
				s += "(";
				foreach (object o in additionalData) s += o.ToString();
				s += ")";
			}
			return s;

		}

	}
}
