using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficSim.gui;

namespace TrafficSim
{
	public abstract class SimObject
	{

		private Texture2D sprite;

		protected Texture2D Sprite
		{
			get
			{
				if (sprite == null) CreateSprite();
				return sprite;
			}
			set
			{
				sprite = value;
			}
		}

		public abstract void Update();
		public abstract void Draw();
		public abstract void CreateSprite();
		public abstract bool MouseHitTest(double mx, double my);
		public abstract GuiObject SideMenu();
	}
}
