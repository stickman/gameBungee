using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace gameBungee
{
    class CameraController
    {
        private Rectangle Border;
        private Matrix Proj;
        private Matrix View;
        private static BasicEffect Effect;

        private Vector3 camPosition;
        private Vector3 cameraLookat;

        public Matrix proj
        {
            get { return Proj; }
        }

        public Matrix view
        {
            get { return View; }
        }

        public BasicEffect effect
        {
            get { return Effect; }
        }

        public CameraController(Rectangle border, Matrix proj, Matrix view, GraphicsDevice Graphics)
        {
            cameraLookat = Vector3.Zero;
            camPosition = new Vector3(0, 0, 1);
            Border = border;
            Proj = proj;
            View = view;

            Effect = new BasicEffect(Graphics);
            Effect.VertexColorEnabled = true;
            Effect.View = view;
            Effect.Projection = proj;
        }
        public void update(Character player, GraphicsDevice graphics)
        {
            Vector2 positionTete = new Vector2();
            positionTete = player.ObjectPhysicCircleTete.FixtureObject.Body.Position;

            var onScreenPositon = graphics.Viewport.Project(new Vector3(positionTete, 0),
                                proj, view, Matrix.Identity);
            Vector3 rightAndDownShift = onScreenPositon - new Vector3(Border.X + Border.Width, Border.Y + Border.Height, 0);
            Vector3 leftAndUpShift = onScreenPositon - new Vector3(Border.X, Border.Y, 0);

            if (rightAndDownShift.X > 0)
            {
                camPosition = camPosition + new Vector3(1, 0, 0);
                Border.X += (int)camPosition.X;
            }
            if (rightAndDownShift.Y > 0)
            {
                //camPosition = new Vector3(0, 0, 1) + new Vector3(0, rightAndDownShift.Y, 0);
            }
            if (leftAndUpShift.X < 0)
            {
                camPosition = camPosition + new Vector3(-1, 0, 0);
                Border.X += -1;
            }
            if (leftAndUpShift.Y < 0)
            {
                //camPosition = new Vector3(0, 0, 1) + new Vector3(0, leftAndUpShift.Y, 0);
            }
            cameraLookat = camPosition - new Vector3(0, 0, 1);
            View = Matrix.CreateLookAt(camPosition, cameraLookat, new Vector3(0.0f, 1.0f, 0.0f));
            Effect.View = view;
            Effect.Projection = proj;
        }
    }
}
