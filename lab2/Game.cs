using System;
using System.IO;

public class Game
{
    public int size;
    public Player cat;
    public Player mouse;
    public GameState state;

    public static string InputFile = "1.ChaseData.txt";
    public static string OutFile = "1.PursuitLog.txt";

    private StreamWriter writer;

    public Game(int size)
    {
        this.size = size;
        cat = new Player("Cat");
        mouse = new Player("Mouse");
        state = GameState.Start;
    }

    public void Run()
    {
        using (StreamReader reader = new StreamReader(InputFile))
        using (writer = new StreamWriter(OutFile))
        {
            reader.ReadLine();

            writer.WriteLine("Cat and Mouse");
            writer.WriteLine();
            writer.WriteLine(string.Format("{0,3}{1,6}{2,10}", "Cat", "Mouse", "Distance"));
            writer.WriteLine("-------------------");

            string line;
            while ((line = reader.ReadLine()) != null && state != GameState.End)
            {
                line = line.Trim();
                if (string.IsNullOrEmpty(line)) continue;

                string[] parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                char command = parts[0][0];

                if (command == 'P')
                {
                    DoPrintCommand();
                }
                else if (command == 'M' || command == 'C')
                {
                    int steps = int.Parse(parts[1]);
                    DoMoveCommand(command, steps);

                    if (cat.state == State.Playing && mouse.state == State.Playing)
                    {
                        if (cat.location == mouse.location)
                        {
                            cat.state = State.Winner;
                            mouse.state = State.Loser;
                            state = GameState.End;
                        }
                    }
                }
            }

            writer.WriteLine("-------------------");
            writer.WriteLine();
            writer.WriteLine();
            writer.WriteLine("Distance traveled:   Mouse    Cat");
            writer.WriteLine($"{mouse.distanceTraveled,26}{cat.distanceTraveled,7}");
            writer.WriteLine();
            
            if (cat.state == State.Winner)
            {
                writer.WriteLine($"Mouse caught at: {cat.location,2}");
            }
            else
            {
                writer.WriteLine("Mouse evaded Cat");
            }
        }
    }

    private void DoMoveCommand(char command, int steps)
    {
        switch (command)
        {
            case 'M': mouse.Move(steps, size); break;
            case 'C': cat.Move(steps, size); break;
        }
    }

    private void DoPrintCommand()
    {
        string catStr = cat.state == State.NotInGame ? "??" : cat.location.ToString();
        string mouseStr = mouse.state == State.NotInGame ? "??" : mouse.location.ToString();
        string distStr = (cat.state != State.NotInGame && mouse.state != State.NotInGame) ? GetDistance().ToString() : "";

        writer.WriteLine(string.Format("{0,3}{1,6}{2,10}", catStr, mouseStr, distStr));
    }

    private int GetDistance()
    {
        return Math.Abs(cat.location - mouse.location);
    }
}