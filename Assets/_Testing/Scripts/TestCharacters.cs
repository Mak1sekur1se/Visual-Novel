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
            Character guard2 = CharacterManager.Instance.CreateCharacter("Guard2 as Raelin");
            Character guard3 = CharacterManager.Instance.CreateCharacter("Guard3 as Generic");

            guard1.SetPosition(Vector2.zero);
            //guard2.SetPosition(new Vector2(0.5f, 0.5f));
            //guard3.SetPosition(Vector2.one);



            yield return guard1.Show();


            yield return guard1.MoveToPosition(new Vector2(1.5f, 1.5f), 0.66f, true);
            yield return guard1.MoveToPosition(Vector2.zero);
            guard2.Show();
            guard3.Show();


            guard1.SetDialogueFont(tempFont);
            guard1.SetNameFont(tempFont);
            guard2.SetDialogueColor(Color.cyan);
            guard3.SetNameColor(Color.red);

            yield return guard1.Say("I want to say something important.");
            yield return guard2.Say("Hold your place.");
            yield return guard3.Say("Let him speak...");

            yield return null;

        }
    }
}
