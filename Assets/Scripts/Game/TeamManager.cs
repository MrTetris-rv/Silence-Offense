using System;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    [SerializeField] private TeamSelectionData teamSelectionData;
    [SerializeField] private GameObject redTeamPosition;
    [SerializeField] private GameObject blueTeamPosition;

    public static event Action OnTeamChoiceReady;

    private void Start()
    {
        Instantiate(redTeamPosition, redTeamPosition.transform.position, blueTeamPosition.transform.rotation);
        Instantiate(blueTeamPosition, blueTeamPosition.transform.position, blueTeamPosition.transform.rotation);
    }

    public void SelectTeam(string teamName)
    {
        if (teamSelectionData != null)
        {
            teamSelectionData.selectedTeam = teamName;
            OnTeamChoiceReady?.Invoke();
            Destroy(gameObject);
        }
        else
        {
            Debug.LogError("TeamSelectionData is not defined!");
        }
    }
}
