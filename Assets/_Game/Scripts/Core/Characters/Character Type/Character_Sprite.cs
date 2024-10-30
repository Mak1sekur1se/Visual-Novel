using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CHARACTERS
{
    /// <summary>
    /// A character that uses sprites or sprite sheets to render its display
    /// </summary>
    public class Character_Sprite : Character
    {
        public Character_Sprite(string name, CharacterConfigData config) : base(name, config) 
        {
            Debug.Log($"Created Sprite Character: '{name}'");
        }
    }
}
