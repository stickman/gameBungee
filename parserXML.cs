using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using FarseerPhysics.Dynamics;

namespace gameBungee
{
    public static class ParserXML
    {
        public static TextureImg parseXmlToTexture(string st)
        {
            switch (st)
            {
                case "tree1":
                    return TextureImg.tree1;
                case "tree2":
                    return TextureImg.tree2;
                case "tree3":
                    return TextureImg.tree3;
                case "tree4":
                    return TextureImg.tree4;
                case "tree5":
                    return TextureImg.tree5;
                case "tree6":
                    return TextureImg.tree6;
                case "tree7":
                    return TextureImg.tree7;
                case "tree8":
                    return TextureImg.tree8;
                case "tree9":
                    return TextureImg.tree9;
                case "tree10":
                    return TextureImg.tree10;
                case "grass":
                    return TextureImg.grass;
                case "Idle":
                    return TextureImg.Idle;
                case "Run":
                    return TextureImg.Run;
                default:
                    return TextureImg.Null;
            }
        }
        public static Vector2 parseXmlToVector2(string st)
        {
            float posx;
            float posy;
            int i = 3;
            string sPosx = "";
            string sPosy = "";
            while (st[i] != ' ')
            {
                sPosx += st[i];
                i++;
            }
            i += 3;
            while (st[i] != '}')
            {
                sPosy += st[i];
                i++;
            }
            posx = float.Parse(sPosx);
            posy = float.Parse(sPosy);
            return new Vector2(posx, posy);
        }

        public static float parseXmlToFloat(string st)
        {
            return float.Parse(st);
        }

        public static BodyType parseXmlToBodyType(string st)
        {
            switch (st)
            {
                case "Static":
                    return BodyType.Static;
                case "Kinematic":
                    return BodyType.Kinematic;
                case "Dynamic":
                    return BodyType.Dynamic;
                default:
                    return BodyType.Static;
            }
        }
        public static Microsoft.Xna.Framework.Color parseXmlToColor(string st)
        {
            string fR = "";
            string fG = "";
            string fB = "";
            string fA = "";

            int i = 3;
            while (st[i] != ' ')
            {
                fR += st[i];
                i++;
            }
            i += 3;
            while (st[i] != ' ')
            {
                fG += st[i];
                i++;
            }
            i += 3;
            while (st[i] != ' ')
            {
                fB += st[i];
                i++;
            }
            i += 3;
            while (st[i] != '}')
            {
                fA += st[i];
                i++;
            }
            return new Microsoft.Xna.Framework.Color(float.Parse(fR),
                                                     float.Parse(fG),
                                                     float.Parse(fB),
                                                     float.Parse(fA));
        }
    }
}
