using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficSim.city;
using TrafficSim.city.builder;
using TrafficSim.city.nodes;
using TrafficSim.city.specials;
using TrafficSim.gui;
using TrafficSim.people;
using TrafficSim.roads;
using TrafficSim.vehicles;

namespace TrafficSim
{
	public class Sim : Game
	{

		//Setting consts. The window size settings of the game.
		public const int GAME_SCALE = 105;
		public const int GAME_WIDTH = (int)(16 * GAME_SCALE);
		public const int GAME_HEIGHT = (int)(9 * GAME_SCALE);
		public const int MINUTES_PER_SECOND = 10;
		public const int STARTING_TIME = 4 * 60 + 30;

		//Static variable used for accessing the current instance of the game anywhere, using Game.instance.
		//The constructor of Game sets this variale to itself.
		public static Sim instance;

		//Objects used by the Monogame framework for graphics.
		public GraphicsDeviceManager graphics;
		public SpriteBatch spriteBatch;

		public City city = new City();
		public List<Vehicle> vehicles = new List<Vehicle>();
		public List<Commuter> commuters = new List<Commuter>();

		public List<SimObject> debugObjects = new List<SimObject>();

		//Current time of day in minutes
		public int time = STARTING_TIME;

		public int day = 1;

		public GuiObject sideMenu;

		/// <summary>
		/// Create a new game. Set the instance variable to it, and create the Monogame objects.
		/// </summary>
		public Sim()
		{
			instance = this;

			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";


		}

		/// <summary>
		/// Initialize graphics settings (screen size etc'), and call SetupGame.
		/// </summary>
		protected override void Initialize()
		{
			base.Initialize();

			graphics.PreferredBackBufferWidth = GAME_WIDTH;
			graphics.PreferredBackBufferHeight = GAME_HEIGHT;
			graphics.ApplyChanges();

			IsFixedTimeStep = true;
			TargetElapsedTime = TimeSpan.FromMilliseconds(1000 / Util.ToTicks(1));

			IsMouseVisible = true;

			sideMenu = new StackPanel(GAME_WIDTH - 350, 0, 350, GAME_HEIGHT);

			CityBuilder builder = new CityBuilder();

			BuilderTrafficCircleNode i1 = new BuilderTrafficCircleNode(430, 415);
			BuilderAllWayStopNode i2 = new BuilderAllWayStopNode(940, 415);

			BuilderDeadEndNode e1top = new BuilderDeadEndNode(430, 50, 0);
			BuilderDeadEndNode e1left = new BuilderDeadEndNode(65, 415, MathHelper.ToRadians(-90));
			BuilderDeadEndNode e1bottom = new BuilderDeadEndNode(430, 780, MathHelper.ToRadians(180));

			BuilderDeadEndNode e2top = new BuilderDeadEndNode(940, 50, MathHelper.ToRadians(0));
			BuilderDeadEndNode e2right = new BuilderDeadEndNode(1310, 415, MathHelper.ToRadians(90));
			BuilderDeadEndNode e2bottom = new BuilderDeadEndNode(940, 780, MathHelper.ToRadians(180));

			CityBuilderParkingTwoLanePath connection = new CityBuilderParkingTwoLanePath(i1, i2);
			i1.right = connection;
			i2.left = connection;

			CityBuilderParkingTwoLanePath i1top = new CityBuilderParkingTwoLanePath(i1, e1top);
			i1.top = i1top;
			CityBuilderParkingTwoLanePath i1left = new CityBuilderParkingTwoLanePath(i1, e1left);
			i1.left = i1left;
			CityBuilderParkingTwoLanePath i1bottom = new CityBuilderParkingTwoLanePath(i1, e1bottom);
			i1.bottom = i1bottom;

			CityBuilderParkingTwoLanePath i2right = new CityBuilderParkingTwoLanePath(i2, e2right);
			i2.right = i2right;
			CityBuilderParkingTwoLanePath i2top = new CityBuilderParkingTwoLanePath(i2, e2top);
			i2.top = i2top;
			CityBuilderParkingTwoLanePath i2bottom = new CityBuilderParkingTwoLanePath(i2, e2bottom);
			i2.bottom = i2bottom;

			builder.nodes.Add(i1);
			builder.nodes.Add(i2);
			builder.nodes.Add(e1top);
			builder.nodes.Add(e1left);
			builder.nodes.Add(e1bottom);
			builder.nodes.Add(e2right);
			builder.nodes.Add(e2top);
			builder.nodes.Add(e2bottom);
			builder.paths.Add(connection);
			builder.paths.Add(i1top);
			builder.paths.Add(i1bottom);
			builder.paths.Add(i1left);
			builder.paths.Add(i2right);
			builder.paths.Add(i2top);
			builder.paths.Add(i2bottom);
			city = builder.Build();

			Random r = new Random();

			foreach (CityNode n in city.nodes)
			{
				if (n is ParkingNode)
				{
					//vehicles.Add(new GasolineCar(n));
				}
			}

			CityNode work1 = new CityNode(1200, 300);
			CityNode work2 = new CityNode(1100, 600);
			city.nodes.Add(work1);
			city.nodes.Add(work2);

			CityNode n1 = city.NearestNode(1200, 300, CityPathType.pedestrians);
			work1.Connect(n1, CityPathType.pedestrians);
			n1.Connect(work1, CityPathType.pedestrians);
			CityNode n2 = city.NearestNode(1100, 600, CityPathType.pedestrians);
			work2.Connect(n2, CityPathType.pedestrians);
			n2.Connect(work2, CityPathType.pedestrians);


			CityNode[] homes = new CityNode[]
			{
				new CityNode(250, 200),
				new CityNode(270, 300),
				new CityNode(120, 300),
				new CityNode(570, 100),
				new CityNode(570, 210),
				new CityNode(220, 560),
				new CityNode(550, 560),
				new CityNode(580, 700),
				new CityNode(750, 560),
				new CityNode(780, 720),
				new CityNode(780, 310),
				new CityNode(1080, 720),
				new CityNode(1080, 100),
			};

			foreach (CityNode home in homes)
			{
				city.nodes.Add(home);
				CityNode n = city.NearestNode((int)home.x, (int)home.y, CityPathType.pedestrians);
				home.Connect(n, CityPathType.pedestrians);
				n.Connect(home, CityPathType.pedestrians);

				CityNode park = City.ClosestParking(n);

				GasolineCar c = new GasolineCar(park);
				vehicles.Add(c);

				Commuter guy = new Commuter(home, home, r.Next(2) == 0 ? work1 : work2, c);
				debugObjects.Add(guy);

			}

		}

