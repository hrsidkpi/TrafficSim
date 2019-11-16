using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficSim.gui;

namespace TrafficSim.roads
{
	public class Lane : SimObject
	{

		public int xStart, yStart, xEnd, yEnd;
		public bool curve;

		public Lane(int xStart, int yStart, int xEnd, int yEnd)
		{
			this.xStart = xStart;
			this.xEnd = xEnd;
			this.yStart = yStart;
			this.yEnd = yEnd;
		}

		public override void CreateSprite()
		{
			float w = xEnd - xStart;
			float h = yEnd - yStart;
			double len = Math.Sqrt(w * w + h * h);
			double dir = Math.Atan2(h, w);

			float xCenter = xStart + (float)(len * Math.Cos(dir) * 0.5);
			float yCenter = yStart + (float)(len * Math.Sin(dir) * 0.5);

			Sprite = Assets.FullRectangle((int)len, 16, Color.Gray);
		}

		public override void Draw()
		{
			float w = xEnd - xStart;
			float h = yEnd - yStart;
			double len = Math.Sqrt(w * w + h * h);
			double dir = Math.Atan2(h, w);

			float xCenter = xStart + (float) (len * Math.Cos(dir) * 0.5);
			float yCenter = yStart + (float) (len * Math.Sin(dir) * 0.5);

			Sim.instance.DrawSprite(Sprite, new Vector2(xCenter, yCenter), dir);
		}

		public override bool MouseHitTest(double mx, double my)
		{
			throw new NotImplementedException();
		}

		public override GuiObject SideMenu()
		{
			throw new NotImplementedException();
		}

		public override void Update()
		{
		}
	}
}
