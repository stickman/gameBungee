using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using FarseerPhysics;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision;
using FarseerPhysics.Collision.Shapes;

namespace gameBungee
{
    public class AnimationManager
    {
        protected Animation Anim;
        protected int i_FrameCurrent;
        protected int i_nbrFrame;
        protected float f_Time;         //permet de savoir combien de temps c'est écoulé pour afficher la frame correspondante
        protected Vector2 v2_Origin = new Vector2(0, 0);
        

        public Animation Animation
        {
            get { return Anim; }
        }

        public int FrameCurrent
        {
            get { return i_FrameCurrent; }
            private set { i_FrameCurrent = value; }
        }

        public float Time
        {
            get { return f_Time; }
            private set { f_Time = value; }
        }

        public Vector2 Origin
        {
            get { return v2_Origin; }
        }

        public void PlayAnimation(Animation anim)
        {
            if (Anim == anim) //Si on a deja chargé la bonne animation
                return;

            this.Anim = anim;
            this.i_FrameCurrent = 0;
            this.f_Time = 0.0f;
            if (anim.Texture != null)
                this.i_nbrFrame = anim.Texture.Width / anim.Texture.Height;
        }
    }

    public class AnimationCharacter : AnimationManager
    {
        public BasicEffect effectPlayer;

        public AnimationCharacter(BasicEffect _effectPlayer)
        {
            effectPlayer = _effectPlayer;
        }
        public void Draw(GameTime gameTime, Fixture ObjectPhysic, Boolean Flip)
        {
            if (Animation == null)
                throw new NotSupportedException("[ERROR AnimationCharacter] Pas d'animation chargée...");

            f_Time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            while (f_Time > Animation.FrameTime && Animation.FrameTime != 0.0f)
            {
                f_Time -= Animation.FrameTime;

                // Advance the frame index; looping or clamping as appropriate.
                if (Animation.IsLooping)
                {
                    i_FrameCurrent = (FrameCurrent + 1) % Animation.Nbr_Frame;
                }
                else
                {
                    i_FrameCurrent = Math.Min(FrameCurrent + 1, Animation.Nbr_Frame - 1);
                }
            }
            
            //Rectangle source = new Rectangle(FrameCurrent * Animation.Texture.Height, 0, Animation.Texture.Height, Animation.Texture.Height);
            //spriteBatch.Draw(Animation.Texture, position, source, Color.White, angleTir, Origin, 1.0f, spriteEffects, 0.0f);
            //effectPlayer.Texture = Anim.Texture;

            Vector3 vectoreUp = new Vector3((float)Math.Cos(ObjectPhysic.Body.Rotation + Math.PI / 2), (float)Math.Sin(ObjectPhysic.Body.Rotation + Math.PI / 2), 0f);

            Quad quad = new Quad(new Vector3(ObjectPhysic.Body.Position, 0), Vector3.Backward, vectoreUp, 10, 10, i_FrameCurrent, this.i_nbrFrame, Flip);
            
            effectPlayer.Techniques[0].Passes[0].Apply();
            EnvironmentVariable.graphics.GraphicsDevice.DrawUserIndexedPrimitives
                    <VertexPositionNormalTexture>(PrimitiveType.TriangleList,
                    quad.Vertices, 0, 4,
                    quad.Indexes, 0, 2);
        }
    }

    public class AnimationObjet : AnimationManager
    {
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffects, float angleTir, Rectangle source)
        {
            if (Animation == null)
                throw new NotSupportedException("[ERROR AnimationCharacter] Pas d'animation chargée...");

            f_Time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            while (Time > Animation.FrameTime)
            {
                f_Time -= Animation.FrameTime;

                // Advance the frame index; looping or clamping as appropriate.
                if (Animation.IsLooping)
                {
                    i_FrameCurrent = (FrameCurrent + 1) % Animation.Nbr_Frame;
                }
                else
                {
                    i_FrameCurrent = Math.Min(FrameCurrent + 1, Animation.Nbr_Frame - 1);
                }
            }
            spriteBatch.Draw(Animation.Texture, position, source, Color.White, angleTir, new Vector2((float)source.Width / 2.0f, (float)source.Height / 2.0f), 1.0f, spriteEffects, 0.0f);
        }
        
