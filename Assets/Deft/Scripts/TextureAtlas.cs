using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deft
{
    public class TextureAtlas
    {
        private Texture2D texture;
        private Rect[] uvs;

        public TextureAtlas() { }
        public TextureAtlas(Texture2D[] textures) => PackTextures(textures);

        public void PackTextures(Texture2D[] textures)
        {
            texture = new Texture2D(4096, 4096);
            texture.filterMode = FilterMode.Point;
            uvs = texture.PackTextures(textures, 0, 4096);
        }

        public Texture2D GetTexture() => texture;
        public Rect GetUVs(int index) => uvs[index];
    }
}
