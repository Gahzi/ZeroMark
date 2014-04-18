using UnityEngine;
using System.Collections.Generic;

public class GUIManager : MonoBehaviour
{
    public enum GUIManagerState { Hidden, ShowingStatTab, ShowingEndGameTab }
    public GUIManagerState state = GUIManagerState.Hidden;

    private GUIContent killsColumnContent = new GUIContent("Kills");
    private GUIContent deathsColumnContent= new GUIContent("Deaths");
    private GUIContent pointsGainedColumnContent = new GUIContent("Points Gained");
    private GUIContent pointsLostColumnContent = new GUIContent("Points Lost");
    private GUIContent pointsBankedColumnContent = new GUIContent("Points Banked");

    private GUIContent endGameContent = new GUIContent("End Game");

    public GUIStyle headerStyle;
    public GUIStyle columnStyle;
    public GUIStyle redPlayerStyle;
    public GUIStyle bluePlayerStyle;
    public GUIStyle endGameStyle;

    private static GUIManager instance;

    public static GUIManager Instance
    {
        get
        {
            if (instance != null)
            {
                return instance;
            }
            else
            {
                instance = ((GameObject)GameObject.Instantiate(Resources.Load("Managers/GUIManager"))).GetComponent<GUIManager>();
                return instance;
            }
        }
    }

    private void Awake()
    {
        instance = this;
    }


    private void Start()
    {

    }
    
    void OnGUI()
    {
        switch (state)
        {
            case GUIManagerState.Hidden:
            {
                break;
            }

            case GUIManagerState.ShowingStatTab:
            {
                ShowStatTab();
                break;
            }

            case GUIManagerState.ShowingEndGameTab:
            {
                ShowEndGameTab();
                break;
            }
        }
    }

