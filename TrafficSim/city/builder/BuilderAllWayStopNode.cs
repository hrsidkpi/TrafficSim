using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficSim.city.nodes;
using TrafficSim.city.specials;

namespace TrafficSim.city.builder
{
	public class BuilderAllWayStopNode : CityBuilderNode
	{

		public CityBuilderPath left, right, top, bottom;

		public CityNode wLeft, wRight, wTop, wBottom;
		public CityNode eLeft, eRight, eTop, eBottom;
		public CityNode pTopLeft, pTopRight, pBottomLeft, pBottomRight;

		public BuilderAllWayStopNode(int x, int y) : base(x, y)
		{
		}

		public override void Build(City c)
		{
			StopSignIntersection i = new StopSignIntersection();

			CityNode nw1 = wTop = new StopSignNode(x - 40, y - 80, i, "nw1");
			CityNode nl1 = new CityNode(x - 40, y, "nl1");
			CityNode ne1 = eTop = new IntersectionExitNode(x + 40, y - 80, i, "ne1");
			nw1.Connect(nl1, CityPathType.privates);

			CityNode nw2 = wLeft = new StopSignNode(x - 80, y + 40, i, "nw2");
			CityNode nl2 = new CityNode(x, y + 40, "nl2");
			CityNode ne2 = eLeft = new IntersectionExitNode(x - 80, y - 40, i, "ne2");
			nw2.Connect(nl2, CityPathType.privates);

			CityNode nw3 = wBottom = new StopSignNode(x + 40, y + 80, i, "nw3");
			CityNode nl3 = new CityNode(x + 40, y, "nl3");
			CityNode ne3 = eBottom = new IntersectionExitNode(x - 40, y + 80, i, "ne3");
			nw3.Connect(nl3, CityPathType.privates);

			CityNode nw4 = wRight = new StopSignNode(x + 80, y - 40, i, "nw4");
			CityNode nl4 = new CityNode(x, y - 40, "nl4");
			CityNode ne4 = eRight = new IntersectionExitNode(x + 80, y + 40, i, "ne4");
			nw4.Connect(nl4, CityPathType.privates);

			nl1.Connect(ne3, CityPathType.privates);
			nl2.Connect(ne4, CityPathType.privates);
			nl3.Connect(ne1, CityPathType.privates);
			nl4.Connect(ne2, CityPathType.privates);

			nl1.Connect(nl2, CityPathType.privates, 40);
			nl2.Connect(nl3, CityPathType.privates, 40);
			nl3.Connect(nl4, CityPathType.privates, 40);
			nl4.Connect(nl1, CityPathType.privates, 40);

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
			c.nodes.Add(nl1);
			c.nodes.Add(ne1);

			c.nodes.Add(nw2);
			c.nodes.Add(nl2);
			c.nodes.Add(ne2);

			c.nodes.Add(nw3);
			c.nodes.Add(nl3);
			c.nodes.Add(ne3);

			c.nodes.Add(nw4);
			c.nodes.Add(nl4);
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
