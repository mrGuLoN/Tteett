using System;
using _Scripts.Player.BasePlayer.BaseStateMachine;
using UnityEngine;

public class PlayerWebController : MonoBehaviour
{
    public BasePlayerControllerSm _prefab;

    private void Start()
    {
        LaboratoryGenerator.Instance.Done.AddListener(StartGame);
    }

    private void StartGame()
    {
        var inst = Instantiate(_prefab);
        inst.fireJoyStick = CanvasSingleTone.instance.fireJoyStick;
        inst.movementJoyStick = CanvasSingleTone.instance.movementJoyStick;
        CanvasSingleTone.instance.SetTarget(inst.transform);
    }
}
