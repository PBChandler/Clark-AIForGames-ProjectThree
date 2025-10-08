using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float gameSpeed = 1.0f;
    public Flag[] flags;
    private int raisedFlags = 0;
    private bool gameWon = false;
    public static bool agentOffDuty = false;
    public static List<NPCTankController> npcs = new List<NPCTankController>();

    [SerializeField] private UIManager uiManager;

    void Start()
    {
        Physics.gravity = new Vector3(0, -500.0f, 0);
        Time.timeScale = gameSpeed;

        foreach (var flag in flags)
        {
            flag.OnFullMast += HandleFlagRaised;
        }
    }

    public static void RemoveTank(NPCTankController tom)
    {
        npcs.Remove(tom);
    }

    public static void AddTank(NPCTankController tom)
    {
        npcs.Add(tom);
    }
    public static void UpdateOffDuty(bool state)
    {
        agentOffDuty = state;
    }

    public static bool CheckOffDuty()
    {
        if(npcs.Count > 1)
        {
            return false;
        }
        return agentOffDuty;
    }
    private void HandleFlagRaised()
    {
        if (gameWon) return;

        raisedFlags++;
        Debug.Log("Flag raised! Total: " + raisedFlags);

        if (raisedFlags >= flags.Length)
        {
            WinGame();
        }
    }

    private void WinGame()
    {
        gameWon = true;
        Debug.Log("All flags raised! YOU WIN!");

        uiManager?.ShowWinScreen();
    }

    public void LoseGame()
    {
        if (gameWon) return;

        Debug.Log("Game Over! YOU LOSE.");
        Time.timeScale = 0f;
        uiManager?.ShowLossScreen();
    }

    void Update()
    {
        if (!gameWon)
            Time.timeScale = gameSpeed;
    }
}
