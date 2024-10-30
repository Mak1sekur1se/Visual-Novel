using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CHARACTERS
{
    /// <summary>
    /// A character that uses a 3D model to render their display in the scene
    /// </summary>
    public class Character_Model3D : Character
    {
        public Character_Model3D(string name, CharacterConfigData config) : base(name, config) 
        {
            Debug.Log($"Created Model3D Character: '{name}'");
        }
    }
}
