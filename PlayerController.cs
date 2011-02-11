using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using FarseerPhysics;
using FarseerPhysics.Collision;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;

namespace gameBungee
{
    class PlayerController
    {

        GamePadState gamePadState ;
        KeyboardState keyboardState;
        MouseState mouseState;

        Character Player;

        public void UpdateInteraction(Character player, GraphicsDevice graphics, Matrix view, Matrix proj)
        {
            gamePadState = GamePad.GetState(PlayerIndex.One);
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            Player = player;

            Player.oldMoving        = Player.isMoving;
            Player.oldMovingRight   = Player.isMovingRight;
            Player.oldJumping       = Player.isJumping;
            Player.oldShooting      = Player.isShooting;

            Player.isMoving         = false;
            Player.isMovingRight    = false;
            Player.isJumping        = false;
            Player.isShooting       = false;

            if (gamePadState.IsButtonDown(Buttons.DPadLeft) ||
                keyboardState.IsKeyDown(Keys.Left) ||
                keyboardState.IsKeyDown(Keys.Q))
            {
                InteractionLeft();

                Player.isMoving         = true;
                Player.isMovingRight    = false;
                Player.isJumping        = false;

            }
            if (gamePadState.IsButtonDown(Buttons.DPadRight) ||
                 keyboardState.IsKeyDown(Keys.Right) ||
                 keyboardState.IsKeyDown(Keys.D))
            {
                InteractionRight();

                Player.isMoving         = true;
                Player.isMovingRight    = true;
                Player.isJumping        = false;
            }
            if (gamePadState.IsButtonDown(Buttons.A) ||
                 keyboardState.IsKeyDown(Keys.Space) ||
                 keyboardState.IsKeyDown(Keys.Z))
            {
                InteractionButtonA();

                Player.isMoving         = true;
                Player.isJumping        = true;
            }
            if (!(gamePadState.IsButtonDown(Buttons.DPadRight) ||
                 keyboardState.IsKeyDown(Keys.Right) ||
                 keyboardState.IsKeyDown(Keys.D)) && !(gamePadState.IsButtonDown(Buttons.DPadLeft) ||
                keyboardState.IsKeyDown(Keys.Left) ||
                keyboardState.IsKeyDown(Keys.Q)))
            {
                Vector2 t1 = -Player.PlayerPhysic.FixtureObject.Body.LinearVelocity;
                Vector2 t2 = new Vector2(0.0f, 0.0f);
                //Player.PlayerPhysic.FixtureObject.Body.ApplyForce(ref t1, ref t2);
                if (Player.PlayerPhysic.FixtureObject.Body.LinearVelocity.Y > -0.01f && Player.PlayerPhysic.FixtureObject.Body.LinearVelocity.Y < 0.01f)
                    Player.PlayerPhysic.FixtureObject.Body.ResetDynamics();
                
                Player.isMoving         = false;
                Player.isMovingRight    = false;
                Player.isJumping        = false;
            }
            if (!gamePadState.IsConnected)
            {
                Vector2 positionTete = new Vector2();
                positionTete = Player.ObjectPhysicCircleTete.FixtureObject.Body.Position;

                var screenPositon = graphics.Viewport.Project(new Vector3(positionTete, 0),
                                    proj, view, Matrix.Identity);
                positionTete.X = screenPositon.X;
                positionTete.Y = screenPositon.Y;
                Vector2 diff = new Vector2(mouseState.X - positionTete.X, mouseState.Y - positionTete.Y);

                if(diff.X < 0)
                    Player.isMovingRight = false;
                else
                    Player.isMovingRight = true;

                Player.angleTir = -Math.Atan(diff.Y / diff.X);
                Player.distanceTir = Math.Sqrt(diff.X * diff.X + diff.Y * diff.Y);
                /*
                 * Vector2 positionTete = new Vector2();
                positionTete = Player.ObjectPhysicCircleTete.FixtureObject.Body.Position;

                Matrix proj = Matrix.CreateOrthographic(800, 480, 0, 1);
                Matrix view = Matrix.Identity;

                var screenPositon = graphics.Viewport.Project(new Vector3(positionTete, 0),
                                    proj, view, Matrix.Identity);
                positionTete.X = screenPositon.X;
                positionTete.Y = screenPositon.Y;
                Vector2 diff = new Vector2(mouseState.X - positionTete.X, positionTete.Y - mouseState.Y);

                Player.angleTir = Math.Atan(diff.Y / diff.X);
                Player.distanceTir = Math.Sqrt(diff.X * diff.X + diff.Y * diff.Y);*/
            }
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                Player.isShooting = true;
            }
            if (mouseState.RightButton == ButtonState.Pressed)
            {
                Player.isShooting = false;
                if (Player.bulletJuncture != null)
                {
                    Player.bulletJuncture.remove();
                    Player.bulletJuncture = null;
                }
            }
        }
        
