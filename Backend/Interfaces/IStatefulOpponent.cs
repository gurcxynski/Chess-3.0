namespace Backend.Interfaces;

internal interface IStatefulOpponent : IOpponent
{
    public void Listen();
    public void ProcessMove(Engine.Move move);
    protected void Send(string data);
}