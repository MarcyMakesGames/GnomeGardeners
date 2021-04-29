public interface IScoringArea : IInteractable
{
    int TotalScore { get; set; }
    void AddScore(int score);
}
