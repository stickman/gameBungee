using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using FarseerPhysics;
using FarseerPhysics.Common;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;

namespace gameBungee
{
    class Object
    {
        //protected Vector2 Position;
        protected List<Fixture> ObjectPhysic = new List<Fixture>();
        protected AnimationObjet A_Animation;
        public AnimationCharacter A_AnimationChar;

        protected const Category PolygonCategory = Category.Cat2;
        protected const Category RectangleCategory = Category.Cat3;
        protected const Category CircleCategory = Category.Cat4;

        public Vector2 Position
        {
            get { return ObjectPhysic.First().Body.Position; }
        }

        public Fixture FixtureObject
        {
            get { return ObjectPhysic.First(); }
        }

        public void addDisplayer()
        {
            Animation temp = new Animation(null, 0.0f, false);
            A_Animation = new AnimationObjet();
            A_Animation.PlayAnimation(temp);
        }

        public void addTexture(Texture2D tex, BasicEffect effectPlayer, float frameTime)
        {
            Animation temp = new Animation(tex, frameTime, true);
            effectPlayer.Texture = tex;
            if( A_AnimationChar == null)
                A_AnimationChar = new AnimationCharacter(effectPlayer);
            A_AnimationChar.PlayAnimation(temp);
        }
    }

    class ObjCercle : Object
    {
        private float radius;

        public ObjCercle(Vector2 InitialPosition, float InitialRadius, float InitialAngle, World worldPhysic)
        {
            ObjectPhysic.Add( FixtureFactory.CreateCircle(worldPhysic, InitialRadius, 1.0f, InitialPosition));
            ObjectPhysic.First().Body.Rotation = InitialAngle;
            ObjectPhysic.First().Body.BodyType = BodyType.Dynamic;
            //Give it some bounce and friction
            ObjectPhysic.First().Restitution = 0.5f;
            ObjectPhysic.First().Friction = 1.0f;
            ObjectPhysic.First().CollisionFilter.CollisionCategories = CircleCategory;

            //Position = InitialPosition;
            radius = InitialRadius;
        }
        public ObjCercle(Vector2 InitialPosition, float InitialRadius, float InitialAngle, World worldPhysic, Body body)
        {
            ObjectPhysic.Add(FixtureFactory.CreateCircle(InitialRadius, 1.0f, body, InitialPosition));
            ObjectPhysic.First().Body.Rotation = InitialAngle;
            ObjectPhysic.First().Body.BodyType = BodyType.Dynamic;
            //Give it some bounce and friction
            ObjectPhysic.First().Restitution = 0.5f;
            ObjectPhysic.First().Friction = 1.0f;
            ObjectPhysic.First().CollisionFilter.CollisionCategories = CircleCategory;

            //Position = InitialPosition;
            radius = InitialRadius;
        }
        public void Draw(GraphicsDevice Graphics)
        {            
                A_Animation.DrawCircle(ObjectPhysic.First(), Graphics);
        }
    }

    class ObjRectange : Object
    {
        //new public List<Fixture> ObjectPhysic = new List<Fixture>();
        Vector2 size;
        public ObjRectange(Vector2 InitialPosition, float width, float height, float InitialAngle, World worldPhysic, BodyType BodyType)
        {
            ObjectPhysic.Add(FixtureFactory.CreateRectangle(worldPhysic, width, height, 1.0f, InitialPosition));
            ObjectPhysic.First().Body.Rotation = InitialAngle;
            //Give it some bounce and friction
            ObjectPhysic.First().Restitution = 0.0f;
            ObjectPhysic.First().Friction = 1.0f;
            ObjectPhysic.First().Body.BodyType = BodyType;
            ObjectPhysic.First().CollisionFilter.CollisionCategories = RectangleCategory;

            size.X = width;
            size.Y = height;
        }
        public ObjRectange(Vector2 InitialPosition, float width, float height, float InitialAngle, BodyType BodyType, Body body)
        {
            ObjectPhysic.Add(FixtureFactory.CreateRectangle(width, height, 1.0f, InitialPosition, body));
            ObjectPhysic.First().Body.Rotation = InitialAngle;
            //Give it some bounce and friction
            ObjectPhysic.First().Restitution = 0.0f;
            ObjectPhysic.First().Friction = 1.0f;
            ObjectPhysic.First().Body.BodyType = BodyType;
            ObjectPhysic.First().CollisionFilter.CollisionCategories = RectangleCategory;

            size.X = width;
            size.Y = height;
        }

        public void Draw(GraphicsDevice Graphics)
        {
            A_Animation.Draw(ObjectPhysic, Graphics);
        }
        public void Draw(GameTime gameTime, GraphicsDevice Graphics, Boolean Flip)
        {
            A_AnimationChar.Draw(gameTime, ObjectPhysic.First(), Graphics, Flip);
        }
    }

    class ObjPolygon : Object
    {
        //new List<Fixture> ObjectPhysic = new List<Fixture>();

        public ObjPolygon(List<Vector2> vertlist, BodyType bodyType, World worldPhysic)
        {
            FarseerPhysics.Common.Vertices vert = new FarseerPhysics.Common.Vertices(vertlist);
            List<FarseerPhysics.Common.Vertices> verts = EarclipDecomposer.ConvexPartition(vert);
            ObjectPhysic = FixtureFactory.CreateCompoundPolygon(worldPhysic, verts, 1.0f);

            foreach (Fixture fixt in ObjectPhysic)
            {
                fixt.Body.BodyType = bodyType;
                fixt.CollisionFilter.CollisionCategories = PolygonCategory;
            }
        }

        public void setFriction(float friction)
        {
            foreach (Fixture fix in ObjectPhysic)
            {
                fix.Friction = friction;
            }
        }


        public void Draw(GraphicsDevice Graphics)
        {
            A_Animation.Draw(ObjectPhysic, Graphics);
        }
    }

    class ObjEdge : Object
    {
        public ObjEdge(Vector2 start, Vector2 end, World worldPhysic)
        {
            ObjectPhysic.Add(FixtureFactory.CreateEdge(worldPhysic, start, end));
            ObjectPhysic.First().Body.BodyType = BodyType.Kinematic;
        }
        public void Draw(GraphicsDevice Graphics)
        {
            A_Animation.DrawEdge(ObjectPhysic, Graphics);
        }
    }
}
