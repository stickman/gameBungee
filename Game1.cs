using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using System.Xml;

using FarseerPhysics;
using FarseerPhysics.Dynamics;

namespace gameBungee
{

    static public class EnvironmentVariable
    {
        static public World worldPhysic;
        static public GraphicsDeviceManager graphics;
        static public int width;
        static public int height;
        static public List<ObjCercle>   lCercle = new List<ObjCercle>();
        static public List<ObjCercle>   lCerclePol = new List<ObjCercle>();
        static public List<ObjEdge>     lEdge = new List<ObjEdge>();
        static public List<ObjRectange> lRectangle = new List<ObjRectange>();
        static public List<ObjPolygon>  lPolygon = new List<ObjPolygon>();
        static public ContentManager content;
    }

    public enum TextureImg { Null, tree1, tree2, tree3, tree4, tree5,
    tree6, tree7, tree8, tree9, tree10, grass, Run, Idle
    };
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        SpriteBatch spriteBatch;

        private Level level;
        private SamplerState _clampTextureAddressMode;

        public Game1()
        {
            EnvironmentVariable.graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            EnvironmentVariable.content = Content;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here


            EnvironmentVariable.graphics.IsFullScreen = false;
            EnvironmentVariable.width = 800;
            EnvironmentVariable.height = 500;
            EnvironmentVariable.graphics.PreferredBackBufferWidth = EnvironmentVariable.width;
            EnvironmentVariable.graphics.PreferredBackBufferHeight = EnvironmentVariable.height;
            EnvironmentVariable.graphics.ApplyChanges();
         
            this.Window.Title = "Premier programme";
            this.Window.AllowUserResizing = false;

            level = new Level(Services, EnvironmentVariable.width, EnvironmentVariable.height/*this.Window.ClientBounds.Width, this.Window.ClientBounds.Height*/);
            loadMap("C:\\Documents and Settings\\yoann\\Bureau\\level1.xml");
            base.IsMouseVisible = true;
            base.Initialize();
        }

