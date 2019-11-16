using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficSim.city.builder
{
	public abstract class CityBuilderNode
	{

		public int x, y;

		public CityBuilderNode(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public abstract void Build(City c);

	}
}
