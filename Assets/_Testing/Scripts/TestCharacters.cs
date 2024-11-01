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
            //Character_Sprite guard1 = CharacterManager.Instance.CreateCharacter("Guard1 as Generic") as Character_Sprite;
            //Character_Sprite student2 = CharacterManager.Instance.CreateCharacter("Female Student 2") as Character_Sprite;
            Character_Sprite raelin = CharacterManager.Instance.CreateCharacter("Raelin") as Character_Sprite;

            //guard1.Show();

            yield return new WaitForSeconds(1f);

            yield return raelin.layers[1].TransitionColor(Color.red, 0.3f);

            yield return new WaitForSeconds(1f);

            yield return raelin.layers[1].TransitionColor(Color.blue, 0.3f);

            yield return new WaitForSeconds(1f);

            yield return raelin.layers[1].TransitionColor(Color.yellow, 0.3f);

            yield return new WaitForSeconds(1f);

            yield return raelin.layers[1].TransitionColor(Color.black, 0.3f);

            yield return new WaitForSeconds(1f);


            //raelin.Show();

            //Sprite raelingBodySprite = 

            yield return null;

        }
    }
}
