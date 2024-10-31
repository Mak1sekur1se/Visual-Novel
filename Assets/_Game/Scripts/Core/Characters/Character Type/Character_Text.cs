using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CHARACTERS
{
    /// <summary>
    /// A character with no graphical art.Text operations only.
    /// </summary>
    public class Character_Text : Character
    {
        public Character_Text(string name, CharacterConfigData config) : base (name, config, null)
        {
            Debug.Log($"Created Text Character: '{name}'");
        }
    }
}