        private void loadMap(string filename)
        {
            using (XmlReader reader = XmlReader.Create(filename))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        switch (reader.Name)
                        {
                            case "Circles":
                                Vector2 pos = Vector2.Zero;
                                float radius = 0.0f;
                                BodyType BT = BodyType.Static;
                                Microsoft.Xna.Framework.Color color = Microsoft.Xna.Framework.Color.Black;
                                while (reader.Read() && !(reader.Name == "Circles" && !reader.IsStartElement())) // It read all the "Circle"
                                {
                                    if (reader.IsStartElement())
                                    {
                                        switch (reader.Name)
                                        {
                                            case "PositionInitial":
                                                if (reader.Read())
                                                    pos = ParserXML.parseXmlToVector2(reader.Value.Trim());
                                                //reader.Read();
                                                break;
                                            case "radius":
                                                if (reader.Read())
                                                    radius = ParserXML.parseXmlToFloat(reader.Value.Trim());
                                                //reader.Read();
                                                break;
                                            case "BodyType":
                                                if (reader.Read())
                                                    BT = ParserXML.parseXmlToBodyType(reader.Value.Trim());
                                                //reader.Read();
                                                break;
                                            case "Color":
                                                if (reader.Read())
                                                    color = ParserXML.parseXmlToColor(reader.Value.Trim());
                                                //reader.Read();
                                                break;
                                        }
                                    }
                                    else if (!reader.IsStartElement() && reader.Name == "Circle")
                                    {
                                        EnvironmentVariable.lCercle.Add(new ObjCercle(pos, radius, 0.0f, EnvironmentVariable.worldPhysic, BT));
                                        EnvironmentVariable.lCercle.Last().Color = color;
                                        EnvironmentVariable.lCercle.Last().addDisplayer();
                                    }
                                }
                                break;
                            case "Rectangles":
                                Vector2 posR = Vector2.Zero;
                                Vector2 sizeR = Vector2.Zero;
                                float angle = 0.0f;
                                BodyType BTR = BodyType.Static;
                                Microsoft.Xna.Framework.Color colorR = Microsoft.Xna.Framework.Color.Black;
                                TextureImg tex = TextureImg.Null;

                                while (reader.Read() && !(reader.Name == "Rectangles" && !reader.IsStartElement())) // It read all the "Circle"
                                {
                                    if (reader.IsStartElement())
                                    {
                                        switch (reader.Name)
                                        {
                                            case "PositionInitial":
                                                if (reader.Read())
                                                    posR = ParserXML.parseXmlToVector2(reader.Value.Trim());
                                                //reader.Read();
                                                break;
                                            case "Size":
                                                if (reader.Read())
                                                    sizeR = ParserXML.parseXmlToVector2(reader.Value.Trim());
                                                //reader.Read();
                                                break;
                                            case "Angle":
                                                if (reader.Read())
                                                    angle = ParserXML.parseXmlToFloat(reader.Value.Trim());
                                                //reader.Read();
                                                break;
                                            case "BodyType":
                                                if (reader.Read())
                                                    BTR = ParserXML.parseXmlToBodyType(reader.Value.Trim());
                                                //reader.Read();
                                                break;
                                            case "Color":
                                                if (reader.Read())
                                                    colorR = ParserXML.parseXmlToColor(reader.Value.Trim());
                                                //reader.Read();
                                                break;
                                            case "Texture":
                                                if (reader.Read())
                                                    tex = ParserXML.parseXmlToTexture(reader.Value.Trim());
                                                //reader.Read();
                                                break;
                                        }
                                    }
                                    else if (!reader.IsStartElement() && reader.Name == "Rectangle")
                                    {
                                        Texture2D img = null;
                                        EnvironmentVariable.lRectangle.Add(new ObjRectange(posR + sizeR / 2,
                                            sizeR.X,
                                            sizeR.Y,
                                            0.0f,
                                            BTR));
                                        EnvironmentVariable.lRectangle.Last().Angle = angle;
                                        EnvironmentVariable.lRectangle.Last().Color = colorR;
                                        switch (tex)
                                        {
                                            case TextureImg.tree1:
                                                img = EnvironmentVariable.content.Load<Texture2D>("textures/tree1");
                                                break;
                                            case TextureImg.tree2:
                                                img = EnvironmentVariable.content.Load<Texture2D>("textures/tree2");
                                                break;
                                            case TextureImg.tree3:
                                                img = EnvironmentVariable.content.Load<Texture2D>("textures/tree3");
                                                break;
                                            case TextureImg.tree4:
                                                img = EnvironmentVariable.content.Load<Texture2D>("textures/tree4");
                                                break;
                                            case TextureImg.tree5:
                                                img = EnvironmentVariable.content.Load<Texture2D>("textures/tree5");
                                                break;
                                            case TextureImg.tree6:
                                                img = EnvironmentVariable.content.Load<Texture2D>("textures/tree6");
                                                break;
                                            case TextureImg.tree7:
                                                img = EnvironmentVariable.content.Load<Texture2D>("textures/tree7");
                                                break;
                                            case TextureImg.tree8:
                                                img = EnvironmentVariable.content.Load<Texture2D>("textures/tree8");
                                                break;
                                            case TextureImg.tree9:
                                                img = EnvironmentVariable.content.Load<Texture2D>("textures/tree9");
                                                break;
                                            case TextureImg.tree10:
                                                img = EnvironmentVariable.content.Load<Texture2D>("textures/tree10");
                                                break;
                                            case TextureImg.grass:
                                                img = EnvironmentVariable.content.Load<Texture2D>("textures/grass");
                                                break;
                                            case TextureImg.Idle:
                                                img = EnvironmentVariable.content.Load<Texture2D>("textures/Idle");
                                                break;
                                            case TextureImg.Run:
                                                img = EnvironmentVariable.content.Load<Texture2D>("textures/Run");
                                                break;
                                        }
                                        EnvironmentVariable.lRectangle.Last().Texture = tex;
                                        EnvironmentVariable.lRectangle.Last().addTexture(img, 0.0f);
                                        EnvironmentVariable.lRectangle.Last().addDisplayer();
                                    }
                                }
                                break;
                            case "Polygons":
                                Vector2 posP = Vector2.Zero;
                                BodyType BTP = BodyType.Static;
                                Microsoft.Xna.Framework.Color colorP = Microsoft.Xna.Framework.Color.Black;
                                List<Vector2> vert = new List<Vector2>();
                                while (reader.Read() && !(reader.Name == "Polygons" && !reader.IsStartElement())) // It read all the "Circle"
                                {
                                    if (reader.IsStartElement())
                                    {
                                        switch (reader.Name)
                                        {
                                            case "PositionInitial":
                                                if (reader.Read())
                                                    posP = ParserXML.parseXmlToVector2(reader.Value.Trim());
                                                //reader.Read();
                                                break;
                                            case "BodyType":
                                                if (reader.Read())
                                                    BTP = ParserXML.parseXmlToBodyType(reader.Value.Trim());
                                                //reader.Read();
                                                break;
                                            case "Color":
                                                if (reader.Read())
                                                    colorP = ParserXML.parseXmlToColor(reader.Value.Trim());
                                                //reader.Read();
                                                break;
                                            case "Vertices":
                                                while (reader.Read() && reader.Name != "Vertices")
                                                {
                                                    if (reader.Read())
                                                        vert.Add(ParserXML.parseXmlToVector2(reader.Value.Trim()) + posP);
                                                    reader.Read();
                                                }
                                                break;
                                        }
                                    }
                                    else if (!reader.IsStartElement() && reader.Name == "Polygon")
                                    {
                                        List<Vector2> vertTp = new List<Vector2>(vert);
                                        EnvironmentVariable.lPolygon.Add(new ObjPolygon(vertTp, BTP, EnvironmentVariable.worldPhysic));
                                        EnvironmentVariable.lPolygon.Last().addDisplayer();
                                        EnvironmentVariable.lPolygon.Last().Color = colorP;

                                        vert.Clear();
                                    }
                                }
                                break;

                        }
                    }
                }
            }


        }
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(EnvironmentVariable.graphics.GraphicsDevice);

            EnvironmentVariable.width = GraphicsDevice.DisplayMode.Width;
            EnvironmentVariable.height= GraphicsDevice.DisplayMode.Height;

            //CameraController.Inialized(new Rectangle(EnvironmentVariable.width - 10, EnvironmentVariable.height / 4, 20, EnvironmentVariable.height / 2), EnvironmentVariable.graphics.GraphicsDevice);

            _clampTextureAddressMode = new SamplerState
            {
                AddressU = TextureAddressMode.Clamp,
                AddressV = TextureAddressMode.Clamp
            };

            level.LoadContent(EnvironmentVariable.graphics.GraphicsDevice, Content);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            level.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            EnvironmentVariable.graphics.GraphicsDevice.SamplerStates[0] = _clampTextureAddressMode; 

            level.Draw(gameTime, spriteBatch);

            base.Draw(gameTime);
        }
    }
}
