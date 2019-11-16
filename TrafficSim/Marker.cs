using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficSim.gui;

namespace TrafficSim
{
	public class Marker : SimObject
	{

		public int x, y;
		public Marker(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public override void CreateSprite()
		{
			Sprite = Assets.FullCircle(3, Color.Yellow);
		}

		public override void Draw()
		{
			throw new NotImplementedException();
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
			throw new NotImplementedException();
		}
	}
}
