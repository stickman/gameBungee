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
using FarseerPhysics.Dynamics.Contacts;

namespace gameBungee
{
    public class Object
    {
        //protected Vector2 Position;
        protected List<Fixture> ObjectPhysic = new List<Fixture>();
        protected AnimationObjet A_Animation;
        public AnimationCharacter A_AnimationChar;

        protected const Category PolygonCategory    = Category.Cat2;       //a changer avec un enum
        protected const Category RectangleCategory  = Category.Cat3;     //a changer avec un enum
        protected const Category CircleCategory     = Category.Cat4;        //a changer avec un enum
        protected const Category PointCategory      = Category.Cat5;        //a changer avec un enum

        protected Vector2 position;

        private Color color = new Color(0,0,0,255);

        public enum ObjectType {Cercle, Rectangle, Polygon, Point, Edge};
        protected ObjectType objType;

        public ObjectType objectType
        {
            get { return objType; }
        }

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }
        public Vector2 PositionInitial
        {
            get { return position; }
            set { position = value; }
        }

        public int FixtureID
        {
            get { return FixtureObject.FixtureId; }
        }
        
        public Vector2 Position
        {
            get { return ObjectPhysic.First().Body.Position; }
        }
        
        public Fixture FixtureObject
        {
            get { return ObjectPhysic.First(); }
        } 
        
        public BodyType Body_Type
        {
            get { return FixtureObject.Body.BodyType; }
            set { foreach (Fixture fix in ObjectPhysic) fix.Body.BodyType = value;}
        }

        public void addDisplayer()
        {
            A_Animation = new AnimationObjet();
            A_Animation.PlayAnimation(null, 0.0f, false);
        }

        public void addTexture(Texture2D tex, float frameTime)
        {
            if (A_AnimationChar == null)
                A_AnimationChar = new AnimationCharacter();
            if (A_AnimationChar.Animation == null)
            {
                Animation temp = new Animation(tex, frameTime, true);
                A_AnimationChar.PlayAnimation(tex, frameTime, true);
            }
            else
            {
                A_AnimationChar.Animation.effectPlayer.Texture = tex;
                A_AnimationChar.Animation.Texture = tex;

                A_AnimationChar.Animation.FrameTime = frameTime;
                A_AnimationChar.update();
            }
        }
        public void addTexture(TextureImg texImg, float frameTime)
        {
            Texture2D img = null;
            switch (texImg)
            {
                case TextureImg.Null:
                    break;
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
            if (A_AnimationChar == null)
                A_AnimationChar = new AnimationCharacter();
            if (A_AnimationChar.Animation == null)
            {
                Animation temp = new Animation(img, frameTime, true);
                A_AnimationChar.PlayAnimation(img, frameTime, true);
            }
            else
            {
                A_AnimationChar.Animation.effectPlayer.Texture = img;
                A_AnimationChar.Animation.Texture = img;

                A_AnimationChar.Animation.FrameTime = frameTime;
                A_AnimationChar.update();
            }
        }
        public void delete()
        {
                EnvironmentVariable.worldPhysic.RemoveBody(ObjectPhysic.Last().Body);
        }
    }

    public class ObjCercle : Object
    {
        private float radius;      
        
        public float Radius
        {
            get { return radius; }
            set { radius = value; }
        }
        
