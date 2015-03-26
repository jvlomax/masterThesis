﻿using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class CentralTISensorTagScript : MonoBehaviour
{
	public Transform PanelCentral;
	public Text Name;
	public Text Address;
	public Text TextConnectButton;

	public GameObject PanelTemperature;
	public Text TextTemperatureEnable;
	public Text TextTemperatureAmbient;
	public Text TextTemperatureTarget;

	private string _temperatureServiceUUID = FullUUID ("AA00");
	private string _temperatureReadWriteUUID = FullUUID ("AA01");
	private string _temperatureConfigureUUID = FullUUID ("AA02");

	private bool _connecting = false;
	private string _connectedID = null;

	bool _connected = false;
	bool Connected
	{
		get { return _connected; }
		set
		{
			if (_connected != value)
			{
				_connected = value;
				
				if (_connected)
				{
					TextConnectButton.text = "Disconnect";
					TemperatureEnabled = false;
					_connecting = false;
				}
				else
				{
					TextConnectButton.text = "Connect";
					PanelTemperature.SetActive (false);
					_connectedID = null;
				}
			}
		}
	}
	
	public void Initialize (CentralPeripheralButtonScript centralPeripheralButtonScript)
	{
		Connected = false;
		Name.text = centralPeripheralButtonScript.TextName.text;
		Address.text = centralPeripheralButtonScript.TextAddress.text;
		TextTemperatureAmbient.text = "...";
		TextTemperatureTarget.text = "...";
	}
	
	void disconnect (Action<string> action)
	{
		if (TemperatureEnabled)
			TemperatureEnabled = false;
		else
			BluetoothLEHardwareInterface.DisconnectPeripheral (Address.text, action);
	}
	
	public void OnBack ()
	{
		if (Connected)
		{
			disconnect ((Address) => {
				
				Connected = false;
				BLETestScript.Show (PanelCentral.transform);
			});
		}
		else
			BLETestScript.Show (PanelCentral.transform);
	}

	void OnCharacteristicNotification (string characteristicUUID, byte[] bytes)
	{
		if (IsEqual(characteristicUUID, _temperatureReadWriteUUID))
		{
			if (bytes.Length >= 4)
			{
				float ambient = AmbientTemperature (bytes);
				float target = TargetTemperature (bytes, ambient);

				TextTemperatureAmbient.text = ambient.ToString ();
				TextTemperatureTarget.text = target.ToString ();
			}
		}
	}

	private bool _temperatureEnabled = false;
	private bool TemperatureEnabled
	{
		get { return _temperatureEnabled; }
		set
		{
			if (_temperatureEnabled != value)
			{
				if (_temperatureEnabled)
				{
					EnableFeature (_temperatureServiceUUID, _temperatureConfigureUUID, false, (enableCharacteristicUUID) => {

						if (IsEqual (enableCharacteristicUUID, _temperatureConfigureUUID))
						{
							BluetoothLEHardwareInterface.UnSubscribeCharacteristic (_connectedID, _temperatureServiceUUID, _temperatureReadWriteUUID, (characteristicUUID) => {

								TextTemperatureEnable.text = "Enable";
								TextTemperatureAmbient.text = "...";
								TextTemperatureTarget.text = "...";
								_temperatureEnabled = value;
							});
						}
					});
				}
				else
				{
					EnableFeature (_temperatureServiceUUID, _temperatureConfigureUUID, true, (enableCharacteristicUUID) => {
						
						if (IsEqual (enableCharacteristicUUID, _temperatureConfigureUUID))
						{
							BluetoothLEHardwareInterface.SubscribeCharacteristic (_connectedID, _temperatureServiceUUID, _temperatureReadWriteUUID, (characteristicUUID) => {
						
								TextTemperatureEnable.text = "Disable";
								_temperatureEnabled = value;

							}, OnCharacteristicNotification);
						}
					});
				}
			}
		}
	}

	public void OnTemperatureEnable ()
	{
		TemperatureEnabled = !TemperatureEnabled;
	}
	
	public void OnConnect ()
	{
		if (!_connecting)
		{
			if (Connected)
			{
				disconnect ((Address) => {
					Connected = false;
				});
			}
			else
			{
				BusyScript.IsBusy = true;

				BluetoothLEHardwareInterface.ConnectToPeripheral (Address.text, (address) => {

					_connectedID = address;
					Connected = true;
				},
				(address, serviceUUID) => {
				},
				(address, serviceUUID, characteristicUUID) => {

					// temperature service
					if (IsEqual(serviceUUID, _temperatureServiceUUID))
					{
						if (IsEqual(characteristicUUID, _temperatureReadWriteUUID))
						{
							PanelTemperature.SetActive (true);
							BusyScript.IsBusy = false;
						}
					}
				});
				
				_connecting = true;
			}
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

	// this calculation comes from the TI web site for the sensortag
	float AmbientTemperature (byte[] bytes)
	{
		return ((float)ShortUnsignedAtOffset (bytes, 2)) / 128f;
	}

	// this calculation comes from the TI web site for the sensortag
	float TargetTemperature (byte[] bytes, float ambient)
	{
		int twoByteValue = ShortSignedAtOffset (bytes, 0);

		float Vobj2 = (float)Convert.ToDouble (twoByteValue);

		Vobj2 *= 0.00000015625f;
		
		float Tdie = ambient + 273.15f;
		
		float S0 = 5.593E-14f;	// Calibration factor
		float a1 = 1.75E-3f;
		float a2 = -1.678E-5f;
		float b0 = -2.94E-5f;
		float b1 = -5.7E-7f;
		float b2 = 4.63E-9f;
		float c2 = 13.4f;
		float Tref = 298.15f;
		float S = S0*(1+a1*(Tdie - Tref)+a2*Mathf.Pow((Tdie - Tref),2f));
		float Vos = b0 + b1*(Tdie - Tref) + b2*Mathf.Pow((Tdie - Tref),2f);
		float fObj = (Vobj2 - Vos) + c2*Mathf.Pow((Vobj2 - Vos),2f);
		float tObj = Mathf.Pow(Mathf.Pow(Tdie,4f) + (fObj/S),.25f);
		
		return tObj - 273.15f;
	}
	
	int ShortSignedAtOffset (byte[] bytes, int offset)
	{
		int lowerByte = bytes[offset];
		int upperByte = (char)(bytes[offset + 1]);
		return ((upperByte << 8) + lowerByte);
	}

	int ShortUnsignedAtOffset (byte[] bytes, int offset)
	{
		int lowerByte = bytes[offset];
		int upperByte = bytes[offset + 1];
		return ((upperByte << 8) + lowerByte);
	}

	void EnableFeature (string serviceUUID, string configurationUUID, bool enable, Action<string> action)
	{
		if (enable)
			SendByte (serviceUUID, configurationUUID, 0x01, action);
		else
			SendByte (serviceUUID, configurationUUID, 0x00, action);
	}

	// this will create a full UUID from a 16 bit UUID specifically for the sensortag
	static string FullUUID (string uuid)
	{
		return "F000" + uuid + "-0451-4000-B000-000000000000";
	}

	bool IsEqual(string uuid1, string uuid2)
	{
		if (uuid1.Length == 4)
			uuid1 = FullUUID (uuid1);
		if (uuid2.Length == 4)
			uuid2 = FullUUID (uuid2);

		return (uuid1.ToUpper().CompareTo(uuid2.ToUpper()) == 0);
	}
	
	void SendByte (string serviceUUID, string characteristicUUID, byte value, Action<string> action)
	{
		BluetoothLEHardwareInterface.Log(string.Format ("Sending: {0}", value));
		byte[] data = new byte[] { value };
		BluetoothLEHardwareInterface.WriteCharacteristic (_connectedID, serviceUUID, characteristicUUID, data, data.Length, action != null, action);
	}
	
	void SendBytes (string serviceUUID, string characteristicUUID, byte[] data, Action<string> action)
	{
		BluetoothLEHardwareInterface.Log(string.Format ("Sending: {0}", data));
		BluetoothLEHardwareInterface.WriteCharacteristic (_connectedID, serviceUUID, characteristicUUID, data, data.Length, action != null, action);
	}
}