    private void ShowStatTab()
    {
        float statX = Screen.width * 0.25f;
        float statY = Screen.height * 0.1f;
        float statWidth = Screen.width * 0.5f;
        float statHeight = Screen.height * 0.8f;
        Rect statTabRect = new Rect(statX,statY,statWidth,statHeight);

        GUI.BeginGroup(statTabRect);
        GUI.Box(new Rect(0, 0, statWidth, statHeight),"");
        //room name

        GUI.skin.label = headerStyle;
        GUIContent roomNameContent = new GUIContent("Room Name");
        Rect roomNameRect = new Rect(statWidth*0.5f,0,GUI.skin.label.CalcSize(roomNameContent).x,GUI.skin.label.CalcSize(roomNameContent).y);
        GUI.Label(roomNameRect, roomNameContent);

        //columns
        GUI.skin.label = columnStyle;
        float columnPadding = statWidth * 0.05f;

        Rect killsColumnRect = new Rect(statWidth * 0.3f, statHeight * 0.08f, GUI.skin.label.CalcSize(killsColumnContent).x, GUI.skin.label.CalcSize(killsColumnContent).y);
        GUI.Label(killsColumnRect, killsColumnContent);

        Rect deathsColumnRect = new Rect(killsColumnRect.x + killsColumnRect.width + columnPadding, killsColumnRect.y, GUI.skin.label.CalcSize(deathsColumnContent).x, GUI.skin.label.CalcSize(deathsColumnContent).y);
        GUI.Label(deathsColumnRect, deathsColumnContent);

        Rect pointsGainedColumnRect = new Rect(deathsColumnRect.x + deathsColumnRect.width + columnPadding, killsColumnRect.y, GUI.skin.label.CalcSize(pointsGainedColumnContent).x, GUI.skin.label.CalcSize(pointsGainedColumnContent).y);
        GUI.Label(pointsGainedColumnRect, pointsGainedColumnContent);

        Rect pointsLostColumnRect = new Rect(pointsGainedColumnRect.x + pointsGainedColumnRect.width + columnPadding, killsColumnRect.y, GUI.skin.label.CalcSize(pointsLostColumnContent).x, GUI.skin.label.CalcSize(pointsLostColumnContent).y);
        GUI.Label(pointsLostColumnRect, pointsLostColumnContent);

        Rect pointsBankedColumnRect = new Rect(pointsLostColumnRect.x + pointsLostColumnRect.width + columnPadding, killsColumnRect.y, GUI.skin.label.CalcSize(pointsBankedColumnContent).x, GUI.skin.label.CalcSize(pointsBankedColumnContent).y);
        GUI.Label(pointsBankedColumnRect, pointsBankedColumnContent);


        GameManager gm = GameManager.Instance;
        float playerPadding = statHeight * 0.05f;
        GUI.skin.label = redPlayerStyle;

        for (int i = 0; i < gm.players.Count; i++)
        {
            KBPlayer currentPlayer = gm.players[i];

            GUIContent cPlayerNameContent = new GUIContent(currentPlayer.name);
            Rect cPlayerNameRect = new Rect(statWidth * 0.1f, statHeight * 0.125f, GUI.skin.label.CalcSize(cPlayerNameContent).x, GUI.skin.label.CalcSize(cPlayerNameContent).y);
            GUI.Label(cPlayerNameRect, cPlayerNameContent);

            GUIContent cPlayerKillCountContent = new GUIContent(currentPlayer.killCount.ToString());
            Rect cPlayerKillCountRect = new Rect(killsColumnRect.x, killsColumnRect.y + (playerPadding * (i + 1)), GUI.skin.label.CalcSize(cPlayerKillCountContent).x, GUI.skin.label.CalcSize(cPlayerKillCountContent).y);
            GUI.Label(cPlayerKillCountRect, cPlayerKillCountContent);

            GUIContent cPlayerDeathCountContent = new GUIContent(currentPlayer.deathCount.ToString());
            Rect cPlayerDeathCountRect = new Rect(deathsColumnRect.x, deathsColumnRect.y + (playerPadding * (i + 1)), GUI.skin.label.CalcSize(cPlayerDeathCountContent).x, GUI.skin.label.CalcSize(cPlayerDeathCountContent).y);
            GUI.Label(cPlayerDeathCountRect, cPlayerDeathCountContent);

            GUIContent cPlayerPointsGainedContent = new GUIContent(currentPlayer.totalTokensGained.ToString());
            Rect cPlayerPointsGainedRect = new Rect(pointsGainedColumnRect.x, pointsGainedColumnRect.y + (playerPadding * (i + 1)), GUI.skin.label.CalcSize(cPlayerPointsGainedContent).x, GUI.skin.label.CalcSize(cPlayerPointsGainedContent).y);
            GUI.Label(cPlayerPointsGainedRect, cPlayerPointsGainedContent);

            GUIContent cPlayerPointsLostContent = new GUIContent(currentPlayer.totalTokensLost.ToString());
            Rect cPlayerPointsLostRect = new Rect(pointsLostColumnRect.x, pointsLostColumnRect.y + (playerPadding * (i + 1)), GUI.skin.label.CalcSize(cPlayerPointsLostContent).x, GUI.skin.label.CalcSize(cPlayerPointsLostContent).y);
            GUI.Label(cPlayerPointsLostRect, cPlayerPointsLostContent);

            GUIContent cPlayerPointsBankedContent = new GUIContent(currentPlayer.totalTokensBanked.ToString());
            Rect cPlayerPointsBankedRect = new Rect(pointsBankedColumnRect.x, pointsBankedColumnRect.y + (playerPadding * (i + 1)), GUI.skin.label.CalcSize(cPlayerPointsBankedContent).x, GUI.skin.label.CalcSize(cPlayerPointsBankedContent).y);
            GUI.Label(cPlayerPointsBankedRect, cPlayerPointsBankedContent);
        }
        //red players

        //middle

        //blue players

        GUI.EndGroup();
    }

