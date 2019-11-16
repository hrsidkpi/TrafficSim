using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficSim.city.nodes;
using TrafficSim.city.specials;

namespace TrafficSim.city.builder
{
	class BuilderTrafficCircleNode : CityBuilderNode
	{

		public CityBuilderPath left, right, top, bottom;

		public CityNode wLeft, wRight, wTop, wBottom;
		public CityNode eLeft, eRight, eTop, eBottom;
		public CityNode pTopLeft, pTopRight, pBottomLeft, pBottomRight;

		public BuilderTrafficCircleNode(int x, int y) : base(x, y)
		{
		}

		public override void Build(City c)
		{

			MergePoint m1 = new MergePoint();
			CityNode nw1 = wTop = new MergeNode(x - 40, y - 80, m1, "nw1");
			CityNode ne1 = eTop = new CityNode(x + 40, y - 80, "ne1");
			CityPath p1 = ne1.Connect(nw1, CityPathType.privates);
			m1.priority = p1;

			MergePoint m2 = new MergePoint();
			CityNode nw2 = wLeft = new MergeNode(x - 80, y + 40, m2, "nw2");
			CityNode ne2 = eLeft = new CityNode(x - 80, y - 40, "ne2");
			CityPath p2 = ne2.Connect(nw2, CityPathType.privates);
			m2.priority = p2;

			MergePoint m3 = new MergePoint();
			CityNode nw3 = wBottom = new MergeNode(x + 40, y + 80, m3, "nw3");
			CityNode ne3 = eBottom = new CityNode(x - 40, y + 80, "ne3");
			CityPath p3 = ne3.Connect(nw3, CityPathType.privates);
			m3.priority = p3;

			MergePoint m4 = new MergePoint();
			CityNode nw4 = wRight = new MergeNode(x + 80, y - 40, m4, "nw4");
			CityNode ne4 = eRight = new CityNode(x + 80, y + 40, "ne4");
			CityPath p4 = ne4.Connect(nw4, CityPathType.privates);
			m4.priority = p4;

			nw1.Connect(ne2, CityPathType.privates, 40);
			nw2.Connect(ne3, CityPathType.privates, 40);
			nw3.Connect(ne4, CityPathType.privates, 40);
			nw4.Connect(ne1, CityPathType.privates, 40);

			pTopLeft = new CityNode(x - 100, y - 100);
			pTopRight = new CityNode(x + 100, y - 100);
			pBottomLeft = new CityNode(x - 100, y + 100);
			pBottomRight = new CityNode(x + 100, y + 100);

			pTopLeft.Connect(pTopRight, CityPathType.pedestrians);
			pTopLeft.Connect(pBottomLeft, CityPathType.pedestrians);

			pTopRight.Connect(pTopLeft, CityPathType.pedestrians);
			pTopRight.Connect(pBottomRight, CityPathType.pedestrians);

			pBottomLeft.Connect(pBottomRight, CityPathType.pedestrians);
			pBottomLeft.Connect(pTopLeft, CityPathType.pedestrians);

			pBottomRight.Connect(pBottomLeft, CityPathType.pedestrians);
			pBottomRight.Connect(pTopRight, CityPathType.pedestrians);

			c.nodes.Add(pTopLeft);
			c.nodes.Add(pTopRight);
			c.nodes.Add(pBottomLeft);
			c.nodes.Add(pBottomRight);

			c.nodes.Add(nw1);
			c.nodes.Add(ne1);

			c.nodes.Add(nw2);
			c.nodes.Add(ne2);

			c.nodes.Add(nw3);
			c.nodes.Add(ne3);

			c.nodes.Add(nw4);
			c.nodes.Add(ne4);
		}

		public void Connect(CityBuilderPath p, out CityNode enter, out CityNode exit, out CityNode p1, out CityNode p2)
		{
			if (right == p)
			{
				enter = eRight;
				exit = wRight;
				p1 = pBottomRight;
				p2 = pTopRight;
			}
			else if (left == p)
			{
				enter = eLeft;
				exit = wLeft;
				p1 = pTopLeft;
				p2 = pBottomLeft;
			}
			else if (bottom == p)
			{
				enter = eBottom;
				exit = wBottom;
				p1 = pBottomLeft;
				p2 = pBottomRight;
			}
			else
			{
				enter = eTop;
				exit = wTop;
				p1 = pTopRight;
				p2 = pTopLeft;
			}
		}
	}
}
