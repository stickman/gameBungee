using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace gameBungee
{
    public class Animation
    {
        Texture2D texture;              //Texture de l'animation
        float f_FrameTime;              //Permet de connaitre le temps d'affichage de chaque frame de l'animation
        bool b_IsLooping;               //Permet de savoir si on est encore en train d'animer
        int i_FrameCount;               //Nombre de frame dans une animation

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        public float FrameTime
        {
            get { return f_FrameTime; }
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
            texture = tex;
            f_FrameTime = FrameTime;
            b_IsLooping = IsLooping;
        }
    }
}
