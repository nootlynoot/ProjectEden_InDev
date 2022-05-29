using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using Pathfinding;

public class LevelManager : MonoBehaviour
{
    public int targetFrameRate = 30;
    [Space(10)]
    public PlayerCon[] ActivePlayers;

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFrameRate;
        ReInput.ControllerConnectedEvent += OnControllerConnected;
    }

    private void Start()
    {
        var graphToScan = AstarPath.active.data.gridGraph;
        AstarPath.active.Scan(graphToScan);
    }

    // Update is called once per frame
    void Update()
    {
        ActivePlayers = FindObjectsOfType<PlayerCon>();
    }

    //put instantiate pick ups here do a public void spawnpickups(vector3 objectPos) 

    #region ("IF CONTROLLER GETS CONNECTED ON RUNTIME)
    void OnControllerConnected(ControllerStatusChangedEventArgs args)
    {
        if (args.controllerType != ControllerType.Joystick) return; // skip if this isn't a Joystick
        //Assign joystick to first player that doesnt have any assigned
        AssignJoystickToNextOpenPlayer(ReInput.controllers.GetJoystick(args.controllerId));
    }
    void AssignJoystickToNextOpenPlayer(Joystick j)
    {
        foreach (Player p in ReInput.players.Players)
        {
            if (p.controllers.joystickCount > 0) continue; // player already has a joystick
            p.controllers.AddController(j, true); // assign joystick to player
            return;
        }
    }
    #endregion
}