    private void ShowEndGameTab()
    {
        float statX = Screen.width * 0.25f;
        float statY = Screen.height * 0.1f;
        float statWidth = Screen.width * 0.5f;
        float statHeight = Screen.height * 0.8f;
        Rect statTabRect = new Rect(statX, statY, statWidth, statHeight);

        GUI.BeginGroup(statTabRect);
        GUI.Box(new Rect(0, 0, statWidth, statHeight), "");
        //room name

        GUI.skin.label = headerStyle;
        GUIContent roomNameContent = new GUIContent("Room Name");
        Rect roomNameRect = new Rect(statWidth * 0.5f, 0, GUI.skin.label.CalcSize(roomNameContent).x, GUI.skin.label.CalcSize(roomNameContent).y);
        GUI.Label(roomNameRect, roomNameContent);

        //columns
        GUI.skin.label = columnStyle;
        float columnPadding = statWidth * 0.05f;

        Rect killsColumnRect = new Rect(statWidth * 0.3f, statHeight * 0.08f, GUI.skin.label.CalcSize(killsColumnContent).x, GUI.skin.label.CalcSize(killsColumnContent).y);
        GUI.Label(killsColumnRect, killsColumnContent);

        Rect deathsColumnRect = new Rect(killsColumnRect.x + killsColumnRect.width + columnPadding, killsColumnRect.y, GUI.skin.label.CalcSize(deathsColumnContent).x, GUI.skin.label.CalcSize(deathsColumnContent).y);
        GUI.Label(deathsColumnRect, deathsColumnContent);

        Rect pointsGainedColumnRect = new Rect(deathsColumnRect.x + deathsColumnRect.width + columnPadding, killsColumnRect.y, GUI.skin.label.CalcSize(pointsGainedColumnContent).x, GUI.skin.label.CalcSize(pointsGainedColumnContent).y);
        GUI.Label(pointsGainedColumnRect, pointsGainedColumnContent);

        Rect pointsLostColumnRect = new Rect(pointsGainedColumnRect.x + pointsGainedColumnRect.width + columnPadding, killsColumnRect.y, GUI.skin.label.CalcSize(pointsLostColumnContent).x, GUI.skin.label.CalcSize(pointsLostColumnContent).y);
        GUI.Label(pointsLostColumnRect, pointsLostColumnContent);

        Rect pointsBankedColumnRect = new Rect(pointsLostColumnRect.x + pointsLostColumnRect.width + columnPadding, killsColumnRect.y, GUI.skin.label.CalcSize(pointsBankedColumnContent).x, GUI.skin.label.CalcSize(pointsBankedColumnContent).y);
        GUI.Label(pointsBankedColumnRect, pointsBankedColumnContent);


        //red players
        GameManager gm = GameManager.Instance;
        float playerPadding = statHeight * 0.05f;
        GUI.skin.label = redPlayerStyle;

        for (int i = 0; i < gm.players.Count; i++)
        {
            KBPlayer currentPlayer = gm.players[i];

            GUIContent cPlayerNameContent = new GUIContent(currentPlayer.name);
            Rect cPlayerNameRect = new Rect(statWidth * 0.1f, statHeight * 0.125f, GUI.skin.label.CalcSize(cPlayerNameContent).x, GUI.skin.label.CalcSize(cPlayerNameContent).y);
            GUI.Label(cPlayerNameRect, cPlayerNameContent);

            GUIContent cPlayerKillCountContent = new GUIContent(currentPlayer.killCount.ToString());
            Rect cPlayerKillCountRect = new Rect(killsColumnRect.x, killsColumnRect.y + (playerPadding * (i + 1)), GUI.skin.label.CalcSize(cPlayerKillCountContent).x, GUI.skin.label.CalcSize(cPlayerKillCountContent).y);
            GUI.Label(cPlayerKillCountRect, cPlayerKillCountContent);

            GUIContent cPlayerDeathCountContent = new GUIContent(currentPlayer.deathCount.ToString());
            Rect cPlayerDeathCountRect = new Rect(deathsColumnRect.x, deathsColumnRect.y + (playerPadding * (i + 1)), GUI.skin.label.CalcSize(cPlayerDeathCountContent).x, GUI.skin.label.CalcSize(cPlayerDeathCountContent).y);
            GUI.Label(cPlayerDeathCountRect, cPlayerDeathCountContent);

            GUIContent cPlayerPointsGainedContent = new GUIContent(currentPlayer.totalTokensGained.ToString());
            Rect cPlayerPointsGainedRect = new Rect(pointsGainedColumnRect.x, pointsGainedColumnRect.y + (playerPadding * (i + 1)), GUI.skin.label.CalcSize(cPlayerPointsGainedContent).x, GUI.skin.label.CalcSize(cPlayerPointsGainedContent).y);
            GUI.Label(cPlayerPointsGainedRect, cPlayerPointsGainedContent);

            GUIContent cPlayerPointsLostContent = new GUIContent(currentPlayer.totalTokensLost.ToString());
            Rect cPlayerPointsLostRect = new Rect(pointsLostColumnRect.x, pointsLostColumnRect.y + (playerPadding * (i + 1)), GUI.skin.label.CalcSize(cPlayerPointsLostContent).x, GUI.skin.label.CalcSize(cPlayerPointsLostContent).y);
            GUI.Label(cPlayerPointsLostRect, cPlayerPointsLostContent);

            GUIContent cPlayerPointsBankedContent = new GUIContent(currentPlayer.totalTokensBanked.ToString());
            Rect cPlayerPointsBankedRect = new Rect(pointsBankedColumnRect.x, pointsBankedColumnRect.y + (playerPadding * (i + 1)), GUI.skin.label.CalcSize(cPlayerPointsBankedContent).x, GUI.skin.label.CalcSize(cPlayerPointsBankedContent).y);
            GUI.Label(cPlayerPointsBankedRect, cPlayerPointsBankedContent);
        }
        

        //middle

        //blue players
        GUI.skin.label = bluePlayerStyle;

        for (int i = 0; i < gm.players.Count; i++)
        {
            KBPlayer currentPlayer = gm.players[i];

            GUIContent cPlayerNameContent = new GUIContent(currentPlayer.name);
            Rect cPlayerNameRect = new Rect(statWidth * 0.1f, statHeight * 0.125f, GUI.skin.label.CalcSize(cPlayerNameContent).x, GUI.skin.label.CalcSize(cPlayerNameContent).y);
            GUI.Label(cPlayerNameRect, cPlayerNameContent);

            GUIContent cPlayerKillCountContent = new GUIContent(currentPlayer.killCount.ToString());
            Rect cPlayerKillCountRect = new Rect(killsColumnRect.x, killsColumnRect.y + (playerPadding * (i + 1)), GUI.skin.label.CalcSize(cPlayerKillCountContent).x, GUI.skin.label.CalcSize(cPlayerKillCountContent).y);
            GUI.Label(cPlayerKillCountRect, cPlayerKillCountContent);

            GUIContent cPlayerDeathCountContent = new GUIContent(currentPlayer.deathCount.ToString());
            Rect cPlayerDeathCountRect = new Rect(deathsColumnRect.x, deathsColumnRect.y + (playerPadding * (i + 1)), GUI.skin.label.CalcSize(cPlayerDeathCountContent).x, GUI.skin.label.CalcSize(cPlayerDeathCountContent).y);
            GUI.Label(cPlayerDeathCountRect, cPlayerDeathCountContent);

            GUIContent cPlayerPointsGainedContent = new GUIContent(currentPlayer.totalTokensGained.ToString());
            Rect cPlayerPointsGainedRect = new Rect(pointsGainedColumnRect.x, pointsGainedColumnRect.y + (playerPadding * (i + 1)), GUI.skin.label.CalcSize(cPlayerPointsGainedContent).x, GUI.skin.label.CalcSize(cPlayerPointsGainedContent).y);
            GUI.Label(cPlayerPointsGainedRect, cPlayerPointsGainedContent);

            GUIContent cPlayerPointsLostContent = new GUIContent(currentPlayer.totalTokensLost.ToString());
            Rect cPlayerPointsLostRect = new Rect(pointsLostColumnRect.x, pointsLostColumnRect.y + (playerPadding * (i + 1)), GUI.skin.label.CalcSize(cPlayerPointsLostContent).x, GUI.skin.label.CalcSize(cPlayerPointsLostContent).y);
            GUI.Label(cPlayerPointsLostRect, cPlayerPointsLostContent);

            GUIContent cPlayerPointsBankedContent = new GUIContent(currentPlayer.totalTokensBanked.ToString());
            Rect cPlayerPointsBankedRect = new Rect(pointsBankedColumnRect.x, pointsBankedColumnRect.y + (playerPadding * (i + 1)), GUI.skin.label.CalcSize(cPlayerPointsBankedContent).x, GUI.skin.label.CalcSize(cPlayerPointsBankedContent).y);
            GUI.Label(cPlayerPointsBankedRect, cPlayerPointsBankedContent);
        }

        //End Game Button
        
        GUI.skin.label = endGameStyle;
        Rect endGameRect = new Rect(statWidth * 0.5f, statHeight*0.95f, GUI.skin.label.CalcSize(endGameContent).x, GUI.skin.label.CalcSize(endGameContent).y);
        if (GUI.Button(endGameRect, endGameContent))
        {
            PhotonNetwork.LeaveRoom();
            Application.LoadLevel(0);
        }

        GUI.EndGroup();
    }

    private void Update()
    {
    }
}