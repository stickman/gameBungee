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
    static public class CameraController
    {
        static private Rectangle Border;
        static private BasicEffect Effect;

        static private Vector3 camPosition;
        static private Vector3 cameraLookat;

        static private Matrix Proj;
        static private Matrix View;

        static private int zoom;

        static public Matrix proj
        {
            get { return Proj; }
        }

        static public Matrix view
        {
            get { return View; }
        }

        static public BasicEffect effect
        {
            get { return Effect; }
        }

        static public void Inialized(Rectangle border, GraphicsDevice Graphics)
        {
            cameraLookat = Vector3.Zero;
            camPosition = new Vector3(0, 0, 1);
            Border = border;

            zoom = 4;

            Proj = Matrix.CreateOrthographic(EnvironmentVariable.width / zoom, EnvironmentVariable.height / zoom, 0, 1);
            View = Matrix.Identity;

            Effect = new BasicEffect(Graphics);
            Effect.VertexColorEnabled = true;
            Effect.View = view;
            Effect.Projection = proj;
        }

        static public void zoomOut()
        {
            zoom++;
            Proj = Matrix.CreateOrthographic(EnvironmentVariable.width / zoom, EnvironmentVariable.height / zoom, 0, 1);
        }

        static public void zoomIn()
        {
            zoom--;
            Proj = Matrix.CreateOrthographic(EnvironmentVariable.width / zoom, EnvironmentVariable.height / zoom, 0, 1); 
        }

        static public void update (Character player, GraphicsDevice graphics)
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
