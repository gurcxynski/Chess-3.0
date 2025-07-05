namespace Backend.Interfaces;

internal interface IOpponent
{
    public event EventHandler<(byte[], int)> OnMoveDataReceived;
    public event EventHandler<string> OnConnectionEstablished;
    public event EventHandler OnMessageSent;
    public void Start();
    public void Stop();
}