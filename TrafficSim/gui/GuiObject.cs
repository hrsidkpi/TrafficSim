using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficSim.gui
{
	public abstract class GuiObject
	{

		public int x, y;
		public int width, height;

		public GuiObject(int x, int y, int width, int height)
		{
			this.width = width;
			this.height = height;
			this.x = x;
			this.y = y;
		}

		public abstract void Draw();

		public abstract void Update();

		public bool MouseHitTest(int mx, int my)
		{
			return false;
		}
		public GuiObject SideMenu()
		{
			return null;
		}

	}
}
