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
        private void Start()
        {
            //Character Stella = CharacterManager.Instance.CreateCharacter("Stella");
            //Character Adam = CharacterManager.Instance.CreateCharacter("Adam");

            StartCoroutine(Test());
        }

        private IEnumerator Test()
        {
            Character Elen = CharacterManager.Instance.CreateCharacter("Elen");
            Character Adam = CharacterManager.Instance.CreateCharacter("Adam");
            Character Ben = CharacterManager.Instance.CreateCharacter("Benjamin");

            List<string> lines = new List<string>()
            {
                "Hi!",
                "This is a line.",
                "And another.",
                "And a last one."
            };

            yield return Elen.Say(lines);

            Elen.SetNameColor(Color.red);
            Elen.SetDialogueColor(Color.green);
            Elen.SetNameFont(tempFont);
            Elen.SetDialogueFont(tempFont);

            yield return Elen.Say(lines);

            lines = new List<string>()
            {
                "I am Adam.",
                "More lines{c} here."
            };

            yield return Adam.Say(lines);

            yield return Ben.Say("This is a line that I want to say.{a} It is a simple line.");

            Debug.Log("Finished");

        }
    }
}
