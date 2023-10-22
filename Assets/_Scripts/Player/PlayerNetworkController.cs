using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerNetworkController : NetworkBehaviour
{
    [SerializeField] private NetworkAnimator _networkAnimator;
    [SerializeField] private BasePlayerStateMachine _playerPrefab;

    private BasePlayerStateMachine _playerSM;
    void Start()
    {
        _playerSM = Instantiate(_playerPrefab);
        _networkAnimator.animator = _playerSM.GetComponent<Animator>();
    }

    private void Update()
    {
        _playerSM.UpdateSM();
    }

    private void FixedUpdate()
    {
        _playerSM.FixedUpdateSM();
    }

    private void LateUpdate()
    {
        _playerSM.LateUpdateSM();
    }

    [Server]
    private void InstancePlayer()
    {
        var player = Instantiate(_playerPrefab);
        _networkAnimator.animator = player.GetComponent<Animator>();
    }
}
