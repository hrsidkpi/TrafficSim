using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficSim.city
{
	public class PathFinderNode
	{

		public CityPath path;
		public PathFinderNode prev, next;

		public PathFinderNode(CityPath path, PathFinderNode prev)
		{
			this.path = path;
			this.prev = prev;
			if(prev != null) prev.next = this;
		}

		public List<CityPath> GetPath()
		{
			if (prev == null || prev.path.s == null)
				return new List<CityPath>() { path };
			List<CityPath> prevPath = prev.GetPath();
			prevPath.Add(path);
			return prevPath;
		}

	}
}
