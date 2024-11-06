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

            StartCoroutine(Test());
        }

        private IEnumerator Test()
        {
            Character_Sprite guard1 = CharacterManager.Instance.CreateCharacter("Guard1 as Generic") as Character_Sprite;
            Character_Sprite guardRed = CharacterManager.Instance.CreateCharacter("Guard Red as Generic") as Character_Sprite;
            Character_Sprite student2 = CharacterManager.Instance.CreateCharacter("Girl as Female Student 2") as Character_Sprite;
            Character_Sprite raelin = CharacterManager.Instance.CreateCharacter("Raelin") as Character_Sprite;
            
            guardRed.SetColor(Color.red);

            student2.SetPosition(new Vector2(0.3f, 0));
            raelin.SetPosition(new Vector2(0.45f, 0));
            guard1.SetPosition(new Vector2(0.6f, 0));
            guardRed.SetPosition(new Vector2(0.75f, 0));


            yield return new WaitForSeconds(2);
            Debug.Log("two seconds later");
            guardRed.SetPriority(1000);
            student2.SetPriority(1);
            yield return new WaitForSeconds(2);
            Debug.Log("two seconds later");

            CharacterManager.Instance.SortCharacters(new string[]{ "Guard1", "Raelin", });


            yield return new WaitForSeconds(2);
            Debug.Log("two seconds later");

            CharacterManager.Instance.SortCharacters();


            yield return new WaitForSeconds(2);
            Debug.Log("two seconds later");
            CharacterManager.Instance.SortCharacters(new string[] { "Raelin", "Guard1", "Girl", "Guard Red"});

            #region Obsolete
            //yield return new WaitForSeconds(1f);

            //yield return raelin.Flip(0.3f);
            //yield return student2.Flip(immediate: true);

            //raelin.UnHighlight();
            //yield return student2.Say("I want to say something");

            //student2.UnHighlight();
            //raelin.Highlight();
            //yield return raelin.Say("But I want to say something too!{c}Can I go first?");

            //student2.Highlight();
            //raelin.UnHighlight();
            //yield return student2.Say("Sure,{a} be my guest");

            //raelin.Highlight();
            //student2.UnHighlight();
            //raelin.TransitionSprite(raelin.GetSprite("Raelin_SideWays_BitShy"), layer: 1);

            //yield return raelin.Say("Yay!");

            //yield return null;
            #endregion
        }
    }
}
