using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FunnyFriday
{
    class TextureManager
    {
        private AtlasParser parser;

        public TextureManager(string _path)
        {
            parser = new AtlasParser(_path);
            atlasImage = new Image(parser.GetFileName());
        }

        Image atlasImage;

        public List<Texture> GetTextures(string[] textures)
        {
            List<int[]> data = new List<int[]>();
            List<Texture> texture = new List<Texture>();

            for (int i = 0; i < textures.Length; i++)
            {
                data.Add(parser.GetInfo(textures[i]));
                texture.Add(new Texture(atlasImage, new IntRect(data[i][0], data[i][1], data[i][2], data[i][3])));
                texture[i].Smooth = true;
                texture[i].Repeated = false;
            }

            return texture;
        }

        public List<Texture> GetTextures()
        {
            List<Texture> texture = new List<Texture>();

            for(int i = 3; i < File.ReadAllLines(parser.GetFileName()).Length; i++)
            {
                int[] temp = parser.GetInfo(i);
                texture.Add(new Texture(atlasImage, new IntRect(temp[0], temp[1], temp[2], temp[3])));
                texture[i - 3].Smooth = true;
                texture[i - 3].Repeated = false;
            }

            return texture;
        }

        public List<Texture> GetTextures(int min, int max)
        {
            List<Texture> texture = new List<Texture>();

            for (int i = min; i < max; i++)
            {
                int[] temp = parser.GetInfo(i);
                texture.Add(new Texture(atlasImage, new IntRect(temp[0], temp[1], temp[2], temp[3])));
                texture[i - min].Smooth = true;
                texture[i - min].Repeated = false;
            }

            return texture;
        }

        public Texture GetTexture(string path)
        {
            var temp = new Texture(path);
            temp.Smooth = true;
            temp.Repeated = false;
            return temp;
        }
    }
}
