namespace LibrarySystem.Domain;

public class Magazine : LibraryItem
{
    public int IssueNumber { get; private set; }

    public Magazine(string title, int issueNumber) : base(title)
    {
        if (issueNumber <= 0)
            throw new ArgumentException("Номер випуску має бути додатнім.", nameof(issueNumber));
            
        IssueNumber = issueNumber;
    }
}