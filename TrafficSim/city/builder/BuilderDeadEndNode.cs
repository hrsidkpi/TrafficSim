using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficSim.city.builder
{
	public class BuilderDeadEndNode : CityBuilderNode
	{

		public CityNode enter, exit;
		public CityNode p1, pMid, p2;
		public double dir;

		public BuilderDeadEndNode(int x, int y, double dir) : base(x, y)
		{
			this.dir = dir;
		}

		public override void Build(City c)
		{

			double xChange = 40 * Math.Cos(dir);
			double yChange = 40 * Math.Sin(dir);

			enter = new CityNode(x + (int)xChange, y + (int)yChange);
			exit = new CityNode(x - (int)xChange, y - (int)yChange);

			xChange = 60 * Math.Cos(dir);
			yChange = 60 * Math.Sin(dir);

			p1 = new CityNode(x - (int)xChange, y - (int)yChange);
			p2 = new CityNode(x + (int)xChange, y + (int)yChange);

			xChange = 60 * Math.Cos(dir - Math.PI / 2);
			yChange = 60 * Math.Sin(dir - Math.PI / 2);

			pMid = new CityNode(x + (int)xChange, y + (int)yChange);

			enter.Connect(exit, CityPathType.privates, 40);

			p1.Connect(pMid, CityPathType.pedestrians);
			p2.Connect(pMid, CityPathType.pedestrians);
			pMid.Connect(p1, CityPathType.pedestrians);
			pMid.Connect(p2, CityPathType.pedestrians);

			c.nodes.Add(enter);
			c.nodes.Add(exit);
			c.nodes.Add(p1);
			c.nodes.Add(p2);
			c.nodes.Add(pMid);
		}

		public void Connect(CityBuilderPath p, out CityNode start, out CityNode end, out CityNode p1, out CityNode p2)
		{
			start = exit;
			end = enter;
			p1 = this.p1;
			p2 = this.p2;
		}

	}
}
