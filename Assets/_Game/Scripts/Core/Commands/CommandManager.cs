using UnityEngine;
using System.Reflection;
using System.Linq;
using System;
using System.Collections;

public class CommandManager : MonoBehaviour
{
    public static CommandManager Instance { get; private set; }
    private static Coroutine process;
    private static bool isRunning => process != null;

    private CommandDatabase database;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            database = new CommandDatabase();

            //给数据库添加命令 谨慎使用因为可以跳过编译检查
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type[] extensionTypes = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(CMD_DatabaseExtension))).ToArray();

            foreach (Type extension in extensionTypes)
            {
                MethodInfo extendMethod = extension.GetMethod("Extend");
                extendMethod.Invoke(null, new object[] { database });
            }
        }
        else
            DestroyImmediate(gameObject);
    }

    public Coroutine Execute(string commandName, params string[] args)
    {
        Delegate command = database.GetCommand(commandName);

        if (command == null)
            return null;

        //if (command is Action)
        //    command.DynamicInvoke(args);//动态调用参数为0
        //else if (command is Action<string>)
        //    command.DynamicInvoke(args[0]);
        //else if (command is Action<string[]>)
        //    command.DynamicInvoke((object)args);

        return StartProcess(commandName, command, args);
    }

    private Coroutine StartProcess(string commandName, Delegate command, string[] args)
    {
        StopCurrentProcess();

        process = StartCoroutine(RunningProcess(command, args));

        return process;
    }

    private void StopCurrentProcess()
    {
        if(process != null)
            StopCoroutine(process);

        process = null;
    }

    private IEnumerator RunningProcess(Delegate command, string[] args)
    {
        yield return WaitingForProcessToComplete(command, args);

        process = null;
    }

    private IEnumerator WaitingForProcessToComplete(Delegate command, string[] args)
    {

        ParameterInfo[] parameters = command.GetMethodInfo().GetParameters();
        bool isMoreParam = parameters.Length != 0 && parameters[0].ParameterType.IsArray;//只能检测函数只有一个参数且只能是Array

        if (command.GetMethodInfo().ReturnType == typeof(IEnumerator))
        {
            if (isMoreParam)
            {
                yield return (IEnumerator)command.DynamicInvoke((object)args);
            }
            else
            {
                yield return (IEnumerator)command.DynamicInvoke(args);
            }
        }
        else
        {
            if (isMoreParam)
            {
                command.DynamicInvoke((object)args);
            }
            else
            {
                command.DynamicInvoke(args);
            }
            //command.DynamicInvoke(isMoreParam ? (object)args : args); 未知报错原因
        }
    }

    private static IEnumerator MoveCharacter(string direction)
    {
        bool left = direction.ToLower() == "left";

        //Get the variables I need. This would be defined somewhere else.
        Transform character = GameObject.Find("Image").transform;
        float moveSpeed = 15;

        //Calculate the target Posiiton for the image
        float targetX = left ? -8 : 8;

        //Calculate the current position of the image
        float currentX = character.position.x;

        //Move the image gradually towards the target position
        while (Mathf.Abs(targetX - currentX) > 0.1f) 
        {
            Debug.Log($"Moving character to {(left ? "left" : "right")} [{currentX}/{targetX}]");
            currentX = Mathf.MoveTowards(currentX, targetX, moveSpeed * Time.deltaTime);
            character.position = new Vector3(currentX, character.position.y, character.position.z);
            yield return null;
        }

    }
}
