using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CHARACTERS
{
    /// <summary>
    /// A character that uses live2D technology to render an animated graphical display
    /// </summary>
    public class Character_Live2D : Character
    {
        public Character_Live2D(string name, CharacterConfigData config) : base(name, config) 
        {
            Debug.Log($"Created Live2D Character: '{name}'");
        }
    }
}
