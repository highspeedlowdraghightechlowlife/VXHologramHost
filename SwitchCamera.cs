using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SwitchCamera : MonoBehaviour
{
    public GameObject small_cam;
    public GameObject large_cam;
    public GameObject _ui_Menu;
    [SerializeField] TextMeshProUGUI output_text;
    public NPC small_NPC;
    public NPC large_NPC;
    public int Manager;
    public ManagerTestScript my_manager;

    public TMP_Dropdown cameraDropdown;
    public Button confirmButton;
    public Button basicButton;
    public Button smartButton;
    private int selectedCamera;
    [SerializeField]  public int selectedVersion;

    void Start()
    {
        // Add listener for the Dropdown value change
        cameraDropdown.onValueChanged.AddListener(delegate { DropdownValueChanged(cameraDropdown); });

        basicButton.onClick.AddListener(() => setAssistantType(0));
        smartButton.onClick.AddListener(() => setAssistantType(1));
        // Add listener for the Button click
        confirmButton.onClick.AddListener(ChangeCamera);
    }
        
    void DropdownValueChanged(TMP_Dropdown change)
    {
        selectedCamera = change.value;
        output_text.text = (change.value == 0) ? "Looking Glass 32-Inch Display" : "Looking Glass 65-Inch Display";

    }

    public void setAssistantType(int type)
    {
        if (type == 0)
        {
            selectedVersion = 0;
        }
        if (type == 1)
        {
            selectedVersion = 1;
        }

    }

    public void ChangeCamera()
    {
        _ui_Menu.SetActive(false);
        ManageCamera();
    }

    private void ManageCamera()
    {
        if (selectedCamera == 0)
        {
            Cam_1();
            Manager = 0;
        }
        else
        {
            Cam_2();
            Manager = 1;
        }
    }

    private void Cam_1()
    {
        small_cam.SetActive(true);
        large_cam.SetActive(false);

        large_NPC.Destroy();
        my_manager.setNPC(small_NPC);
    }

    private void Cam_2()
    {
        small_cam.SetActive(false);
        large_cam.SetActive(true);

        small_NPC.Destroy();
        my_manager.setNPC(large_NPC);

    }
}
