using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class myBluetoothHandler : MonoBehaviour {
    private int devicesFound = 0;
	// Use this for initialization
    public Text screenText;
    public Text otherText;
    public Text errorText;
	void Start () {
        try { 
        BluetoothDeviceScript script = BluetoothLEHardwareInterface.Initialize(true, false, OnInitialized, OnFail);
        BluetoothLEHardwareInterface.ScanForPeripheralsWithServices(null, OnDeviceDiscovered);
        }catch(AndroidJavaException e){
            errorText.text = "error: " + e.Message;
        } catch ( MissingComponentException e) {
            errorText.text = "error: " + e.Message;

        } catch (UnityException e) {
            errorText.text = "error: " + e.Message;

        }
        screenText.text = "started scan";
	}
	
	// Update is called once per frame
	void Update () {
        
	}



    public void OnDeviceDiscovered(string name, string id) {
        errorText.text = "device found: " + name + " id: " + id;
        BluetoothLEHardwareInterface.StopScan();
    }

    public void OnInitialized() {
        otherText.text = "initialized";
        BluetoothLEHardwareInterface.StopScan();
        BluetoothLEHardwareInterface.ScanForPeripheralsWithServices(null, OnDeviceDiscovered);
    }

    public void OnFail(string error) {
        errorText.text = "error: " + error;
    }
}