        private void InteractionLeft()
        {
            if (Player.PlayerPhysic.FixtureObject.Body.LinearVelocity.X > -40)//speed max is limited to 40
            {
                Contact contact = null;
                if (Player.ObjectPhysicCircle.FixtureObject.Body.ContactList != null)
                    contact = Player.ObjectPhysicCircle.FixtureObject.Body.ContactList.Contact;
                bool isOnContact = false;
                while (contact != null && isOnContact != true)
                {
                    if ((contact.FixtureA.FixtureId == Player.ObjectPhysicCircle.FixtureObject.FixtureId
                     || contact.FixtureB.FixtureId == Player.ObjectPhysicCircle.FixtureObject.FixtureId) && contact.IsTouching())
                    {
                        Vector2 t1 = new Vector2(-50000.0f, 0.0f);
                        Player.PlayerPhysic.FixtureObject.Body.ApplyForce(t1);
                        Player.PlayerPhysic.FixtureObject.Body.Awake = true;
                        isOnContact = true;
                    }
                    contact = contact.Next;
                }
                if (isOnContact == false)//si le joueur n'est pas en contact alors il peut orienter sa chute
                {
                    if (Player.ObjectPhysicCircleTete.FixtureObject.Body.ContactList != null)//Check head touch something
                        contact = Player.ObjectPhysicCircleTete.FixtureObject.Body.ContactList.Contact;
                    while (contact != null && isOnContact != true)//Player's head is in contact with the world?
                    {
                        if ((contact.FixtureA.FixtureId == Player.ObjectPhysicCircleTete.FixtureObject.FixtureId
                         || contact.FixtureB.FixtureId == Player.ObjectPhysicCircleTete.FixtureObject.FixtureId) && contact.IsTouching())
                        //permet de ne pas prendre en compte le déplacement si celui ci a un obstacle
                        {
                            isOnContact = true;
                        }
                        contact = contact.Next;
                    }
                    if (!isOnContact)
                    {
                        Vector2 t1 = new Vector2(-10000.0f, 0.0f);
                        Player.PlayerPhysic.FixtureObject.Body.ApplyForce(t1);
                        Player.PlayerPhysic.FixtureObject.Body.Awake = true;
                    }
                }
            }
        }
        
        private void InteractionRight()
        {
            if (Player.PlayerPhysic.FixtureObject.Body.LinearVelocity.X < 40)//speed max is limited to 40
            {
                Contact contact = null;
                if (Player.ObjectPhysicCircle.FixtureObject.Body.ContactList != null)
                    contact = Player.ObjectPhysicCircle.FixtureObject.Body.ContactList.Contact;
                bool isOnContact = false;
                while (contact != null && isOnContact != true)
                {
                    if ((contact.FixtureA.FixtureId == Player.ObjectPhysicCircle.FixtureObject.FixtureId
                     || contact.FixtureB.FixtureId == Player.ObjectPhysicCircle.FixtureObject.FixtureId) && contact.IsTouching())
                        //permet de ne pas prendre en compte le déplacement si celui ci a un obstacle
                    {
                        Vector2 t1 = new Vector2(50000.0f, 0.0f);
                        Player.PlayerPhysic.FixtureObject.Body.ApplyForce(t1);
                        Player.PlayerPhysic.FixtureObject.Body.Awake = true;
                        isOnContact = true;
                    }
                    contact = contact.Next;
                }

                if (isOnContact == false)
                {
                    if (Player.ObjectPhysicCircleTete.FixtureObject.Body.ContactList != null)//Check head touch something
                        contact = Player.ObjectPhysicCircleTete.FixtureObject.Body.ContactList.Contact;
                    while (contact != null && isOnContact != true)//Player's head is in contact with the world?
                    {
                        if ((contact.FixtureA.FixtureId == Player.ObjectPhysicCircleTete.FixtureObject.FixtureId
                         || contact.FixtureB.FixtureId == Player.ObjectPhysicCircleTete.FixtureObject.FixtureId) && contact.IsTouching())
                        {
                            isOnContact = true;
                        }
                        contact = contact.Next;
                    }
                    if (!isOnContact)
                    {
                        Vector2 t1 = new Vector2(10000.0f, 0.0f);
                        Player.PlayerPhysic.FixtureObject.Body.ApplyForce(t1);
                        Player.PlayerPhysic.FixtureObject.Body.Awake = true;
                    }
                }
            }
        }

        private void InteractionButtonA()
        {
            Contact contact = null;
            bool temp1 = Player.ObjectPhysicCircle.FixtureObject.CollisionFilter.IsInCollisionCategory(Category.Cat3);
            bool temp2 = Player.ObjectPhysicCircle.FixtureObject.CollisionFilter.IsInCollidesWithCategory(Category.Cat9);

            if (Player.ObjectPhysicCircle.FixtureObject.Body.ContactList != null)
                contact = Player.ObjectPhysicCircle.FixtureObject.Body.ContactList.Contact;
            
            bool isOnContact = false;

            while (contact != null && isOnContact != true)
            {
                if(contact.FixtureA.CollisionFilter.IsInCollidesWithCategory(contact.FixtureB.CollisionFilter.CollisionCategories))
                    {
                        if ((contact.FixtureA.FixtureId == Player.ObjectPhysicCircle.FixtureObject.FixtureId
                         || contact.FixtureB.FixtureId == Player.ObjectPhysicCircle.FixtureObject.FixtureId) && contact.IsTouching())
                        {
                            Vector2 t1 = new Vector2(0.0f, 6000.0f);
                            Vector2 t2 = new Vector2(0.0f, 0.0f);
                            Player.PlayerPhysic.FixtureObject.Body.ApplyLinearImpulse(ref t1, ref t2);
                            Player.PlayerPhysic.FixtureObject.Body.Awake = true;
                            isOnContact = true;
                        }
                    }
                contact = contact.Next;
            }
            if(!isOnContact)
            {
                if (Player.bulletJuncture != null && Player.bulletJuncture.isHung)
                {
                    Vector2 t1 = new Vector2(0.0f, 6000.0f);
                    Vector2 t2 = new Vector2(0.0f, 0.0f);
                    Player.PlayerPhysic.FixtureObject.Body.ApplyLinearImpulse(ref t1, ref t2);
                    Player.PlayerPhysic.FixtureObject.Body.Awake = true;
                }
            }
        }

    }
}
