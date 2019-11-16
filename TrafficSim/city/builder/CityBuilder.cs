using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficSim.city.nodes;
using TrafficSim.city.specials;
using static TrafficSim.city.builder.CityBuilderNode;

namespace TrafficSim.city.builder
{
	public class CityBuilder
	{

		public List<CityBuilderNode> nodes = new List<CityBuilderNode>();
		public List<CityBuilderPath> paths = new List<CityBuilderPath>();

		public City Build()
		{
			City city = new City();

			foreach(CityBuilderNode n in nodes)
			{
				n.Build(city);
			}

			foreach(CityBuilderPath p in paths)
			{
				if(p is CityBuilderTwoLaneTwoWayPath)
				{
					CityBuilderTwoLaneTwoWayPath path = p as CityBuilderTwoLaneTwoWayPath;
					path.Build(city);
				}

				if (p is CityBuilderParkingTwoLanePath)
				{
					CityBuilderParkingTwoLanePath path = p as CityBuilderParkingTwoLanePath;
					path.Build(city);
				}
			}

			return city;
		}

	}
}
