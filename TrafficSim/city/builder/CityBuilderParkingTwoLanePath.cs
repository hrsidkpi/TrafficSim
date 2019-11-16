using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficSim.city.nodes;
using TrafficSim.city.specials;

namespace TrafficSim.city.builder
{
	public class CityBuilderParkingTwoLanePath : CityBuilderPath
	{

		public static int PARKING_LENGTH = 65;

		public CityBuilderNode n1, n2;

		//Road
		public CityNode s1, e1, s2, e2;

		//Sidewalk
		public CityNode p11, p12, p21, p22;

		public CityBuilderParkingTwoLanePath(CityBuilderNode s, CityBuilderNode e) : base(s, e)
		{
			this.n1 = s;
			this.n2 = e;
		}

		public override void Build(City c)
		{
			if (this.n1 is BuilderAllWayStopNode)
			{
				BuilderAllWayStopNode n1 = this.n1 as BuilderAllWayStopNode;
				n1.Connect(this, out s1, out e1, out p11, out p12);
			}
			if (this.n1 is BuilderTrafficCircleNode)
			{
				BuilderTrafficCircleNode n1 = this.n1 as BuilderTrafficCircleNode;
				n1.Connect(this, out s1, out e1, out p11, out p12);
			}
			if (this.n1 is BuilderDeadEndNode)
			{
				BuilderDeadEndNode n1 = this.n1 as BuilderDeadEndNode;
				n1.Connect(this, out s1, out e1, out p11, out p12);
			}


			if (this.n2 is BuilderAllWayStopNode)
			{
				BuilderAllWayStopNode n2 = this.n2 as BuilderAllWayStopNode;
				n2.Connect(this, out s2, out e2, out p21, out p22);
			}
			if(this.n2 is BuilderTrafficCircleNode)
			{
				BuilderTrafficCircleNode n2 = this.n2 as BuilderTrafficCircleNode;
				n2.Connect(this, out s2, out e2, out p21, out p22);
			}
			if (this.n2 is BuilderDeadEndNode)
			{
				BuilderDeadEndNode n2 = this.n2 as BuilderDeadEndNode;
				n2.Connect(this, out s2, out e2, out p21, out p22);
			}

			double dist = Util.Distance(s1.x, s1.y, e2.x, e2.y);
			int parkCount = (int)(dist / PARKING_LENGTH);
			double dir = Util.GetLookatDir(s1.x, s1.y, e2.x, e2.y);

			CityNode last = s1;
			CityNode lastPark = p11;
			for(int i = 1; i <= parkCount; i++)
			{
				int x = s1.x + (int)(i * PARKING_LENGTH * Math.Cos(dir));
				int y = s1.y + (int)(i * PARKING_LENGTH * Math.Sin(dir));
				MergeNode exit = new MergeNode(x, y, new MergePoint());

				x -= (int)(PARKING_LENGTH / 2 * Math.Cos(dir));
				x += (int)(20 * Math.Cos(dir + Math.PI / 2));
				y -= (int)(PARKING_LENGTH / 2 * Math.Sin(dir));
				y += (int)(20 * Math.Sin(dir + Math.PI / 2));
				ParkingNode park = new ParkingNode(x, y);

				CityPath priority = last.Connect(exit, CityPathType.privates);
				last.Connect(park, CityPathType.privates);
				park.Connect(exit, CityPathType.privates);

				lastPark.Connect(park, CityPathType.pedestrians);
				park.Connect(lastPark, CityPathType.pedestrians);

				c.nodes.Add(exit);
				c.nodes.Add(park);

				(exit.intersection as MergePoint).priority = priority;

				last = exit;
				lastPark = park;
			}
			last.Connect(e2, CityPathType.privates);
			lastPark.Connect(p22, CityPathType.pedestrians);
			p22.Connect(lastPark, CityPathType.pedestrians);


			dist = Util.Distance(s2.x, s2.y, e1.x, e1.y);
			parkCount = (int)(dist / PARKING_LENGTH);
			dir = Util.GetLookatDir(s2.x, s2.y, e1.x, e1.y);

			last = s2;
			lastPark = p21;
			for (int i = 1; i <= parkCount; i++)
			{
				int x = s2.x + (int)(i * PARKING_LENGTH * Math.Cos(dir));
				int y = s2.y + (int)(i * PARKING_LENGTH * Math.Sin(dir));
				CityNode exit = new CityNode(x, y);

				x -= (int)(PARKING_LENGTH / 2 * Math.Cos(dir));
				x += (int)(20 * Math.Cos(dir + Math.PI / 2));
				y -= (int)(PARKING_LENGTH / 2 * Math.Sin(dir));
				y += (int)(20 * Math.Sin(dir + Math.PI / 2));
				ParkingNode park = new ParkingNode(x, y);

				last.Connect(exit, CityPathType.privates);
				last.Connect(park, CityPathType.privates);
				park.Connect(exit, CityPathType.privates);

				lastPark.Connect(park, CityPathType.pedestrians);
				park.Connect(lastPark, CityPathType.pedestrians);

				c.nodes.Add(exit);
				c.nodes.Add(park);

				last = exit;
				lastPark = park;
			}
			last.Connect(e1, CityPathType.privates);
			lastPark.Connect(p12, CityPathType.pedestrians);
			p12.Connect(lastPark, CityPathType.pedestrians);
		}

	}
}