        public void DrawCircle(Fixture ObjectPhysic)
        {
            CircleShape circle = (CircleShape)ObjectPhysic.Shape;
            Transform xf;
            ObjectPhysic.Body.GetTransform(out xf);
            Vector2 center = MathUtils.Multiply(ref xf, circle.Position);
            float radius = circle.Radius;

            const int segments = 32;
            const double increment = Math.PI * 2.0 / segments;
            double theta = 0.0;

            Color colorFill = new Color(0.9f, 0.7f, 0.7f, 0.5f);

            Vector2 v0 = center + radius * new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta));
            theta += increment;

            VertexPositionColor[] _vertsFill = new VertexPositionColor[(segments-2)*3];
            for (int i = 1; i < segments - 1; i++)
            {
                Vector2 v1 = center + radius * new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta));
                Vector2 v2 = center +
                             radius *
                             new Vector2((float)Math.Cos(theta + increment), (float)Math.Sin(theta + increment));

                _vertsFill[(i-1) * 3].Position = new Vector3(v0, 0.0f);
                _vertsFill[(i - 1) * 3].Color = colorFill;

                _vertsFill[(i - 1) * 3 + 1].Position = new Vector3(v1, 0.0f);
                _vertsFill[(i - 1) * 3 + 1].Color = colorFill;

                _vertsFill[(i - 1) * 3 + 2].Position = new Vector3(v2, 0.0f);
                _vertsFill[(i - 1) * 3 + 2].Color = colorFill;

                theta += increment;
            }
            EnvironmentVariable.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, _vertsFill, 0, (_vertsFill.Count() / 3) - 2);
        }

        public void DrawEdge(List<Fixture> ObjectPhysicList)
        {
            Color colorFill = new Color(0.9f, 0.7f, 0.7f, 0.5f);
            VertexPositionColor[] _vertsFill = new VertexPositionColor[4];
            foreach (Fixture ObjectPhysic in ObjectPhysicList)
            {
                EdgeShape poly = (EdgeShape)ObjectPhysic.Shape;
                Transform xf;
                ObjectPhysic.Body.GetTransform(out xf);
                _vertsFill[0].Position = new Vector3(poly.Vertex1, 0.0f);
                _vertsFill[0].Color = colorFill;
                _vertsFill[1].Position = new Vector3(poly.Vertex2, 0.0f);
                _vertsFill[1].Color = colorFill;
           }
            EnvironmentVariable.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, _vertsFill, 0, 2);        
        }

        public void Draw(List<Fixture> ObjectPhysicList)
        {
            foreach (Fixture ObjectPhysic in ObjectPhysicList)
            {
                PolygonShape poly = (PolygonShape)ObjectPhysic.Shape;
                Transform xf;
                ObjectPhysic.Body.GetTransform(out xf);
                VertexPositionColor[] _vertsFill = new VertexPositionColor[(poly.Vertices.Count - 2) * 3];

                for (int i = 0; i < poly.Vertices.Count - 2; i++)
                {
                    _vertsFill[3 * i].Position = new Vector3(MathUtils.Multiply(ref xf, poly.Vertices[0]), 0);
                    _vertsFill[3 * i].Color = new Color(0.9f, 0.7f, 0.7f);

                    _vertsFill[3 * i + 1].Position = new Vector3(MathUtils.Multiply(ref xf, poly.Vertices[i + 1]), 0);
                    _vertsFill[3 * i + 1].Color = new Color(0.9f, 0.7f, 0.7f);

                    _vertsFill[3 * i + 2].Position = new Vector3(MathUtils.Multiply(ref xf, poly.Vertices[i + 2]), 0);
                    _vertsFill[3 * i + 2].Color = new Color(0.9f, 0.7f, 0.7f);
                }
                EnvironmentVariable.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, _vertsFill, 0, poly.Vertices.Count - 2);
            }
        }
    }
}
