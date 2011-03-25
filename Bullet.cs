using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Contacts;

namespace gameBungee
{
    class Bullet
    {
        private ObjCercle bullet;
        private float radius;
        private ObjEdge liana;
        //public FarseerPhysics.Dynamics.Joints.SliderJoint juncture; //la jointure avec un objet
        public FarseerPhysics.Dynamics.Joints.DistanceJoint juncture; //la jointure avec un objet
        private Fixture juncturePoint;

        private Vector2 AnchorPointJuncturePoint = Vector2.Zero;
        private Vector2 AnchorPointBullet = new Vector2(0, 0);
        private const float minLength = 0.0f;
        private const float maxLength = 20.0f;

        public bool isHung = false;

        World worldPhysic;

        public Vector2 positionBullet
        {
            get { return bullet.FixtureObject.Body.Position; }
        }

        public Bullet(Vector2 position, float radius, Fixture juncturePoint, World worldPhysic)
        {
            this.worldPhysic = worldPhysic;
            this.radius = radius;
            bullet = new ObjCercle(position, radius, 0, worldPhysic);
            bullet.addDisplayer();
            bullet.FixtureObject.OnCollision = new OnCollisionEventHandler(collisionBullet);
            this.juncturePoint = juncturePoint;
        }

        public void shootBullet(bool Flip, double angleTir)
        {
            isHung = false;
            if (Flip)
            {
                bullet.FixtureObject.CollisionFilter.CollisionCategories = Category.Cat30;
                bullet.FixtureObject.CollisionFilter.RemoveCollidesWithCategory(Category.Cat20);
                bullet.FixtureObject.Body.IgnoreGravity = true;
                bullet.FixtureObject.Body.ApplyForce(100000 * new Vector2((float)Math.Cos(angleTir), (float)Math.Sin(angleTir)));
            }
            else
            {
                bullet.FixtureObject.CollisionFilter.CollisionCategories = Category.Cat30;
                bullet.FixtureObject.CollisionFilter.RemoveCollidesWithCategory(Category.Cat20);
                bullet.FixtureObject.Body.IgnoreGravity = true;
                bullet.FixtureObject.Body.ApplyForce(100000 * new Vector2((float)Math.Cos(angleTir + Math.PI), (float)Math.Sin(angleTir + Math.PI)));
            }
        }

        public Boolean collisionBullet(Fixture fixA, Fixture fixB, Contact contact)
        {
            bullet.FixtureObject.Body.BodyType = BodyType.Static;
            //juncture = JointFactory.CreateSliderJoint(worldPhysic, juncturePoint.Body, bullet.FixtureObject.Body, AnchorPointJuncturePoint, AnchorPointBullet, minLength, maxLength);
            juncture = JointFactory.CreateDistanceJoint(worldPhysic, juncturePoint.Body, bullet.FixtureObject.Body, AnchorPointJuncturePoint, AnchorPointBullet);
            isHung = true;
            return false;
        }

        public void remove()
        {
            if(bullet.FixtureObject != null)
                worldPhysic.RemoveBody(bullet.FixtureObject.Body);
            if (liana.FixtureObject != null)
                worldPhysic.RemoveBody(liana.FixtureObject.Body);
        }
        public void update(Vector2 posHead, float distance)
        {
            if(liana != null)
                worldPhysic.RemoveBody(liana.FixtureObject.Body);
            liana = new ObjEdge(posHead, bullet.FixtureObject.Body.Position, worldPhysic);
            liana.FixtureObject.CollisionFilter.RemoveCollidesWithCategory(Category.All);
            liana.addDisplayer();
            if (juncture != null)
            {
                juncture.Length = distance;
                //juncture.MinLength = distance;
                //juncture.MaxLength = distance;
            }
        }

        public void Draw(GraphicsDevice Graphics)
        {
            if (liana != null)
                liana.Draw(Graphics);
            bullet.Draw(Graphics);
        }
    }
}