        public ObjCercle(Vector2 InitialPosition, float InitialRadius, float InitialAngle, World worldPhysic, Color color, BodyType bodyType = BodyType.Static)
        {
            objType = ObjectType.Cercle;
            this.ObjectPhysic.Add(FixtureFactory.CreateCircle(worldPhysic, InitialRadius, 1.0f, InitialPosition));
            ObjectPhysic.Last().Body.Rotation = InitialAngle;
            Body_Type = bodyType;
            //Give it some bounce and friction
            ObjectPhysic.Last().Restitution = 0.5f;
            ObjectPhysic.Last().Friction = 1.0f;

            if(bodyType != BodyType.Kinematic)
                ObjectPhysic.Last().CollisionFilter.CollisionCategories = CircleCategory;
            else
                ObjectPhysic.Last().CollisionFilter.CollidesWith = PointCategory;

            radius = InitialRadius;
            this.Body_Type = bodyType;
            PositionInitial = InitialPosition;
            this.Color = color;
        }
        public ObjCercle(Vector2 InitialPosition, float InitialRadius, float InitialAngle, World worldPhysic, BodyType bodyType = BodyType.Static)
        {
            objType = ObjectType.Cercle;
            this.ObjectPhysic.Add(FixtureFactory.CreateCircle(worldPhysic, InitialRadius, 1.0f, InitialPosition));
            ObjectPhysic.Last().Body.Rotation = InitialAngle;
            ObjectPhysic.Last().Body.BodyType = bodyType;
            //Give it some bounce and friction
            ObjectPhysic.Last().Restitution = 0.5f;
            ObjectPhysic.Last().Friction = 1.0f;

            if (bodyType != BodyType.Kinematic) 
                ObjectPhysic.Last().CollisionFilter.CollisionCategories = CircleCategory;
            else
                ObjectPhysic.Last().CollisionFilter.CollidesWith = PointCategory;

            radius = InitialRadius;
            Body_Type = bodyType;
            PositionInitial = InitialPosition;
            this.Color = Color.Black;
        }
        public ObjCercle(Vector2 InitialPosition, float InitialRadius, float InitialAngle, World worldPhysic, Body body)
        {
            objType = ObjectType.Cercle;
            ObjectPhysic.Add(FixtureFactory.CreateCircle(InitialRadius, 1.0f, body, InitialPosition));
            ObjectPhysic.First().Body.Rotation = InitialAngle;
            ObjectPhysic.First().Body.BodyType = BodyType.Dynamic;
            Body_Type = BodyType.Static;
            //Give it some bounce and friction
            ObjectPhysic.First().Restitution = 0.5f;
            ObjectPhysic.First().Friction = 1.0f;
            ObjectPhysic.First().CollisionFilter.CollisionCategories = CircleCategory;

            //Position = InitialPosition;
            radius = InitialRadius;
        }
        public void Draw()
        {            
                A_Animation.DrawCircle(ObjectPhysic.First(), this.Color);
        }
    }
    
    public class ObjRectange : Object
    {
        Vector2 size;
        float angle;
        float angleInit;

        private TextureImg img = TextureImg.Null;
        public TextureImg Texture
        {
            get { return img; }
            set { img = value; }
        }

        new public Vector2 PositionInitial
        {
            get { return position-Size/2; }
            set { position = value + Size/2; }
        }

        public float Angle
        {
            get { return angleInit; }
            set { angleInit = value;
                  ObjectPhysic.Last().Body.Rotation = value * (float)Math.PI / 180.0f; }
        }
        
        public Vector2 Size
        {
            get { return size; }
            set { size = value; }
        }
        public ObjRectange(Vector2 InitialPosition, float width, float height, float InitialAngle, BodyType bodyType)
        {
            objType = ObjectType.Rectangle;
            angle = InitialAngle;
            ObjectPhysic.Add(FixtureFactory.CreateRectangle(EnvironmentVariable.worldPhysic, width, height, 1.0f, InitialPosition));
            ObjectPhysic.Last().Body.Rotation = InitialAngle * (float)Math.PI / 180.0f;
            //Give it some bounce and friction
            ObjectPhysic.Last().Restitution = 0.0f;
            ObjectPhysic.Last().Friction = 1.0f;
            Color = new Color(0, 0, 0, 255);

            if (bodyType != BodyType.Kinematic)
                ObjectPhysic.Last().CollisionFilter.CollisionCategories = RectangleCategory;
            else
            {
                ObjectPhysic.Last().CollisionFilter.CollidesWith = PointCategory;
            }
            ObjectPhysic.Last().Body.BodyType = bodyType;
            Body_Type = bodyType;

            ObjectPhysic.Last().CollisionFilter.CollisionCategories = RectangleCategory;
            PositionInitial = InitialPosition;
            angleInit = InitialAngle;
            size.X = width;
            size.Y = height;
            
        }
        public ObjRectange(Vector2 InitialPosition, float width, float height, float InitialAngle, BodyType BodyType, Body body)
        {
            objType = ObjectType.Rectangle;
            ObjectPhysic.Add(FixtureFactory.CreateRectangle(width, height, 1.0f, InitialPosition, body));
            ObjectPhysic.First().Body.Rotation = InitialAngle * (float)Math.PI / 180.0f;
            //Give it some bounce and friction
            ObjectPhysic.First().Restitution = 0.0f;
            ObjectPhysic.First().Friction = 1.0f;
            Body_Type = BodyType;
            ObjectPhysic.First().CollisionFilter.CollisionCategories = RectangleCategory;

            angleInit = InitialAngle;
            size.X = width;
            size.Y = height;
        }

