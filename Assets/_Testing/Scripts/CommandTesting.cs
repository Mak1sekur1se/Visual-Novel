using COMMANDS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TESTING
{
    public class CommandTesting : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(Running());


        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                CommandManager.Instance.Execute("moveCharDemo", "left");
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                CommandManager.Instance.Execute("moveCharDemo", "right");
        }

        private IEnumerator Running()
        {
            yield return CommandManager.Instance.Execute("print");
            yield return CommandManager.Instance.Execute("print_lp", "Hello World");
            yield return CommandManager.Instance.Execute("print_mp", "Line1", "Line2", "Line3");

            yield return CommandManager.Instance.Execute("lambda");
            yield return CommandManager.Instance.Execute("lambda_lp", "Hello World");
            yield return CommandManager.Instance.Execute("lambda_mp", "Line1", "Line2", "Line3");

            yield return CommandManager.Instance.Execute("process");
            yield return CommandManager.Instance.Execute("process_lp", "5");
            yield return CommandManager.Instance.Execute("process_mp", "Line1", "Line2", "Line3");
        }
    }
}