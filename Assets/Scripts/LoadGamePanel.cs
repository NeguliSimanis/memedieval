using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadGamePanel : MonoBehaviour
{

    /* TO-DO:
     * >> get moveSpeed from Player preferences
     */
    #region variables
    [SerializeField]
    float moveSpeed = 25f;

    [SerializeField]
    Button disablePanelButton;

    [SerializeField]
    RectTransform activePanelPosition;
    [SerializeField]
    RectTransform inactivePanelPosition;

    private bool isMovingUp = false;
    private bool isMovingDown = false;

    private float targetPositionY;
    #endregion

    private void Start()
    {
        Button disableButton = disablePanelButton.GetComponent<Button>();
        disableButton.onClick.AddListener(DisablePanel);
    }

    #region Button functions
    public void ActivatePanel()
    {
        isMovingUp = true;
        targetPositionY = activePanelPosition.localPosition.y;
    }

    private void DisablePanel()
    {
        isMovingDown = true;
        targetPositionY = inactivePanelPosition.localPosition.y;
    }
    #endregion

    void Update()
    {
        if (isMovingUp)
        {
            gameObject.transform.Translate(0f, moveSpeed, 0f);
            if (gameObject.transform.position.y > targetPositionY)
            {
                isMovingUp = false;
            }
           
        }

        if (isMovingDown)
        {
            gameObject.transform.Translate(0f, -moveSpeed, 0f);
            if (gameObject.transform.position.y < targetPositionY)
            {
                isMovingDown = false;
            }
        }
    }
}

