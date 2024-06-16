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
    //public Dropdown cameraDropdown;
    public TMP_Dropdown cameraDropdown;
    public Button confirmButton;
    private int selectedCamera;

    void Start()
    {
        // Add listener for the Dropdown value change
        cameraDropdown.onValueChanged.AddListener(delegate { DropdownValueChanged(cameraDropdown); });
        // Add listener for the Button click
        confirmButton.onClick.AddListener(ChangeCamera);
    }

    void DropdownValueChanged(TMP_Dropdown change)
    {
        selectedCamera = change.value;
        output_text.text = (change.value == 0) ? "Looking Glass 32-Inch Display" : "Looking Glass 60-Inch Display";

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
        //small_NPC.SetActive(true);
        large_cam.SetActive(false);
        //large_NPC.SetActive(false);
        //my_manager.resultTarget = small_NPC;
        large_NPC.Destroy();
        my_manager.setNPC(small_NPC);
    }

    private void Cam_2()
    {
        small_cam.SetActive(false);
        // small_NPC.SetActive(false);
        large_cam.SetActive(true);
        //  large_NPC.SetActive(true);
        small_NPC.Destroy();
        my_manager.setNPC(large_NPC);

    }
}
