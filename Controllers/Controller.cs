using System;
using System.Collections.Generic;
public class Controller {
    public List<Command> commands { get; }

    private Command connect;
    private Command hello;

    public delegate void viewEventHandler(string text);
    public event viewEventHandler writeText;
    

    private Persistence persistence { get; }

    public Controller() {
        commands = new List<Command>();
        
        connect = new Command(Action.CONNECT, "connect", "Connect to database");
        commands.Add(connect);

        hello = new Command(Action.HELLO, "hello", "Hello world programs");
        commands.Add(hello);

        persistence = new Persistence();
    }

    public void executeCommand(string action) {
        if (action.Equals(connect.command)) {
            writeText("Connected");
        } else if (action.Equals(hello.command)) {
            writeText("Hello world!");
        } else {
            writeText("Command not found");
        }
    }
}