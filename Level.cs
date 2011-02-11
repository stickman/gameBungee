using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

using FarseerPhysics;
using FarseerPhysics.Collision;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;

namespace gameBungee
{
    class Level
    {
        World worldPhysic;
        ContentManager content;
        List<FarseerPhysics.Dynamics.Joints.SliderJoint> jointList;
        GraphicsDevice graphics;
        private static VertexDeclaration _vertexDeclaration;
        private static BasicEffect _effect;
        private Matrix proj;
        private Matrix view;
        
        SpriteFont spriteFont;

        ObjCercle obj1;
        
        ObjRectange obj2;
        ObjRectange obj21;
        ObjRectange obj22;
        ObjRectange obj23;

        ObjRectange platform1;
        ObjRectange platform2;

        ObjPolygon  obj3;
        ObjPolygon  obj4;

        Character Player;
        PlayerController Controller;
        
        public Level(IServiceProvider serviceProvider)
        {
            worldPhysic = new World(new Vector2(0, -80.0f));
            content = new ContentManager(serviceProvider, "Content");
        }

        public void LoadContent(GraphicsDevice Graphics, ContentManager Content, ref Matrix proj, ref Matrix view)
        {
            this.graphics = Graphics;
            _vertexDeclaration = new VertexDeclaration(VertexPositionColor.VertexDeclaration.GetVertexElements());
            
            _effect = new BasicEffect(Graphics);
            _effect.VertexColorEnabled = true;
            _effect.View = view;
            _effect.Projection = proj;

            this.view = view;
            this.proj = proj;

            Texture2D texture = Content.Load<Texture2D>("textures/Idle");
            Player = new Character(texture, Graphics, ref proj, ref view, worldPhysic, Content);

            Controller = new PlayerController();

            this.spriteFont = Content.Load<SpriteFont>("font/Hud");

            //obj1 = new ObjCercle(new Vector2(0, 0), 1, 0.0f, worldPhysic);
            //obj1.addDisplayer();

            obj2 = new ObjRectange(new Vector2(0, -30), 80, 5, 0.0f, worldPhysic, BodyType.Static);
            obj2.addDisplayer();
            obj21 = new ObjRectange(new Vector2(40, 0), 80, 5, (float)Math.PI/2.0f, worldPhysic, BodyType.Static);
            obj21.addDisplayer();
            obj22 = new ObjRectange(new Vector2(-40, 0), 80, 5, (float)Math.PI / 2.0f, worldPhysic, BodyType.Static);
            obj22.addDisplayer();
            obj23 = new ObjRectange(new Vector2(0, 30), 80, 5, 0.0f, worldPhysic, BodyType.Static);
            obj23.addDisplayer();

            platform1 = new ObjRectange(new Vector2(-30, 0), 20, 5, 0.0f, worldPhysic, BodyType.Static);
            platform1.addDisplayer();
            platform2 = new ObjRectange(new Vector2(25, -10), 30, 5, 0.0f, worldPhysic, BodyType.Static);
            platform2.addDisplayer();

            List<Vector2> vertlist = new List<Vector2>();
            vertlist.Add(new Vector2(-40, 0));
            vertlist.Add(new Vector2(-30, -15));
            vertlist.Add(new Vector2(-20, -17));
            vertlist.Add(new Vector2(0, -30));
            vertlist.Add(new Vector2(20, -17));
            vertlist.Add(new Vector2(30, -15));
            vertlist.Add(new Vector2(40, 0));
            vertlist.Add(new Vector2(40, -40));
            vertlist.Add(new Vector2(-40, -40));

            //obj3 = new ObjPolygon(vertlist, BodyType.Static, worldPhysic);
            //obj3.addDisplayer();
            //obj3.setFriction(1.0f);

            //vertlist.Clear();
            //vertlist.Add(new Vector2(-10, -10 - 10));
            //vertlist.Add(new Vector2(10, -10 - 10));
            //vertlist.Add(new Vector2(10, 20 - 10));
            //vertlist.Add(new Vector2(-10, 20 - 10));
            //obj4 = new ObjPolygon(vertlist, BodyType.Static, worldPhysic);
            //obj4.addDisplayer();
        }
        
            private void JoinObject(Fixture Obj1, Fixture Obj2, Vector2 AnchorPoint1, Vector2 AnchorPoint2, float minLength, float maxLength)
        {
            jointList.Add(JointFactory.CreateSliderJoint(worldPhysic, Obj1.Body, Obj2.Body, AnchorPoint1, AnchorPoint2, minLength, maxLength));
        }

        public void Update(GameTime gameTime)
        {

            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboardState = Keyboard.GetState();
            
            Controller.UpdateInteraction(Player, graphics, view, proj);

            worldPhysic.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.002f);
            Player.update();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            double FPS = 1000.0d / gameTime.ElapsedGameTime.TotalMilliseconds;
            //Formatage de la chaine
            MouseState ms = Mouse.GetState();

            var playerPos = graphics.Viewport.Project(new Vector3(Player.ObjectPhysicCircleTete.FixtureObject.Body.Position, 0),
                                proj, view, Matrix.Identity);

            string texte = string.Format("souris {0:d}:{1:d}\ntete {2:000}:{3:000}\n{4:00.00} images / sec", ms.X, ms.Y, playerPos.X, playerPos.Y, FPS);//string.Format("{0:00.00} images / sec", FPS);
            Vector2 taille = this.spriteFont.MeasureString(texte);
            /*
            // set the cull mode? should be unnecessary
            graphics.RenderState.CullMode = CullMode.None;
            // turn alpha blending on
            graphics.RenderState.AlphaBlendEnable = true;
            // set the vertex declaration...this ensures if window resizes occur...rendering continues ;)
            graphics.VertexDeclaration = _vertexDeclaration;
            */
            RasterizerState rasterizerState1 = new RasterizerState();
            rasterizerState1.CullMode = CullMode.None;
            graphics.RasterizerState = rasterizerState1;

            _effect.Techniques[0].Passes[0].Apply();


            //obj1.Draw(gameTime, spriteBatch, SpriteEffects.None);
            //obj1.Draw(graphics);
            obj2.Draw(graphics);
            
            obj21.Draw(graphics);
            obj22.Draw(graphics);
            obj23.Draw(graphics);

            platform1.Draw(graphics);
            platform2.Draw(graphics);

            //obj2.Draw(gameTime, spriteBatch, SpriteEffects.None);
            //obj3.Draw(graphics);
            //obj4.Draw(graphics);

            Player.Draw(gameTime, graphics);

            spriteBatch.Begin();
            spriteBatch.DrawString(this.spriteFont, texte, new Vector2(this.graphics.Viewport.Width - taille.X, 5), Color.Green);
            spriteBatch.End();
        }
    }
}
