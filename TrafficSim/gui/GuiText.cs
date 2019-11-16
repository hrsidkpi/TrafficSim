using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficSim.gui
{
	public class GuiText : GuiObject
	{

		public SpriteFont font;
		public string txt;
		public ObjectLinker[] links;

		public GuiText(int x, int y, int width, int height, SpriteFont font, string txt, params ObjectLinker[] links) : base(x, y, width, height)
		{
			this.txt = txt;
			this.font = font;
			this.links = links;
		}

		public override void Draw()
		{
			string s = "";
			int j = 0;
			for (int i = 0; i < txt.Length - 1; i++)
			{
				string c = txt[i] + "" + txt[i + 1];
				if (c == "&l")
				{
					s += links[j];
					j++;
					i++;
				}
				else s += txt[i];
			}
			Sim.instance.DrawString(s, new Vector2(x, y), font, Color.Black);
		}

		public override void Update()
		{
			
		}
	}
}