        public void Draw()
        {
            A_Animation.Draw(ObjectPhysic,Color);
        }
        public void Draw(GameTime gameTime, Boolean Flip)
        {
            if (A_AnimationChar != null)
                A_AnimationChar.Draw(gameTime, ObjectPhysic.Last(), size, Flip);
        }
    }

    public class ObjPolygon : Object
    {
        List<Vector2> vertlist;

        public List<Vector2> VerticesList
        {
            get { return vertlist; }
        }
        public List<Fixture> FixtureList
        {
            get { return ObjectPhysic; }
        }

        public ObjPolygon(List<Vector2> vertlist, BodyType bodyType, World worldPhysic)
        {
            objType = ObjectType.Polygon;
            this.vertlist = vertlist;
            FarseerPhysics.Common.Vertices vert = new FarseerPhysics.Common.Vertices(vertlist);
            List<FarseerPhysics.Common.Vertices> verts = EarclipDecomposer.ConvexPartition(vert);
            ObjectPhysic = FixtureFactory.CreateCompoundPolygon(worldPhysic, verts, 1.0f);
            Body_Type = bodyType;

            foreach (Fixture fixt in ObjectPhysic)
            {
                fixt.Body.BodyType = bodyType;
                if (bodyType != BodyType.Kinematic)
                    fixt.CollisionFilter.CollisionCategories = PolygonCategory;
                else
                {
                    fixt.CollisionFilter.CollisionCategories = PolygonCategory;
                    fixt.CollisionFilter.CollidesWith = PointCategory;
                }
            }
        }

        public void setFriction(float friction)
        {
            foreach (Fixture fix in ObjectPhysic)
            {
                fix.Friction = friction;
            }
        }
        public void Draw()
        {
            A_Animation.Draw(ObjectPhysic, Color);
        }
    }

    public class ObjEdge : Object
    {
        public ObjEdge(Vector2 start, Vector2 end, World worldPhysic)
        {
            objType = ObjectType.Edge;
            ObjectPhysic.Add(FixtureFactory.CreateEdge(worldPhysic, start, end));
            ObjectPhysic.First().Body.BodyType = BodyType.Kinematic;
        }
        public void Draw()
        {
            A_Animation.DrawEdge(ObjectPhysic);
        }
    }

    public class ObjPoint : Object
    {
        private Fixture fixureCollidesWith;
        public Object objCollidesWith;
        public ObjPoint(Vector2 InitialPosition, float InitialRadius)
        {
            objType = ObjectType.Point;
            this.ObjectPhysic.Add(FixtureFactory.CreateCircle(EnvironmentVariable.worldPhysic, InitialRadius, 1.0f, InitialPosition));
            ObjectPhysic.Last().Body.BodyType = BodyType.Dynamic;
            ObjectPhysic.Last().CollisionFilter.AddCollisionCategory(PointCategory);
            ObjectPhysic.Last().OnCollision += new OnCollisionEventHandler(CollisionPointMouse);
        }

        public bool CollisionPointMouse(Fixture obj1, Fixture obj2, Contact contact)
        {
            if (fixureCollidesWith == null || fixureCollidesWith.ShapeType == ShapeType.Circle)
            {
                if (contact.FixtureA.FixtureId == this.FixtureObject.FixtureId)
                    fixureCollidesWith = contact.FixtureB;
                else
                    fixureCollidesWith = contact.FixtureA;
                foreach (ObjCercle c in EnvironmentVariable.lCercle)
                {
                    if (c.FixtureObject.FixtureId == fixureCollidesWith.FixtureId)
                    {
                        objCollidesWith = c;
                        return false;
                    }
                } 
                foreach (ObjRectange r in EnvironmentVariable.lRectangle)
                {
                    if (r.FixtureObject.FixtureId == fixureCollidesWith.FixtureId)
                    {
                        objCollidesWith = r;
                        return false;
                    }
                }
                foreach (ObjPolygon p in EnvironmentVariable.lPolygon)
                {
                    foreach (Fixture fix in p.FixtureList)
                    {
                        if (fix.FixtureId == fixureCollidesWith.FixtureId)
                        {
                            objCollidesWith = p;
                            return false;
                        }
                    }
                }
            }
            return false;
        }
    }
}
