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
    public class Bullet
    {
        private ObjCercle bullet;
        private float radius;
        private ObjEdge liana;
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
            bullet = new ObjCercle(position, radius, 0, worldPhysic, BodyType.Dynamic);
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
            if (!isHung)
            //je joins la balle a la cible avec un angle strict, et la balle au joueur avec un distance joint
            {
                Vector2 diff = bullet.FixtureObject.Body.Position - fixB.Body.Position;
                //ObjCercle bulletTP = new ObjCercle(diff, radius, 0.0f, worldPhysic, fixB.Body);
                //worldPhysic.RemoveBody(bullet.FixtureObject.Body);
                //bullet = bulletTP;
                //bullet.addDisplayer();

                //juncture = new FarseerPhysics.Dynamics.Joints.DistanceJoint(juncturePoint.Body, bullet.FixtureObject.Body, new Vector2(1, 1)/*AnchorPointJuncturePoint*/, new Vector2(1, 1));
                //juncture = JointFactory.CreateDistanceJoint(worldPhysic, juncturePoint.Body, bullet.FixtureObject.Body, new Vector2(10,0)/*AnchorPointJuncturePoint*/, new Vector2(10,0));
                //worldPhysic.AddJoint(juncture); 


                //junctureHook = JointFactory. CreateRevoluteJoint(worldPhysic, fixA.Body, fixB.Body, diff);
                //junctureHook.MaxImpulse = 3;
                //junctureHook.

                worldPhysic.RemoveBody(bullet.FixtureObject.Body);
                bullet = null;
                //bullet.FixtureObject.Body.BodyType = BodyType.Dynamic;
                juncture = new FarseerPhysics.Dynamics.Joints.DistanceJoint(juncturePoint.Body, fixB.Body, AnchorPointJuncturePoint, new Vector2(diff.X, diff.Y));

                worldPhysic.AddJoint(juncture);
                isHung = true;
            }
            return false;
        }

        public void remove()
        {
            if (isHung)
            {
                if (bullet != null && bullet.FixtureObject != null)
                    bullet.FixtureObject.Body.FixtureList.Remove(bullet.FixtureObject);
                if (juncture != null)
                    worldPhysic.RemoveJoint(juncture);
            }
            else
                worldPhysic.RemoveBody(bullet.FixtureObject.Body);

            if (liana.FixtureObject != null)
                worldPhysic.RemoveBody(liana.FixtureObject.Body);

        }
        public void update(Vector2 posHead, float distance)
        {
            if(liana != null)
                worldPhysic.RemoveBody(liana.FixtureObject.Body);
            if(isHung)
                liana = new ObjEdge(juncture.WorldAnchorA, juncture.WorldAnchorB, worldPhysic);
            else
                liana = new ObjEdge(posHead, bullet.FixtureObject.Body.Position, worldPhysic);
            liana.FixtureObject.CollisionFilter.RemoveCollidesWithCategory(Category.All);
            liana.FixtureObject.Body.BodyType = BodyType.Dynamic;
            liana.addDisplayer();
        }

        public void Draw(GraphicsDevice Graphics)
        {
            if (liana != null)
                liana.Draw();
            if(bullet != null)
                bullet.Draw();
        }
    }
}
