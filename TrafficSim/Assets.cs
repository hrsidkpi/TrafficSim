using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficSim
{
	public static class Assets
	{

		public static Texture2D StopSignIntersectionBack;

		public static SpriteFont Font12;
		public static SpriteFont Font24;
		public static SpriteFont Font18;

		public static void Load()
		{
			StopSignIntersectionBack = Texture2D.FromStream(Sim.instance.GraphicsDevice, new FileStream("res/stop_sign_intersection.png", FileMode.Open));
			Font12 = Sim.instance.Content.Load<SpriteFont>("../res/bin/Font12");
			Font24 = Sim.instance.Content.Load<SpriteFont>("../res/bin/Font24");
			Font18 = Sim.instance.Content.Load<SpriteFont>("../res/bin/Font18");
		}

		public static Texture2D FullCircle(int r, Color color)
		{
			int diam = r * 2;

			Texture2D res = new Texture2D(Sim.instance.GraphicsDevice, diam, diam);
			Color[] colorData = new Color[diam * diam];

			for(int x = 0; x < diam; x++)
			{
				for(int y = 0; y < diam; y++)
				{
					int i = x + y * diam;
					Vector2 pos = new Vector2(x - r, y - r);
					if (pos.LengthSquared() <= r * r) colorData[i] = color;
					else colorData[i] = Color.Transparent;
				}
			}

			res.SetData(colorData);
			return res;	

		}

		public static Texture2D FullRectangle(int w, int h, Color color)
		{
			if (w == 0 || h == 0) return new Texture2D(Sim.instance.GraphicsDevice, 1, 1);
			Texture2D res = new Texture2D(Sim.instance.GraphicsDevice, w, h);
			Color[] colorData = new Color[w * h];

			for (int i = 0; i < w * h; i++) colorData[i] = color;

			res.SetData(colorData);
			return res;

		}

		

	}
}
