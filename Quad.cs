
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace gameBungee
{
    public struct Quad
    {
        private Vector3 Origin;
        private Vector3 UpperLeft;
        private Vector3 LowerLeft;
        private Vector3 UpperRight;
        private Vector3 LowerRight;
        private Vector3 Normal;
        private Vector3 Up;
        private Vector3 Left;

        private float NbFrame;

        public VertexPositionNormalTexture[] Vertices;
        //        public int[] Indexes;
        public short[] Indexes;


        public Quad(Vector3 origin, Vector3 normal, Vector3 up,
            float width, float height, int i_FrameCurrent, int NbFrame, Boolean Flip)
        {
            this.NbFrame = (float)NbFrame;
            Vertices = new VertexPositionNormalTexture[4];
            Indexes = new short[6];
            Origin = origin;
            Normal = normal;
            Up = up;

            // Calculate the quad corners
            if (!Flip)
            {
                Left = Vector3.Cross(normal, Up);
                Vector3 uppercenter = (Up * height / 2) + origin;
                UpperLeft = uppercenter + (Left * width / 2);
                UpperRight = uppercenter - (Left * width / 2);
                LowerLeft = UpperLeft - (Up * height);
                LowerRight = UpperRight - (Up * height);
            }
            else
            {
                Left = Vector3.Cross(normal, Up);
                Vector3 uppercenter = (Up * height / 2) + origin;
                UpperLeft = uppercenter + (Left * width / 2);
                UpperRight = uppercenter - (Left * width / 2);
                LowerLeft = UpperLeft - (Up * height);
                LowerRight = UpperRight - (Up * height); 
            }

            FillVertices(i_FrameCurrent, Flip);
        }

        private void FillVertices(int i_FrameCurrent, Boolean Flip)
        {
            if (!Flip)
            {
                // Fill in texture coordinates to display full texture
                // on quad
                Vector2 textureLowerLeft = new Vector2(0.0f + i_FrameCurrent / NbFrame, 1.0f);
                Vector2 textureLowerRight = new Vector2(1.0f / NbFrame + i_FrameCurrent / NbFrame, 1.0f);
                Vector2 textureUpperLeft = new Vector2(i_FrameCurrent / NbFrame, 0.0f);
                Vector2 textureUpperRight = new Vector2(1.0f / NbFrame + i_FrameCurrent / NbFrame, 0.0f);

                // Provide a normal for each vertex
                for (int i = 0; i < Vertices.Length; i++)
                {
                    Vertices[i].Normal = Normal;
                }

                // Set the position and texture coordinate for each
                // vertex
                Vertices[0].Position = LowerLeft;
                Vertices[0].TextureCoordinate = textureLowerLeft;
                Vertices[1].Position = UpperLeft;
                Vertices[1].TextureCoordinate = textureUpperLeft;
                Vertices[2].Position = LowerRight;
                Vertices[2].TextureCoordinate = textureLowerRight;
                Vertices[3].Position = UpperRight;
                Vertices[3].TextureCoordinate = textureUpperRight;

                // Set the index buffer for each vertex, using
                // clockwise winding
                Indexes[0] = 0;
                Indexes[1] = 1;
                Indexes[2] = 2;
                Indexes[3] = 2;
                Indexes[4] = 1;
                Indexes[5] = 3;
            }
            else
            {
                // Fill in texture coordinates to display full texture
                // on quad
                Vector2 textureLowerLeft  = new Vector2(1.0f / NbFrame + i_FrameCurrent / NbFrame, 1.0f);
                Vector2 textureLowerRight = new Vector2(0.0f + i_FrameCurrent / NbFrame, 1.0f);
                Vector2 textureUpperLeft  = new Vector2(1.0f / NbFrame + i_FrameCurrent / NbFrame, 0.0f);
                Vector2 textureUpperRight = new Vector2(i_FrameCurrent / NbFrame, 0.0f);

                // Provide a normal for each vertex
                for (int i = 0; i < Vertices.Length; i++)
                {
                    Vertices[i].Normal = Normal;
                }

                // Set the position and texture coordinate for each
                // vertex
                Vertices[0].Position = LowerLeft;
                Vertices[0].TextureCoordinate = textureLowerLeft;
                Vertices[1].Position = UpperLeft;
                Vertices[1].TextureCoordinate = textureUpperLeft;
                Vertices[2].Position = LowerRight;
                Vertices[2].TextureCoordinate = textureLowerRight;
                Vertices[3].Position = UpperRight;
                Vertices[3].TextureCoordinate = textureUpperRight;

                // Set the index buffer for each vertex, using
                // clockwise winding
                Indexes[0] = 0;
                Indexes[1] = 1;
                Indexes[2] = 2;
                Indexes[3] = 2;
                Indexes[4] = 1;
                Indexes[5] = 3;

            }
        }
    }
}
