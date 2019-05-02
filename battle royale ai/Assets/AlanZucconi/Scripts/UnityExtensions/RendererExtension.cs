using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlanZucconi
{
    public static class RendererExtension
    {
        // Chaining syntax for material blocks
        public static MaterialPropertyBlock GetPropertyBlock (this Renderer renderer)
        {
            MaterialPropertyBlock block = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(block);
            return block;
        }
        // --- Chainable setters ---
        // Float
        public static MaterialPropertyBlock Set (this MaterialPropertyBlock block, string name, float value)
        {
            block.SetFloat(name, value);
            return block;
        }
        public static MaterialPropertyBlock Set(this MaterialPropertyBlock block, int nameID, float value)
        {
            block.SetFloat(nameID, value);
            return block;
        }
        // Color
        public static MaterialPropertyBlock Set(this MaterialPropertyBlock block, string name, Color value)
        {
            block.SetColor(name, value);
            return block;
        }
        public static MaterialPropertyBlock Set(this MaterialPropertyBlock block, int nameID, Color value)
        {
            block.SetColor(nameID, value);
            return block;
        }
        // Vector
        public static MaterialPropertyBlock Set(this MaterialPropertyBlock block, string name, Vector4 value)
        {
            block.SetVector(name, value);
            return block;
        }
        public static MaterialPropertyBlock Set(this MaterialPropertyBlock block, int nameID, Vector4 value)
        {
            block.SetVector(nameID, value);
            return block;
        }
        // Float
        public static MaterialPropertyBlock Set(this MaterialPropertyBlock block, string name, Texture value)
        {
            block.SetTexture(name, value);
            return block;
        }
        public static MaterialPropertyBlock Set(this MaterialPropertyBlock block, int nameID, Texture value)
        {
            block.SetTexture(nameID, value);
            return block;
        }
    }
}