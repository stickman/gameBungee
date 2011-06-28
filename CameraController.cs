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

        static private BasicEffect EffectPlayer;

        static private Rectangle Border;
        static private BasicEffect Effect;

        static private Vector3 camPosition;
        static private Vector3 cameraLookat;

        static private Matrix Proj;
        static private Matrix View;

        static private float zoom;

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

        static public BasicEffect effectPlayer
        {
            get { return EffectPlayer; }
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

            EffectPlayer = new BasicEffect(EnvironmentVariable.graphics.GraphicsDevice);
            EffectPlayer.EnableDefaultLighting();
            EffectPlayer.World = Matrix.Identity;
            EffectPlayer.View = view;
            EffectPlayer.Projection = proj;
            EffectPlayer.TextureEnabled = true;
            //EffectPlayer.Texture = tex;
        }

        static public Texture2D PlayerTexture
        {
            get { return EffectPlayer.Texture; }
            set { EffectPlayer.Texture = value; }
        }

        static public void zoomOut()
        {
            zoom = zoom / 2;
            Proj = Matrix.CreateOrthographic(EnvironmentVariable.width / zoom, EnvironmentVariable.height / zoom, 0, 1);
            Effect.Projection = Proj;
            EffectPlayer.Projection = Proj;
        }

        static public void zoomIn()
        {
            zoom = zoom*2;
            Proj = Matrix.CreateOrthographic(EnvironmentVariable.width / zoom, EnvironmentVariable.height / zoom, 0, 1);             Effect.Projection = Proj;
            Effect.Projection = Proj;
            EffectPlayer.Projection = Proj;
        }

        static public void displacement(Vector2 pos1, Vector2 pos2)
        {

            Vector3 pos12 = EnvironmentVariable.graphics.GraphicsDevice.Viewport.Unproject(new Vector3(
                                pos1.X, pos1.Y, 1), proj, view, Matrix.Identity);
            Vector3 pos22 = EnvironmentVariable.graphics.GraphicsDevice.Viewport.Unproject(new Vector3(
                                pos2.X, pos2.Y, 1), proj, view, Matrix.Identity);

            Vector3 Disp = new Vector3(pos12.X - pos22.X, pos12.Y - pos22.Y, 0);

            camPosition = camPosition + new Vector3(Disp.X, Disp.Y, 0);
            cameraLookat = camPosition - new Vector3(0, 0, 1);

            View = Matrix.CreateLookAt(camPosition, cameraLookat, new Vector3(0.0f, 1.0f, 0.0f));

            Effect.View = view;
            EffectPlayer.View = view;
        }
        static public void update (Character player, GraphicsDevice graphics)
        {
            Vector2 positionTete = new Vector2();
            positionTete = player.ObjectPhysicCircleTete.FixtureObject.Body.Position;
            
            var onScreenPositon = graphics.Viewport.Project(new Vector3(positionTete, 0),
                                proj, view, Matrix.Identity);
            //Vector3 rightAndDownShift = onScreenPositon - new Vector3(Border.X + Border.Width, Border.Y + Border.Height, 0);
            //Vector3 leftAndUpShift = onScreenPositon - new Vector3(Border.X, Border.Y, 0);

            if (onScreenPositon.X > Border.X + Border.Width)
            {
                //camPosition = camPosition + new Vector3(onScreenPositon.X - Border.X - Border.Width, 0, 0);
                camPosition = camPosition + new Vector3((onScreenPositon.X - Border.X - Border.Width)/(Border.X/2), 0, 0);
            }
            if (onScreenPositon.X < Border.X)
            {
                //camPosition = camPosition + new Vector3(onScreenPositon.X - Border.X, 0, 0);
                camPosition = camPosition + new Vector3((onScreenPositon.X - Border.X) / (Border.X / 2), 0, 0);
            }
            //camPosition = new Vector3(positionTete.X, positionTete.Y, 0);
            cameraLookat = camPosition - new Vector3(0, 0, 1);

            View = Matrix.CreateLookAt(camPosition, cameraLookat, new Vector3(0.0f, 1.0f, 0.0f));

            Effect.View = view;
            EffectPlayer.View = view;

        }
    }
}
