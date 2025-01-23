[System.Serializable]
public class PlayerTeamData
{
    public string playerName;
    public string teamName;

    public PlayerTeamData(string playerName, string teamName)
    {
        this.playerName = playerName;
        this.teamName = teamName;
    }
}
