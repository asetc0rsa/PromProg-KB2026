using System;

public class Player
{
    public string name;
    public int location;
    public State state = State.NotInGame;
    public int distanceTraveled = 0;

    public Player(string name)
    {
        this.name = name;
        this.location = -1;
    }

    public void Move(int steps, int fieldSize)
    {
        if (state == State.NotInGame)
        {
            location = steps;
            state = State.Playing;
        }
        else
        {
            distanceTraveled += Math.Abs(steps);
 
            int idx = location - 1;
            idx = (idx + steps) % fieldSize;
            
            if (idx < 0) 
            {
                idx += fieldSize;
            }
            
            location = idx + 1;
        }
    }
}