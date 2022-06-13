namespace GrafischeEditor_DP.VisitorPattern
{
    public interface IVisitor
    {
        void VisitFigure(Figuur element);
        void VisitGroup(Groep element);
    }
}
