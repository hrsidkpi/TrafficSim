using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficSim.gui
{
	public class StackPanel : GuiObject
	{

		private readonly List<GuiObject> children;
		public Texture2D sprite;

		public StackPanel(int x, int y, int width, int height) : base(x, y, width, height)
		{
			sprite = Assets.FullRectangle(width, height, Color.Gray);
			children = new List<GuiObject>();
		}

		public override void Draw()
		{
			Sim.instance.DrawSprite(sprite, new Vector2(x + width / 2, y + height / 2), 0);
			foreach (GuiObject o in children) o.Draw();
		}

		public void AddChild(GuiObject o)
		{
			o.x += x;
			o.y += y;
			children.Add(o);
		}

		public override void Update()
		{
			
		}
	}
}
