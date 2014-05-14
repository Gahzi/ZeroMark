using UnityEngine;

public class GUIManager : MonoBehaviour
{
    public enum GUIManagerState { Hidden, ShowingStatTab, ShowingEndGameTab }

    public GUIManagerState state = GUIManagerState.Hidden;

    private GUIContent killsColumnContent = new GUIContent("Kills");
    private GUIContent deathsColumnContent = new GUIContent("Deaths");
    private GUIContent pointsGainedColumnContent = new GUIContent("Points Gained");
    private GUIContent pointsLostColumnContent = new GUIContent("Points Lost");
    private GUIContent pointsBankedColumnContent = new GUIContent("Points Banked");

    private GUIContent endGameContent = new GUIContent("End Game");

    public GUIStyle headerStyle;
    public GUIStyle columnStyle;
    public GUIStyle redPlayerStyle;
    public GUIStyle bluePlayerStyle;
    public GUIStyle nonePlayerStyle;
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
        switch (GameManager.Instance.gameType)
        {
            case GameManager.GameType.CapturePoint:
                {
                    break;
                }

            case GameManager.GameType.DataPulse:
                {
                    break;
                }

            case GameManager.GameType.Deathmatch:
                {
                    break;
                }
        }

        GameManager.Instance.localPlayer.playerCamera.redScoreText.text = "00";
        GameManager.Instance.localPlayer.playerCamera.blueScoreText.text = "00";
    }

    private void OnGUI()
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
        Rect statTabRect = new Rect(statX, statY, statWidth, statHeight);

        GUI.BeginGroup(statTabRect);
        GUI.Box(new Rect(0, 0, statWidth, statHeight), "");
        //room name

        GUI.skin.label = headerStyle;
        GUIContent roomNameContent = new GUIContent(PhotonNetwork.room.name);
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

        GameManager gm = GameManager.Instance;
        float playerPadding = statHeight * 0.05f;

        //red players
        for (int i = 0; i < gm.players.Count; i++)
        {
            KBPlayer currentPlayer = gm.players[i];

            if (currentPlayer.team == KBConstants.Team.Red)
            {
                GUI.skin.label = redPlayerStyle;
            }
            else if (currentPlayer.team == KBConstants.Team.Blue)
            {
                GUI.skin.label = bluePlayerStyle;
            }
            else
            {
                GUI.skin.label = nonePlayerStyle;
            }

            GUIContent cPlayerNameContent = new GUIContent(currentPlayer.name);
            Rect cPlayerNameRect = new Rect(statWidth * 0.1f, statHeight * 0.125f + (playerPadding * (i)), GUI.skin.label.CalcSize(cPlayerNameContent).x, GUI.skin.label.CalcSize(cPlayerNameContent).y);
            GUI.Label(cPlayerNameRect, cPlayerNameContent);

            GUIContent cPlayerKillCountContent = new GUIContent(currentPlayer.killCount.ToString());
            Rect cPlayerKillCountRect = new Rect(killsColumnRect.x, killsColumnRect.y + (playerPadding * (i + 1)), GUI.skin.label.CalcSize(cPlayerKillCountContent).x, GUI.skin.label.CalcSize(cPlayerKillCountContent).y);
            GUI.Label(cPlayerKillCountRect, cPlayerKillCountContent);

            GUIContent cPlayerDeathCountContent = new GUIContent(currentPlayer.deathCount.ToString());
            Rect cPlayerDeathCountRect = new Rect(deathsColumnRect.x, deathsColumnRect.y + (playerPadding * (i + 1)), GUI.skin.label.CalcSize(cPlayerDeathCountContent).x, GUI.skin.label.CalcSize(cPlayerDeathCountContent).y);
            GUI.Label(cPlayerDeathCountRect, cPlayerDeathCountContent);

            GUIContent cPlayerPointsGainedContent = new GUIContent(currentPlayer.totalPointsGained.ToString());
            Rect cPlayerPointsGainedRect = new Rect(pointsGainedColumnRect.x, pointsGainedColumnRect.y + (playerPadding * (i + 1)), GUI.skin.label.CalcSize(cPlayerPointsGainedContent).x, GUI.skin.label.CalcSize(cPlayerPointsGainedContent).y);
            GUI.Label(cPlayerPointsGainedRect, cPlayerPointsGainedContent);

            GUIContent cPlayerPointsLostContent = new GUIContent(currentPlayer.totalPointsLost.ToString());
            Rect cPlayerPointsLostRect = new Rect(pointsLostColumnRect.x, pointsLostColumnRect.y + (playerPadding * (i + 1)), GUI.skin.label.CalcSize(cPlayerPointsLostContent).x, GUI.skin.label.CalcSize(cPlayerPointsLostContent).y);
            GUI.Label(cPlayerPointsLostRect, cPlayerPointsLostContent);

            GUIContent cPlayerPointsBankedContent = new GUIContent(currentPlayer.totalPointsBanked.ToString());
            Rect cPlayerPointsBankedRect = new Rect(pointsBankedColumnRect.x, pointsBankedColumnRect.y + (playerPadding * (i + 1)), GUI.skin.label.CalcSize(cPlayerPointsBankedContent).x, GUI.skin.label.CalcSize(cPlayerPointsBankedContent).y);
            GUI.Label(cPlayerPointsBankedRect, cPlayerPointsBankedContent);
        }

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
        GUIContent roomNameContent = new GUIContent(PhotonNetwork.room.name);
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
            if (currentPlayer != null)
            {
                if (currentPlayer.team == KBConstants.Team.Red)
                {
                    GUI.skin.label = redPlayerStyle;
                }
                else if (currentPlayer.team == KBConstants.Team.Blue)
                {
                    GUI.skin.label = bluePlayerStyle;
                }
                else
                {
                    GUI.skin.label = nonePlayerStyle;
                }

                GUIContent cPlayerNameContent = new GUIContent(currentPlayer.name);
                Rect cPlayerNameRect = new Rect(statWidth * 0.1f, statHeight * 0.125f + (playerPadding * (i)), GUI.skin.label.CalcSize(cPlayerNameContent).x, GUI.skin.label.CalcSize(cPlayerNameContent).y);
                GUI.Label(cPlayerNameRect, cPlayerNameContent);

                GUIContent cPlayerKillCountContent = new GUIContent(currentPlayer.killCount.ToString());
                Rect cPlayerKillCountRect = new Rect(killsColumnRect.x, killsColumnRect.y + (playerPadding * (i + 1)), GUI.skin.label.CalcSize(cPlayerKillCountContent).x, GUI.skin.label.CalcSize(cPlayerKillCountContent).y);
                GUI.Label(cPlayerKillCountRect, cPlayerKillCountContent);

                GUIContent cPlayerDeathCountContent = new GUIContent(currentPlayer.deathCount.ToString());
                Rect cPlayerDeathCountRect = new Rect(deathsColumnRect.x, deathsColumnRect.y + (playerPadding * (i + 1)), GUI.skin.label.CalcSize(cPlayerDeathCountContent).x, GUI.skin.label.CalcSize(cPlayerDeathCountContent).y);
                GUI.Label(cPlayerDeathCountRect, cPlayerDeathCountContent);

                GUIContent cPlayerPointsGainedContent = new GUIContent(currentPlayer.totalPointsGained.ToString());
                Rect cPlayerPointsGainedRect = new Rect(pointsGainedColumnRect.x, pointsGainedColumnRect.y + (playerPadding * (i + 1)), GUI.skin.label.CalcSize(cPlayerPointsGainedContent).x, GUI.skin.label.CalcSize(cPlayerPointsGainedContent).y);
                GUI.Label(cPlayerPointsGainedRect, cPlayerPointsGainedContent);

                GUIContent cPlayerPointsLostContent = new GUIContent(currentPlayer.totalPointsLost.ToString());
                Rect cPlayerPointsLostRect = new Rect(pointsLostColumnRect.x, pointsLostColumnRect.y + (playerPadding * (i + 1)), GUI.skin.label.CalcSize(cPlayerPointsLostContent).x, GUI.skin.label.CalcSize(cPlayerPointsLostContent).y);
                GUI.Label(cPlayerPointsLostRect, cPlayerPointsLostContent);

                GUIContent cPlayerPointsBankedContent = new GUIContent(currentPlayer.totalPointsBanked.ToString());
                Rect cPlayerPointsBankedRect = new Rect(pointsBankedColumnRect.x, pointsBankedColumnRect.y + (playerPadding * (i + 1)), GUI.skin.label.CalcSize(cPlayerPointsBankedContent).x, GUI.skin.label.CalcSize(cPlayerPointsBankedContent).y);
                GUI.Label(cPlayerPointsBankedRect, cPlayerPointsBankedContent);
            }
        }

        //End Game Button

        GUI.skin.label = endGameStyle;
        Rect endGameRect = new Rect(statWidth * 0.5f, statHeight * 0.95f, GUI.skin.label.CalcSize(endGameContent).x, GUI.skin.label.CalcSize(endGameContent).y);
        if (GUI.Button(endGameRect, endGameContent))
        {
            PhotonNetwork.LeaveRoom();
            Application.LoadLevel(0);
        }

        GUI.EndGroup();
    }

    private void Update()
    {
        float remainingGameTime = 0f;
        int remainingMins, remainingSecs = 0;
        remainingGameTime = KBConstants.GameConstants.maxGameTimeCapturePoint - GameManager.Instance.gameTime;
        remainingMins = Mathf.FloorToInt(remainingGameTime / 60.0f);
        remainingSecs = (int)(remainingGameTime - (remainingMins * 60));
        string time = remainingMins.ToString("00") + ":" + remainingSecs.ToString("00");

        switch (GameManager.Instance.gameType)
        {
            case GameManager.GameType.CapturePoint:
                GameManager.Instance.localPlayer.playerCamera.redScoreText.text = GameManager.Instance.redTeamScore.ToString("00");
                GameManager.Instance.localPlayer.playerCamera.blueScoreText.text = GameManager.Instance.blueTeamScore.ToString("00");
                remainingGameTime = KBConstants.GameConstants.maxGameTimeCapturePoint - GameManager.Instance.gameTime;
                GameManager.Instance.localPlayer.playerCamera.timeRemainingNumberText.text = time;
                GameManager.Instance.localPlayer.playerCamera.redHeldPointTotalText.text = GameManager.Instance.redHeldPointTotal.ToString("00");
                GameManager.Instance.localPlayer.playerCamera.blueHeldPointTotalText.text = GameManager.Instance.blueHeldPointTotal.ToString("00");
                break;

            case GameManager.GameType.DataPulse:

                switch (GameManager.Instance.state)
                {
                    case GameManager.GameState.PreGame:
                        //((Mathf.Sin(Time.time * speed) + 0.5f) * (lightMaxIntensity - lightMinIntensity)) + lightMinIntensity
                        float speed = 8.00f;
                        float min = 0.3f;
                        float max = 1.0f;
                        float alpha = ((Mathf.Sin(Time.time * speed) + 0.5f) * (max - min)) + min;
                        GameManager.Instance.localPlayer.playerCamera.dataPulseCountdown.GetComponent<TextMesh>().color = new Color(1.0f, 0, 0.0f, alpha);

                        GameManager.Instance.localPlayer.playerCamera.dataPulseCountdown.text = GameManager.Instance.preGameWaitTime.ToString("00");


                        break;

                    case GameManager.GameState.InGame:
                        GameManager.Instance.localPlayer.playerCamera.dataPulseCountdown.text = remainingSecs.ToString("00");

                        if (GameManager.Instance.timeToNextPulse < 10.0001f)
                        {
                            GameManager.Instance.localPlayer.playerCamera.dataPulseCountdown.color = new Color(255, 0, 255, 255);
                        }
                        else
                        {
                            GameManager.Instance.localPlayer.playerCamera.dataPulseCountdown.color = new Color(255, 255, 255, 137);
                        }

                        break;

                    case GameManager.GameState.RedWins:
                        break;

                    case GameManager.GameState.BlueWins:
                        break;

                    case GameManager.GameState.Tie:
                        break;

                    case GameManager.GameState.EndGame:
                        break;

                    default:
                        break;
                }

                GameManager.Instance.localPlayer.playerCamera.redScoreText.text = GameManager.Instance.redTeamScore.ToString("00");
                GameManager.Instance.localPlayer.playerCamera.blueScoreText.text = GameManager.Instance.blueTeamScore.ToString("00");
                GameManager.Instance.localPlayer.playerCamera.redHeldPointTotalText.text = GameManager.Instance.redHeldPointTotal.ToString("00");
                GameManager.Instance.localPlayer.playerCamera.blueHeldPointTotalText.text = GameManager.Instance.blueHeldPointTotal.ToString("00");
                remainingGameTime = KBConstants.GameConstants.maxGameTimeDataPulse - GameManager.Instance.gameTime;
                GameManager.Instance.localPlayer.playerCamera.timeRemainingNumberText.text = time;


                break;

            case GameManager.GameType.Deathmatch:
                GameManager.Instance.localPlayer.playerCamera.redScoreText.text = GameManager.Instance.redTeamScore.ToString("00");
                GameManager.Instance.localPlayer.playerCamera.blueScoreText.text = GameManager.Instance.blueTeamScore.ToString("00");
                int totalToWin = KBConstants.GameConstants.maxScoreDeathmatch;
                GameManager.Instance.localPlayer.playerCamera.timeRemainingNumberText.text = totalToWin.ToString();
                GameManager.Instance.localPlayer.playerCamera.redHeldPointTotalText.text = GameManager.Instance.redHeldPointTotal.ToString("00");
                GameManager.Instance.localPlayer.playerCamera.blueHeldPointTotalText.text = GameManager.Instance.blueHeldPointTotal.ToString("00");

                break;

            default:
                break;
        }
    }
}