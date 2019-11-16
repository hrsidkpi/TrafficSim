using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficSim
{
	public class Program
	{

		[STAThread]
		public static void Main(string[] args)
		{
			using(Sim sim = new Sim())
			{
				sim.Run();
			}
		}

	}
}
