// Dynamic link library for getting information from win32 api
// Creator: Tymoshenko Oleh
// Course project 2020
#include "BatteryInfo_Win32.h"

extern "C" __declspec(dllexport) bool GetBatteryInfo(BATTERY_INFORMATION * batInfo)
{
	// Get handle to battery device
	HDEVINFO hdev = SetupDiGetClassDevs(&GUID_DEVCLASS_BATTERY,
										0,
										0,
										DIGCF_PRESENT | DIGCF_DEVICEINTERFACE);
	if (INVALID_HANDLE_VALUE != hdev)
	{
		// struct for contain device interface
		SP_DEVICE_INTERFACE_DATA did = { 0 };
		did.cbSize = sizeof(did);

		// function for getting device (battery) intefrace (enumerate batterys)
		if (SetupDiEnumDeviceInterfaces(hdev,
										0,
										&GUID_DEVCLASS_BATTERY,
										0,
										&did))
		{
			DWORD cbRequired = 0; // required size for PSP_DEVICE_INTERFACE_DETAIL_DATA

			// call funtion without params for getting required size
			SetupDiGetDeviceInterfaceDetail(hdev,
											&did,
											0,
											0,
											&cbRequired,
											0);
			if (ERROR_INSUFFICIENT_BUFFER == GetLastError()) // after calling func for getting required size, function return ERROR_INSUFFICIENT_BUFFER
			{
				// alloc required memory for struct
				PSP_DEVICE_INTERFACE_DETAIL_DATA pdidd = (PSP_DEVICE_INTERFACE_DETAIL_DATA) LocalAlloc(LPTR, cbRequired);
				if (pdidd) // if memory allocated
				{
					pdidd->cbSize = sizeof(*pdidd);

					// get detailed information about battery interface
					if (SetupDiGetDeviceInterfaceDetail(hdev,
														&did,
														pdidd,
														cbRequired,
														&cbRequired,
														0))
					{
						// Enumerated a battery.  Ask it for information.
						HANDLE hBattery = CreateFile(pdidd->DevicePath,
												  	 GENERIC_READ | GENERIC_WRITE,
													 FILE_SHARE_READ | FILE_SHARE_WRITE,
													 NULL,
													 OPEN_EXISTING,
													 FILE_ATTRIBUTE_NORMAL,
													 NULL);
						if (INVALID_HANDLE_VALUE != hBattery)
						{
							BATTERY_QUERY_INFORMATION bqi = { 0 };

							DWORD dwWait = 0;
							DWORD dwOut;

							// Ask the battery for its tag.
							if (DeviceIoControl(hBattery,
												IOCTL_BATTERY_QUERY_TAG,
												&dwWait,
												sizeof(dwWait),
												&bqi.BatteryTag,
												sizeof(bqi.BatteryTag),
												&dwOut,
												NULL)
												&& bqi.BatteryTag)
							{
								// query the battery info with the tag
								BATTERY_INFORMATION bi = { 0 };
								bqi.InformationLevel = BatteryInformation;

								if (DeviceIoControl(hBattery,
									IOCTL_BATTERY_QUERY_INFORMATION,
									&bqi,
									sizeof(bqi),
									&bi,
									sizeof(bi),
									&dwOut,
									NULL))
								{
									//// Only non-UPS system batteries count
									//cout << "BATTERY INFORMATION \n";
									//cout << "Capabilities:" << bi.Capabilities << endl;
									//cout << "Chemistry:" << bi.Chemistry << endl;
									//cout << "CriticalBias:" << bi.CriticalBias << endl;
									//cout << "CycleCount:" << bi.CycleCount << endl;
									//cout << "DefaultAlert1:" << bi.DefaultAlert1 << endl;
									//cout << "DefaultAlert2:" << bi.DefaultAlert2 << endl;
									//cout << "DesignedCapacity:" << bi.DesignedCapacity << endl;
									//cout << "FullChargedCapacity:" << bi.FullChargedCapacity << endl;
									//wcout << "Technology:" << bi.Technology << endl;
									*batInfo = bi;
									CloseHandle(hBattery);
									LocalFree(pdidd);
									SetupDiDestroyDeviceInfoList(hdev);
									return true;
								}

							}
						}
					}
				}
			}
		}
	}
	return false;
}