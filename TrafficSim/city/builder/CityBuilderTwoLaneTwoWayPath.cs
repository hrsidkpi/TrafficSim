using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficSim.city.builder
{
	public class CityBuilderTwoLaneTwoWayPath : CityBuilderPath
	{

		public CityBuilderNode n1, n2;

		//Road
		public CityNode s1, e1, s2, e2;

		//Sidewalk
		public CityNode p11, p12, p21, p22;

		public CityBuilderTwoLaneTwoWayPath(CityBuilderNode s, CityBuilderNode e) : base(s, e)
		{
			this.n1 = s;
			this.n2 = e;
		}

		public override void Build(City c)
		{
			if (this.n1 is BuilderAllWayStopNode)
			{
				BuilderAllWayStopNode n1 = this.n1 as BuilderAllWayStopNode;
				n1.Connect(this, out s1, out e1, out p11, out p21);
			}
			if (this.n1 is BuilderDeadEndNode)
			{
				BuilderDeadEndNode n1 = this.n1 as BuilderDeadEndNode;
				n1.Connect(this, out s1, out e1, out p11, out p21);
			}


			if (this.n2 is BuilderAllWayStopNode)
			{
				BuilderAllWayStopNode n2 = this.n2 as BuilderAllWayStopNode;
				n2.Connect(this, out s2, out e2, out p12, out p22);
			}
			if (this.n2 is BuilderDeadEndNode)
			{
				BuilderDeadEndNode n2 = this.n2 as BuilderDeadEndNode;
				n2.Connect(this, out s2, out e2, out p12, out p22);
			}

			s1.Connect(e2, CityPathType.privates);
			s2.Connect(e1, CityPathType.privates);
		}

	}
}
