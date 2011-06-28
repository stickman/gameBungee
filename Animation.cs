using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace gameBungee
{
    public class Animation
    {
        Texture2D texture;              //Texture de l'animation
        float f_FrameTime;              //Permet de connaitre le temps d'affichage de chaque frame de l'animation
        bool b_IsLooping;               //Permet de savoir si on est encore en train d'animer
        public BasicEffect effectPlayer;

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        public float FrameTime
        {
            get { return f_FrameTime; }
            set { this.f_FrameTime = value; }
        }

        public bool IsLooping
        {
            get { return b_IsLooping; }
        }

        public int Nbr_Frame
        {
            get { return Texture.Width / Texture.Height; }
        }

        public int FrameWidth
        {
            get { return texture.Width; }
        }

        public int FrameHeight
        {
            get { return texture.Height; }
        }

        public Animation(Texture2D tex, float FrameTime, bool IsLooping)
        {
            f_FrameTime = FrameTime;
            b_IsLooping = IsLooping;

            if (tex != null)
            {
                texture = tex;
                effectPlayer = new BasicEffect(EnvironmentVariable.graphics.GraphicsDevice);

                effectPlayer.EnableDefaultLighting();
                effectPlayer.World = Matrix.Identity;
                effectPlayer.View = CameraController.view;
                effectPlayer.Projection = CameraController.proj;
                effectPlayer.TextureEnabled = true;

                effectPlayer.Texture = tex;
            }
        }
    }
}
