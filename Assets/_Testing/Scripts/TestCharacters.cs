using CHARACTERS;
using DIALOGUE;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TESTING
{
    public class TestCharacters : MonoBehaviour
    {
        public TMP_FontAsset tempFont;

        private Character CreateCharacter(string name) => CharacterManager.Instance.CreateCharacter(name);
        private void Start()
        {
            //Character Raelin = CharacterManager.Instance.CreateCharacter("Generic");
            //Character Adam = CharacterManager.Instance.CreateCharacter("Adam");

            StartCoroutine(Test());
        }

        private IEnumerator Test()
        {
            Character guard1 = CharacterManager.Instance.CreateCharacter("Guard1 as Generic");
            Character_Sprite student2 = CharacterManager.Instance.CreateCharacter("Female Student 2") as Character_Sprite;
            Character_Sprite raelin = CharacterManager.Instance.CreateCharacter("Raelin") as Character_Sprite;

           

            yield return null;

        }
    }
}
