namespace Backend.Interfaces;

internal interface IStatelessOpponent : IOpponent
{
    public void SendFen(string fen);
}