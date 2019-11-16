using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficSim
{
	public static class Util
	{

		public static double GetLookatDir(double xStart, double yStart, double xEnd, double yEnd)
		{
			double w = xEnd - xStart;
			double h = yEnd - yStart;
			return Math.Atan2(h, w);
		}

		public static double Distance(double xStart, double yStart, double xEnd, double yEnd)
		{
			double w = xStart - xEnd;
			double h = yStart - yEnd;
			return Math.Sqrt(h * h + w * w);
		}

		public static double DirectionDiff(double d1, double d2)
		{
			double a1 = MathHelper.ToDegrees((float)d1);
			double a2 = MathHelper.ToDegrees((float)d2);

			double diff1 = a2 - a1;


			double res = diff1;
			if (diff1 > 180) res = diff1 - 360;
			else if (diff1 < -180) res = diff1 + 360;

			return MathHelper.ToRadians((float)res);

		}

		public static double ToPixels(double meters)
		{
			return meters * 8;
		}

		public static double ToTicks(double seconds)
		{
			return seconds * 120.0;
		}

		public static double StopTime(double speed, double deacceleration)
		{
			return speed / deacceleration;
		}

		public static double StopDistance(double speed, double deacceleration)
		{
			return speed / 2 * StopTime(speed, deacceleration);
		}

		public static double SlowTime(double speed, double deacceleration, double desiredSpeed)
		{
			return (speed - desiredSpeed) / deacceleration;
		}

		public static double SlowDistance(double speed, double deacceleration, double desiredSpeed)
		{
			return ((speed + desiredSpeed) / 2) * SlowTime(speed, deacceleration, desiredSpeed);
		}

		public static string GetTimeString()
		{
			string hours = Sim.instance.time / 60 < 10 ? "0" + Sim.instance.time / 60 : "" + Sim.instance.time / 60;
			string minutes = Sim.instance.time % 60 < 10 ? "0" + Sim.instance.time % 60 : "" + Sim.instance.time % 60;
			return hours + ":" + minutes + (Sim.instance.time / 60 <= 12 ? " AM" : " PM");
		}

		public static string GetTimeString(int time)
		{
			string hours = time / 60 < 10 ? "0" + time / 60 : "" + time / 60;
			string minutes = time % 60 < 10 ? "0" + time % 60 : "" + time % 60;
			return hours + ":" + minutes + (time / 60 <= 12 ? " AM" : " PM");
		}

	}
}
