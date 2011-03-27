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
            camPosition = new Vector3(0, 0, 1);
            Border = border;
            Proj = proj;
            View = view;

            Effect = new BasicEffect(Graphics);
            Effect.VertexColorEnabled = true;
            Effect.View = view;
            Effect.Projection = proj;
        }
        public void update(Character player, GraphicsDevice Graphics)
        {
            camPosition = new Vector3(0, 0, 1) + new Vector3(player.ObjectPhysicCircleTete.Position.X, 0, 0);
            Vector3 cameraLookat = camPosition - new Vector3(0,0,1);
            View = Matrix.CreateLookAt(camPosition, cameraLookat, new Vector3(0.0f, 1.0f, 0.0f)); 
            Effect.View = view;
            Effect.Projection = proj;
        }
    }
}
