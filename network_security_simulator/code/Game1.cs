using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Text;
using System;

namespace simulation_with_full_device_details
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public Texture2D Tower1;
        public Texture2D Towerr2;
        public Texture2D centralWire;
        public Texture2D nodeWires;
        public Vector2 Tower1Position;
        public Vector2 Tower2Position;
        public Vector2 centralWirePosition;
        public Vector2[] nodeWirePositions = new Vector2[5];
        public Vector2 nodeWirePosition;
        public Vector2 mousePosition;
        private List<string> tempholder = new List<string>();// will hold a copy of the nodeweaknesses list
      

        SpriteFont font;
        Nodes[] nodeStorage = new Nodes[5];
        private Nodes node;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1000;
            _graphics.PreferredBackBufferHeight = 700;
            _graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            this.Window.Title = "Cyber Attack simulator";
            IsMouseVisible = true;

            centralWirePosition = new Vector2(130, 360);
            float[] nodeWireXPos = {183,365,500,683,845};
            float[] nodeWireYPos = { 370, 280, 370, 280, 370 };
            int placeIndicator = 0;
            for (int i = 0; i < 5; i++)
            {
                nodeWirePosition = new Vector2(nodeWireXPos[placeIndicator], nodeWireYPos[placeIndicator]);
                nodeWirePositions[i] = nodeWirePosition;
                placeIndicator += 1;
            }

        }

        protected override void Initialize()
        {
            centralWire = new Texture2D(this.GraphicsDevice, 740,10);
            Color[] colorCentralCable = new Color[740 * 10];

            for (int i = 0; i < (740 *10); i++)
            {
                colorCentralCable[i] = Color.Crimson;
            }
            centralWire.SetData<Color>(colorCentralCable);

            nodeWires = new Texture2D(this.GraphicsDevice, 5, 80);
            Color[] nodewireCables = new Color[5 * 80];
            for (int i = 0; i < (5*80); i++)
            {
                nodewireCables[i] = Color.DimGray;
            }
            nodeWires.SetData<Color>(nodewireCables);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("Font");
            var texture = Content.Load<Texture2D>("computerResized");

            for (int i = 0; i < nodeStorage.Length; i++)
            {
                node = new Nodes(texture, i);
                nodeStorage[i] = node;
            }
            Tower1 = Content.Load<Texture2D>("t1");
            Tower1Position = new Vector2(69,250);
            Towerr2 = Content.Load<Texture2D>("t1");
            Tower2Position = new Vector2(870, 250);
        }

        protected override void Update(GameTime gameTime)
        {
            _graphics.PreferredBackBufferWidth = 1000;
            _graphics.PreferredBackBufferHeight = 700;
            _graphics.ApplyChanges();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //add mouse input
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            MouseState mouse = Mouse.GetState();
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                mousePosition.X = mouse.X;
                mousePosition.Y = mouse.Y;
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.AliceBlue);
            _spriteBatch.Begin();
            for (int i = 0; i < nodeStorage.Length; i++)
            {
                nodeStorage[i].Draw(_spriteBatch);
            }
            _spriteBatch.Draw(Tower1, Tower1Position, Color.White);
            _spriteBatch.Draw(Towerr2, Tower2Position, Color.White);
            _spriteBatch.Draw(centralWire, centralWirePosition, Color.White);
            int placeIndicator = 0;
            for (int i = 0; i < 5; i++)
            {
                _spriteBatch.Draw(nodeWires, nodeWirePositions[placeIndicator], Color.White);
                placeIndicator += 1;
            }

            foreach (var node in nodeStorage) //general device info
            {
                if (mousePosition.X >= node.GetNodeXPos() && mousePosition.X <= (node.GetNodeXPos() + 150) && mousePosition.Y >= node.GetNodeYPos() && mousePosition.Y <= (node.GetNodeYPos() + 150))
                {
                    node.GetDeviceDetails();
                    node.GetPeopleDetails();
                    _spriteBatch.DrawString(font, node.DisplayNodeDetails(), new Vector2(0,0), Color.Black);
                }
            } 

            foreach (var node in nodeStorage) //attack info - when you create a menu, this should be changed to only be shown in attack instance
            {
                
                    node.GetDeviceSecurityScore();
                    node.getUserSecurityScore();
                    node.getNodeSecurityLevels();
                    node.determineWhetherDeviceWillBeAttacked();
                    _spriteBatch.DrawString(font, node.displayStatusOfNodes(), new Vector2(node.GetNodeXPos(), node.GetNodeYPos()), Color.Black);                    
                                
            }            

            base.Draw(gameTime);
            _spriteBatch.End();
        }
    }
}
