using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CHARACTERS
{
    /// <summary>
    /// Contains all data and functions available to a layer composing a sprite character
    /// </summary>
    public class CharacterSpriteLayer
    {
        private CharacterManager characterManager => CharacterManager.Instance;

        public int layer { get; private set; } = 0;
        public Image renderer { get; private set; } = null;

        public List<CanvasGroup> oldRenderers = new List<CanvasGroup>();

        public CharacterSpriteLayer(Image defaultRenderer, int layer = 0)
        {
            renderer = defaultRenderer;
            this.layer = layer;
        }

        public void SetSprite(Sprite sprite) 
        {
            renderer.sprite = sprite;
        }

    }
}
