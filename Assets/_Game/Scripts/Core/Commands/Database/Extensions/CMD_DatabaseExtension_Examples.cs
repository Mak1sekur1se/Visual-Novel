using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMD_DatabaseExtension_Examples : CMD_DatabaseExtension
{
    new public static void Extend(CommandDatabase database)
    {
        //Add Action with no parameters
        database.AddCommand("print", new Action(PrintDefaultMessage));
        database.AddCommand("print_lp", new Action<string>(PrintUsermessage));
        database.AddCommand("print_mp", new Action<string[]>(PrintLines));

        //Add lambda with no parameters
        database.AddCommand("lambda", new Action(() => { Debug.Log("Printing a default message to console from lambda command"); }));
        database.AddCommand("lambda_lp", new Action<string>((arg) => { Debug.Log($"Log User Lambda Message: '{arg}'"); }));
        database.AddCommand("lambda_mp", new Action<string[]>((args) => { Debug.Log(string.Join(",", args)); }));

        //Add coroutine with no parameters
        database.AddCommand("process", new Func<IEnumerator>(SimpleProcess));
        database.AddCommand("process_lp", new Func<string,IEnumerator>(LineProcess));
        database.AddCommand("process_mp", new Func<string[],IEnumerator>(MultiProcess));
    }

    private static void PrintDefaultMessage()
    {
        Debug.Log("Printing a default message to console");
    }

    private static void PrintUsermessage(string message)
    {
        Debug.Log($"User Message: '{message}'");
    }

    private static void PrintLines(string[] lines)
    {
        int i = 1;
        foreach (string line in lines)
        {
            Debug.Log($"{i++}. '{line}'");
        }
    }

    private static IEnumerator SimpleProcess()
    {
        for (int i = 1; i <= 5; i++)
        {
            Debug.Log($"Process Running... [{i}]");
            yield return new WaitForSeconds(1);
        }
    }

    private static IEnumerator LineProcess(string data)
    {
        if (int.TryParse(data, out int num))
        {
            for (int i = 1; i <= num; i++)
            {
                Debug.Log($"Process Running... [{i}]");
                yield return new WaitForSeconds(1);
            }
        }

    }

    private static IEnumerator MultiProcess(string[] data)
    {
        foreach (string line in data)
        {
            Debug.Log($"Process Message: '{line}'");

            yield return new WaitForSeconds(0.5f);
        }
    }
}
