using System;
using UnityEngine;

[Serializable]
public class TankManager
{
    public Color m_PlayerColor;       // The color of the tank   
    public Transform m_SpawnPoint;    // The spawn point of the tank      
    [HideInInspector] public int m_PlayerNumber;  // It is set in the Game Manager and then is transferred to the TankMovement and TankShooting scripts           
    [HideInInspector] public string m_ColoredPlayerText; // For an HTML string to contain info for player's color
    [HideInInspector] public GameObject m_Instance;      // Instance of a tank spawned via the Game Manager and transferred to this class   
    [HideInInspector] public int m_Wins;             // Number of wins this tank has made       


    private TankMovement m_Movement;        // Reference to the <TankMovement>() component of the tank's instance
    private TankShooting m_Shooting;        // Reference to the <TankShooting>() component of the tank's instance
    private GameObject m_CanvasGameObject;  // Reference to the <m_CanvasGameObject>() component of the tank's instance


    public void Setup()
    {
        m_Movement = m_Instance.GetComponent<TankMovement>();
        m_Shooting = m_Instance.GetComponent<TankShooting>();
        m_CanvasGameObject = m_Instance.GetComponentInChildren<Canvas>().gameObject;

        m_Movement.m_PlayerNumber = m_PlayerNumber;
        m_Shooting.m_PlayerNumber = m_PlayerNumber;

        m_ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_PlayerColor) + ">PLAYER " + m_PlayerNumber + "</color>";

        MeshRenderer[] renderers = m_Instance.GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = m_PlayerColor;
        }
    }


    public void DisableControl()
    {
        m_Movement.enabled = false;
        m_Shooting.enabled = false;

        m_CanvasGameObject.SetActive(false);
    }


    public void EnableControl()
    {
        m_Movement.enabled = true;
        m_Shooting.enabled = true;

        m_CanvasGameObject.SetActive(true);
    }


    public void Reset()
    {
        m_Instance.transform.position = m_SpawnPoint.position;
        m_Instance.transform.rotation = m_SpawnPoint.rotation;

        m_Instance.SetActive(false);
        m_Instance.SetActive(true);
    }
}
