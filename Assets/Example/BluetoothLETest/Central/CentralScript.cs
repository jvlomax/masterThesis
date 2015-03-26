using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CentralScript : MonoBehaviour
{
	public Transform PanelTypeSelection;
	public Transform PanelScrollContents;
	public CentralRFduinoScript PanelCentralRFduino;
	public CentralTISensorTagScript PanelCentralTISensorTag;
	public GameObject CentralPeripheralButtonPrefab;
	public Text TextScanButton;

	private Dictionary<string, CentralPeripheralButtonScript> _peripheralList;
	private bool _scanning = false;

	public void Initialize ()
	{
		BluetoothLEHardwareInterface.Initialize (true, false, () => {
			
		}, (error) => {
		});
	}

	public void OnBack ()
	{
		RemovePeripherals ();

		if (_scanning)
			OnScan (); // this will stop scanning

		BluetoothLEHardwareInterface.DeInitialize (() => {
			BLETestScript.Show (PanelTypeSelection);
		});
	}

	public void OnScan ()
	{
		if (_scanning)
		{
			BluetoothLEHardwareInterface.StopScan ();
			TextScanButton.text = "Start Scan";
			_scanning = false;
		}
		else
		{
			RemovePeripherals ();

			BluetoothLEHardwareInterface.ScanForPeripheralsWithServices (null, (address, name) => {

				AddPeripheral (name, address);
			});

			TextScanButton.text = "Stop Scan";
			_scanning = true;
		}
	}

	void RemovePeripherals ()
	{
		for (int i = 0; i < PanelScrollContents.childCount; ++i)
		{
			GameObject gameObject = PanelScrollContents.GetChild (i).gameObject;
			Destroy (gameObject);
		}
		
		if (_peripheralList != null)
			_peripheralList.Clear ();
	}

	void AddPeripheral (string name, string address)
	{
		if (_peripheralList == null)
			_peripheralList = new Dictionary<string, CentralPeripheralButtonScript> ();

		if (!_peripheralList.ContainsKey (address))
		{
			GameObject peripheralObject = (GameObject)Instantiate (CentralPeripheralButtonPrefab);
			CentralPeripheralButtonScript script = peripheralObject.GetComponent<CentralPeripheralButtonScript> ();
			script.TextName.text = name;
			script.TextAddress.text = address;
			script.PanelCentralRFduino = PanelCentralRFduino;
			script.PanelCentralTISensorTag = PanelCentralTISensorTag;
			peripheralObject.transform.SetParent (PanelScrollContents);
			peripheralObject.transform.localScale = new Vector3 (1f, 1f, 1f);

			_peripheralList[address] = script;
		}
	}

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	}
}