		/// <summary>
		/// Called by monogame when the framework is ready to load images. Assets.Load() loads all the assets used for the game.
		/// </summary>
		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);
			Assets.Load();
		}

		int updateCounter = 0;
		MouseState oldMouse;
		protected override void Update(GameTime gameTime)
		{
			updateCounter++;
			if (updateCounter == Util.ToTicks(1))
			{
				updateCounter = 0;
				time += MINUTES_PER_SECOND;
				if (time == 60 * 24)
				{
					time = 0;
					day++;
				}
			}

			sideMenu.Update();
			foreach (Vehicle v in vehicles) v.Update();
			foreach (SimObject o in debugObjects) o.Update();
			city.Update();

			MouseState mouse = Mouse.GetState();
			if (oldMouse == null)
				oldMouse = mouse;

			if (mouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton == ButtonState.Released)
			{
				foreach(Vehicle v in vehicles)
				{
					if (v.MouseHitTest(mouse.X, mouse.Y))
						sideMenu = v.SideMenu();
				}
			}
			oldMouse = mouse;

		}

		protected override void Draw(GameTime gameTime)
		{

			GraphicsDevice.Clear(Color.CornflowerBlue);
			spriteBatch.Begin();

			DrawSprite(Assets.StopSignIntersectionBack, new Vector2(Assets.StopSignIntersectionBack.Width / 2, Assets.StopSignIntersectionBack.Height / 2), 0);

			foreach (SimObject o in debugObjects) o.Draw();
			city.Draw();
			foreach (Vehicle v in vehicles) v.Draw();

			sideMenu.Draw();
			DrawString(Util.GetTimeString(), new Vector2(1300, 770), Color.Black);

			spriteBatch.End();

			base.Draw(gameTime);
		}

		public void DrawSprite(Texture2D texture, Vector2 pos, double rot)
		{
#pragma warning disable 0618
			spriteBatch.Draw(texture, position: pos, color: Color.White, rotation: (float)rot, origin: new Vector2(texture.Width / 2, texture.Height / 2));
#pragma warning restore 0618
		}

		public void DrawString(string txt, Vector2 pos, Color color)
		{
			spriteBatch.DrawString(Assets.Font24, txt, pos, color);
		}

		public void DrawString(string txt, Vector2 pos, SpriteFont font, Color color)
		{
			spriteBatch.DrawString(font, txt, pos, color);
		}



	}

}
