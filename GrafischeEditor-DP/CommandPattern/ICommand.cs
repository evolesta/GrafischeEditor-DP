namespace GrafischeEditor_DP.CommandPattern
{
    /// <summary>
    /// 'Command' interface declares a method for executing a command
    /// </summary>
    public interface ICommand
    {
        void Execute(); // declareer een methode voor het uitvoeren van een command
        void Undo(); // declareer een methode voor het terugdraaien van een command
    }
}