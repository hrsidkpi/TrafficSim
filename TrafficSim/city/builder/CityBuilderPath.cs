using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficSim.city.builder
{
	public abstract class CityBuilderPath
	{

		public CityBuilderNode s, e;

		public CityBuilderPath(CityBuilderNode s, CityBuilderNode e)
		{
			this.s = s;
			this.e = e;
		}

		public abstract void Build(City c);

	}
}
