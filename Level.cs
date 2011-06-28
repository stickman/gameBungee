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
        ContentManager content;
        GraphicsDevice graphics;        
        SpriteFont spriteFont;
        Character Player;
        PlayerController Controller;

        int Width;
        int Height;
        public Level(IServiceProvider serviceProvider, int width, int height)
        {
            Width = width;
            Height = height;

            EnvironmentVariable.worldPhysic = new World(new Vector2(0, -80.0f));
            content = new ContentManager(serviceProvider, "Content");
        }

        public void LoadContent(GraphicsDevice Graphics, ContentManager Content)
        {
            this.graphics = Graphics;
            //_vertexDeclaration = new VertexDeclaration(VertexPositionColor.VertexDeclaration.GetVertexElements());
            CameraController.Inialized(new Rectangle(Width / 3, Height / 3, Width / 3, Height / 3), EnvironmentVariable.graphics.GraphicsDevice);

            this.spriteFont = Content.Load<SpriteFont>("font/Hud");

            Texture2D texture = Content.Load<Texture2D>("textures/Idle");
            Player = new Character(texture, EnvironmentVariable.worldPhysic, EnvironmentVariable.content, CameraController.effect,new Vector2(0,20));

            Controller = new PlayerController();


        }

        public void Update(GameTime gameTime)
        {

            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboardState = Keyboard.GetState();

            Controller.UpdateInteraction(Player, graphics);

            EnvironmentVariable.worldPhysic.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.002f);
            Player.update();
            CameraController.update(Player, EnvironmentVariable.graphics.GraphicsDevice);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            double FPS = 1000.0d / gameTime.ElapsedGameTime.TotalMilliseconds;
            //Formatage de la chaine
            MouseState ms = Mouse.GetState();

            var playerPos = EnvironmentVariable.graphics.GraphicsDevice.Viewport.Project(new Vector3(Player.ObjectPhysicCircleTete.FixtureObject.Body.Position, 0),
                                CameraController.proj, CameraController.view, Matrix.Identity);

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
            EnvironmentVariable.graphics.GraphicsDevice.RasterizerState = rasterizerState1;

            CameraController.effect.Techniques[0].Passes[0].Apply();

            foreach (ObjCercle c in EnvironmentVariable.lCercle)
                c.Draw();
            foreach (ObjRectange r in EnvironmentVariable.lRectangle)
            {
                if (r.Texture == TextureImg.Null)
                    r.Draw();
            }

            foreach (ObjCercle c in EnvironmentVariable.lCerclePol)
                c.Draw();

            foreach (ObjEdge e in EnvironmentVariable.lEdge)
                e.Draw();

            foreach (ObjPolygon p in EnvironmentVariable.lPolygon)
                p.Draw();

            Player.Draw(gameTime);
            foreach (ObjRectange r in EnvironmentVariable.lRectangle)
            {
                if (r.Texture != TextureImg.Null)
                    r.Draw(gameTime, false);
            }

            spriteBatch.Begin();
            spriteBatch.DrawString(this.spriteFont, texte, new Vector2(this.graphics.Viewport.Width - taille.X, 5), Color.Green);
            spriteBatch.End();
        }
    }
}
