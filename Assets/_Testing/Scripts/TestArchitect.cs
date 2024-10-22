using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TESTING
{
    public class TestArchitect : MonoBehaviour
    {
        DialogueSystem ds;
        TextArchitect architect;

        string[] lines = new string[5]
        {
            "This is a random line of dialogue.",
            "I want to say something, come over here.",
            "The world is a crazy place sometimes",
            "Don't lose hope, things will get better!",
            "It's a bird? It's a plane? No! - It's Super Sheltie!"
        };
        // Start is called before the first frame update
        void Start()
        {
            ds = DialogueSystem.Instance;
            architect = new TextArchitect(ds.dialogueContainer.dialogueText);
            architect.buildMethod = TextArchitect.BuildMethod.typewriter;
            architect.speed = 0.5f;
        }

        // Update is called once per frame
        void Update()
        {
            string longline = "this is a very long line that make no sense but I am just populating it with stuff.this is a very long line that make no sense but I am just populating it with stuff.this is a very long line that make no sense but I am just populating it with stuff.this is a very long line that make no sense but I am just populating it with stuff.this is a very long line that make no sense but I am just populating it with stuff.this is a very long line that make no sense but I am just populating it with stuff.";
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (architect.isBuilding)
                {
                    //第二次点击加速
                    if (!architect.hurryUp)
                        architect.hurryUp = true;
                    //第三次点击强制结束
                    else
                        architect.ForeceComplete();
                }
                else
                architect.Build(longline);
                //architect.Build(lines[Random.Range(0, lines.Length)]);
            }

            else if (Input.GetKeyDown(KeyCode.A)) {
                architect.Append(longline);
                //architect.Append(lines[Random.Range(0, lines.Length)]);
            }
        }
    }
}
