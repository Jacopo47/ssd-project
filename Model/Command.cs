public class Command
{
    public Action action { get; }
    public string command { get; }
    public string description { get; }

    public Command(Action action, string command, string description)
    {
        this.action = action;
        this.command = command;
        this.description = description;
    }

    public Command(Action action, string command) {
        this.action = action;
        this.command = command;
        this.description = string.Empty;
    }

    public override string ToString()
    {
        return command + " - " + description;
    }
}

public enum Action {
    HELLO, CONNECT
}