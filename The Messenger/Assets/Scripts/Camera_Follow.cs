using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Camera_Follow : MonoBehaviour
{
    [Header("ScreenX Value")]
    public float RightFacingValue;
    public float LeftFacingValue;
    [Header("Dampning")]
    public float MovingDamp;
    public float StaticDamp;
    int Input;
    CinemachineFramingTransposer VC;
    public PlayerMovement Pl_Move;
    public PlayerMovement StoneMove;
    public CinemachineConfiner2D Confiner;
    private void Awake()
    {
        VC = transform.GetChild(1).GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();
        Confiner = transform.GetChild(1).GetComponent<CinemachineConfiner2D>();
    }

    private void FixedUpdate()
    {
        if(Pl_Move != null)
            Input = Pl_Move.GetDir();
        else if (StoneMove != null)
            Input = StoneMove.GetDir();
        if (Input == 1)
        {
            VC.m_XDamping = MovingDamp;
            VC.m_ScreenX = RightFacingValue;
        }
        else if (Input == -1)
        {
            VC.m_XDamping = MovingDamp;
            VC.m_ScreenX = LeftFacingValue;
        }
        else
            VC.m_XDamping = StaticDamp;
    }
}
